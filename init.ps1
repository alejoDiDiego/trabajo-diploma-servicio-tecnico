# Read-only harness verification. No build, application startup, database access, or Git writes.
[CmdletBinding()]
param()

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$root = $PSScriptRoot

function Assert-Harness {
    param([bool]$Condition, [string]$Message)
    if (-not $Condition) { throw $Message }
}

function Read-HarnessText {
    param([string]$RelativePath)
    $fullPath = Join-Path $root $RelativePath
    Assert-Harness (Test-Path -LiteralPath $fullPath -PathType Leaf) "Missing file: $RelativePath"
    return [System.IO.File]::ReadAllText($fullPath, [System.Text.Encoding]::UTF8)
}

function Assert-Fields {
    param($Value, [string[]]$Names, [string]$Context)
    Assert-Harness ($null -ne $Value) "Null object: $Context"
    foreach ($name in $Names) {
        Assert-Harness ($null -ne $Value.PSObject.Properties[$name]) "Missing $name in $Context"
    }
}

$agents = Read-HarnessText 'AGENTS.md'
$progress = Read-HarnessText 'progress.md'
$tracker = (Read-HarnessText 'feature_list.json') | ConvertFrom-Json
Assert-Fields $tracker @('schema_version', 'features') 'feature_list.json'
Assert-Harness ($tracker.schema_version -eq 1) 'Unsupported feature tracker schema_version.'
Assert-Harness ($tracker.features -is [array]) 'features must be an array.'
$features = @($tracker.features)
$byId = @{}
$statuses = @('not-started', 'in-progress', 'blocked', 'done')

foreach ($feature in $features) {
    Assert-Fields $feature @('id', 'name', 'description', 'type', 'authorized_files', 'dependencies', 'status', 'acceptance_criteria', 'visual_scenarios', 'evidence') 'feature'
    foreach ($field in @('id', 'name', 'description', 'type', 'status')) {
        Assert-Harness (($feature.$field -is [string]) -and -not [string]::IsNullOrWhiteSpace($feature.$field)) "Invalid $field in feature."
    }
    Assert-Harness (-not $byId.ContainsKey($feature.id)) "Duplicate feature ID: $($feature.id)"
    Assert-Harness ($statuses -contains $feature.status) "Invalid status: $($feature.id)"
    Assert-Harness (@('documentation', 'behavior') -contains $feature.type) "Invalid type: $($feature.id)"
    foreach ($field in @('authorized_files', 'dependencies', 'acceptance_criteria', 'visual_scenarios')) {
        Assert-Harness ($feature.$field -is [array]) "$field must be an array: $($feature.id)"
    }
    Assert-Harness ($feature.authorized_files.Count -gt 0) "Missing authorized file scope: $($feature.id)"
    foreach ($file in $feature.authorized_files) {
        Assert-Harness (($file -is [string]) -and -not [string]::IsNullOrWhiteSpace($file)) "Invalid authorized file: $($feature.id)"
        Assert-Harness (-not [System.IO.Path]::IsPathRooted($file) -and $file -notmatch '(^|[\\/])\.\.([\\/]|$)') "Scope must use repository-relative paths: $($feature.id)"
    }
    Assert-Harness ($feature.evidence -is [string]) "Evidence must be text: $($feature.id)"
    Assert-Harness ($feature.acceptance_criteria.Count -gt 0) "Missing acceptance criteria: $($feature.id)"
    foreach ($criterion in $feature.acceptance_criteria) {
        Assert-Fields $criterion @('description', 'verified') "criterion in $($feature.id)"
        Assert-Harness (($criterion.description -is [string]) -and -not [string]::IsNullOrWhiteSpace($criterion.description)) "Empty criterion: $($feature.id)"
        Assert-Harness ($criterion.verified -is [bool]) "Criterion verified must be Boolean: $($feature.id)"
        if ($feature.status -eq 'done') {
            Assert-Harness $criterion.verified "Unverified completion criterion: $($feature.id)"
        }
    }
    $scenarioIds = @{}
    foreach ($scenario in $feature.visual_scenarios) {
        Assert-Fields $scenario @('id', 'description', 'status', 'evidence') "visual scenario in $($feature.id)"
        foreach ($field in @('id', 'description', 'status', 'evidence')) {
            Assert-Harness ($scenario.$field -is [string]) "Scenario $field must be text: $($feature.id)"
        }
        Assert-Harness (-not [string]::IsNullOrWhiteSpace($scenario.id) -and -not $scenarioIds.ContainsKey($scenario.id)) "Missing/duplicate scenario ID: $($feature.id)"
        Assert-Harness (-not [string]::IsNullOrWhiteSpace($scenario.description)) "Empty scenario description: $($feature.id)"
        $scenarioIds[$scenario.id] = $true
        Assert-Harness (@('planned', 'passed', 'failed', 'blocked') -contains $scenario.status) "Invalid scenario status: $($feature.id)"
        if ($scenario.status -eq 'passed') {
            Assert-Harness (-not [string]::IsNullOrWhiteSpace($scenario.evidence)) "Passed scenario lacks evidence: $($scenario.id)"
        }
        if ($feature.status -eq 'done') {
            Assert-Harness ($scenario.status -eq 'passed') "Incomplete visual scenario: $($scenario.id)"
        }
    }
    if ($feature.type -eq 'behavior' -and $feature.status -ne 'not-started') {
        Assert-Harness ($feature.visual_scenarios.Count -gt 0) "Behavior task needs a visual scenario matrix: $($feature.id)"
    }
    if ($feature.status -eq 'done') {
        Assert-Harness (-not [string]::IsNullOrWhiteSpace($feature.evidence)) "Done feature lacks evidence: $($feature.id)"
    }
    $byId[$feature.id] = $feature
}
Assert-Harness (@($features | Where-Object { $_.status -eq 'in-progress' }).Count -le 1) 'Only one feature may be in-progress.'

foreach ($feature in $features) {
    foreach ($dependency in $feature.dependencies) {
        Assert-Harness (($dependency -is [string]) -and -not [string]::IsNullOrWhiteSpace($dependency)) "Invalid dependency: $($feature.id)"
        Assert-Harness ($byId.ContainsKey($dependency) -and $dependency -ne $feature.id) "Unknown/self dependency in $($feature.id): $dependency"
        if (@('in-progress', 'done') -contains $feature.status) {
            Assert-Harness ($byId[$dependency].status -eq 'done') "Unfinished dependency in $($feature.id): $dependency"
        }
    }
}
$visited = @{}
$visiting = @{}
function Assert-Acyclic {
    param([string]$Id)
    Assert-Harness (-not $visiting.ContainsKey($Id)) "Cyclic dependency at $Id"
    if ($visited.ContainsKey($Id)) { return }
    $visiting[$Id] = $true
    foreach ($dependency in $byId[$Id].dependencies) { Assert-Acyclic $dependency }
    $visiting.Remove($Id)
    $visited[$Id] = $true
}
foreach ($feature in $features) { Assert-Acyclic $feature.id }

foreach ($name in @('feature_list.json', 'progress.md', 'init.ps1')) {
    Assert-Harness ($agents.Contains($name)) "AGENTS.md must route to $name"
}
foreach ($marker in @('Current State', 'Verification Evidence', 'Blockers', 'Next Session')) {
    Assert-Harness ($progress.Contains($marker)) "Missing progress section: $marker"
}

# Check inline relative file links in the harness Markdown, not code blocks or external URLs.
$markdownPaths = @('AGENTS.md', 'progress.md') + @(Get-ChildItem -LiteralPath (Join-Path $root 'docs/agents') -Filter '*.md' -File | ForEach-Object { 'docs/agents/' + $_.Name })
$linkCount = 0
foreach ($relativePath in $markdownPaths) {
    $text = Read-HarnessText $relativePath
    $text = [regex]::Replace($text, '(?ms)^\s*(```|~~~)[^\r\n]*\r?\n.*?^\s*\1[^\r\n]*$', '')
    foreach ($match in [regex]::Matches($text, '\[[^\]\r\n]*\]\(([^)\r\n]+)\)')) {
        $target = $match.Groups[1].Value.Trim()
        if ($target -match '^(#|[a-zA-Z][a-zA-Z0-9+.-]*:|//)') { continue }
        $fileTarget = [uri]::UnescapeDataString(($target -split '#', 2)[0])
        Assert-Harness (-not [System.IO.Path]::IsPathRooted($fileTarget)) "Use a relative link in $relativePath"
        $destination = Join-Path (Split-Path (Join-Path $root $relativePath) -Parent) $fileTarget
        Assert-Harness (Test-Path -LiteralPath $destination) "Broken link in ${relativePath}: $target"
        $linkCount++
    }
}

& git -C $root diff --check
Assert-Harness ($LASTEXITCODE -eq 0) 'git diff --check failed.'
Write-Output "PASS: $($features.Count) task(s), state/completion gates, $linkCount relative file links, and git diff --check."
Write-Output 'Harness checks only; no application build, desktop flow, database, or concurrency verification.'
