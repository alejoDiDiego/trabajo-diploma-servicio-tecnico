# Optional desktop-testing environment (local only)

**Scope:** this guide describes an optional, machine-local toolset for agent-driven visual
validation of the WinForms UI. Nothing in the repository, the build, or the delivered
application depends on it. A person who only wants to use or grade the app can ignore this
guide entirely; the app keeps running with no Python, MCP, or automation software installed.

**Verified implementation (2026-10-03):** the environment below was installed on the author's
Windows 11 machine, validated on a throwaway WinForms window, and used to drive the real
application through the state cycles recorded in the external evidence file
`visual-validation-20261003.md` (kept outside the repository). Tool versions were pinned.

## What it provides

| Piece | Purpose |
| --- | --- |
| Windows-MCP 0.8.7 (Python 3.14) | MCP server the agent can call for screenshots, accessibility-tree snapshots, clicks, typing, shortcuts and waits |
| pywinauto 0.6.9 (Python 3.10) | Deterministic WinForms control automation (read values, enabled/visible state, precise clicks/typing) used by helper scripts |
| OpenCode external config + launcher | Enables the MCP tools only for sessions started through the launcher |

Suggested layout, outside the repository:

```text
%LOCALAPPDATA%\ServicioTecnicoDesktopTests\
├── environments\windows-mcp\      (uv virtualenv, Python 3.14)
├── environments\pywinauto\        (uv virtualenv, Python 3.10)
├── config\opencode.json           (MCP server definition)
├── scripts\                       (launcher, checks, driver scripts)
└── evidence\                      (screenshots, logs, validation record)
```

## Install and verify

Prerequisites: Windows 10/11, `uv`, and local Python 3.14 and 3.10 interpreters
(`py -0p` lists them). PowerShell, from any directory:

```powershell
$base = Join-Path $env:LOCALAPPDATA 'ServicioTecnicoDesktopTests'
New-Item -ItemType Directory -Path $base -Force | Out-Null
uv venv --python 3.14 (Join-Path $base 'environments\windows-mcp')
uv venv --python 3.10 (Join-Path $base 'environments\pywinauto')
uv pip install --python (Join-Path $base 'environments\windows-mcp\Scripts\python.exe') 'windows-mcp==0.8.7'
uv pip install --python (Join-Path $base 'environments\pywinauto\Scripts\python.exe') 'pywinauto==0.6.9' 'Pillow==12.3.0'
```

Then run the read-only check:

```powershell
powershell -NoProfile -File "$env:LOCALAPPDATA\ServicioTecnicoDesktopTests\scripts\check-environment.ps1"
```

It verifies both environments, the pinned versions, and that the input desktop is readable
(an active, unlocked desktop is required for real input). It does not launch the app or touch
the database.

## OpenCode integration

The MCP server is enabled through an external config so the repository stays clean. Create
`config\opencode.json` pointing at the `windows-mcp.exe` inside the virtualenv:

```json
{
  "$schema": "https://opencode.ai/config.json",
  "mcp": {
    "windows-mcp": {
      "type": "local",
      "command": ["<base>\\environments\\windows-mcp\\Scripts\\windows-mcp.exe", "serve"],
      "enabled": true,
      "environment": { "ANONYMIZED_TELEMETRY": "false" }
    }
  }
}
```

Start sessions that should see the tools with a launcher that sets `OPENCODE_CONFIG` to that
file (for example `scripts\start-test-session.cmd`). **Restart OpenCode after changing the
config**; running sessions keep the configuration loaded at startup. Sessions started without
the launcher behave exactly as before and do not require the tools.

## Driver guidance and known limits

- Use Windows-MCP for observation (Snapshot/Screenshot) and general interaction.
- Use pywinauto for precise field work. **Known limitation:** Windows-MCP 0.8.7 omits
  interactive controls whose accessible name is empty. WinForms `TextBox` controls usually
  expose only `AutomationId` (no Name), so they do not appear in its tree; buttons, tabs and
  other controls with visible text do. Fall back to pywinauto (which locates by `AutomationId`)
  or to screenshot coordinates.
- Browser Playwright is not a WinForms driver and is not part of this setup.

## Operating notes for this application

- Detail forms opened from a list are **modal** (`ShowDialog`). While one is open the main
  window stays disabled by design; close the detail (Cerrar) before using the menu.
- MessageBox buttons follow the **operating-system language** (for example Yes/No) even when
  the application UI is in Spanish.
- The dynamic reason dialogs (for example cancel delivery, annul budget) have no control
  names; locate them by the edit/button text.
- Opening the application runs startup migrations/seeds and is a write operation. Only run
  against an authorized non-production database.
- Test data policy: create records with an `AGENT_TEST`/`AGENT_` prefix and record their IDs
  in `progress.md` or the external evidence file. Deletion requires explicit owner approval;
  the 2026-10-03 run intentionally kept its records for later review.

## Evidence and uninstall

Keep screenshots, logs and the validation record under the external `evidence\` directory;
the repository stores only a summary in `progress.md`. To remove the toolset, delete the
`ServicioTecnicoDesktopTests` folder and remove the launcher shortcut; no repository or
application file changes are needed.
