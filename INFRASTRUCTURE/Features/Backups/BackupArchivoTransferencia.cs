using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace REPOSITORY.Features.Backups
{
    public sealed class BackupArchivoTransferencia
    {
        private readonly string _contenedor;
        private readonly string _carpeta;
        private readonly bool _linux;
        private BackupArchivoTransferencia(string contenedor, string carpeta, bool linux)
        { _contenedor = contenedor; _carpeta = carpeta; _linux = linux; }

        public static async Task<BackupArchivoTransferencia> CrearAsync(string endpoint, BackupServidor servidor)
        {
            string host = endpoint.Replace("tcp:", "").Split(',')[0];
            bool local = host == "." || host.Equals("localhost", StringComparison.OrdinalIgnoreCase) || host == "127.0.0.1"
                || host.Equals(Environment.MachineName, StringComparison.OrdinalIgnoreCase) || host == "(local)"
                || host.StartsWith("(localdb)\\", StringComparison.OrdinalIgnoreCase)
                || host.StartsWith(".\\", StringComparison.OrdinalIgnoreCase)
                || host.StartsWith(Environment.MachineName + "\\", StringComparison.OrdinalIgnoreCase);
            if (!local) throw new InvalidOperationException("Only local SQL file transfer is supported.");
            string carpeta = Environment.GetEnvironmentVariable("TP_BACKUP_STAGING");
            if (string.IsNullOrWhiteSpace(carpeta)) carpeta = servidor.CarpetaBackup;
            if (servidor.Linux)
            {
                if (!carpeta.StartsWith("/", StringComparison.Ordinal) || carpeta.Contains("..") || carpeta.IndexOfAny(new[] { '\r', '\n', '\0' }) >= 0)
                    throw new InvalidOperationException();
                string contenedor = Environment.GetEnvironmentVariable("TP_BACKUP_DOCKER_CONTAINER");
                if (string.IsNullOrWhiteSpace(contenedor)) contenedor = null;
                string port = endpoint.Contains(",") ? endpoint.Split(',').Last() : "1433";
                if (!Regex.IsMatch(port, "^[0-9]{1,5}$")) throw new InvalidOperationException();
                if (string.IsNullOrWhiteSpace(contenedor))
                {
                    string ids = await Docker("ps", "--filter", "publish=" + port, "--format", "{{.ID}}").ConfigureAwait(false);
                    string[] encontrados = ids.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string id in encontrados)
                    {
                        string nombre = (await Docker("inspect", "--format", "{{.Config.Hostname}}", id).ConfigureAwait(false)).Trim();
                        if (nombre.Equals(servidor.Maquina, StringComparison.OrdinalIgnoreCase))
                        { if (contenedor != null) throw new InvalidOperationException(); contenedor = id; }
                    }
                }
                if (!Regex.IsMatch(contenedor ?? "", "^[A-Za-z0-9][A-Za-z0-9_.-]{0,127}$")) throw new InvalidOperationException();
                string hostname = (await Docker("inspect", "--format", "{{.Config.Hostname}}", contenedor).ConfigureAwait(false)).Trim();
                if (!hostname.Equals(servidor.Maquina, StringComparison.OrdinalIgnoreCase)) throw new InvalidOperationException();
                string published = await Docker("port", contenedor, "1433/tcp").ConfigureAwait(false);
                if (!published.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Any(x => x.EndsWith(":" + port, StringComparison.Ordinal)))
                    throw new InvalidOperationException();
                return new BackupArchivoTransferencia(contenedor, carpeta.TrimEnd('/'), true);
            }
            if (!Path.IsPathRooted(carpeta) || !Directory.Exists(carpeta)) throw new InvalidOperationException();
            return new BackupArchivoTransferencia(null, Path.GetFullPath(carpeta).TrimEnd('\\'), false);
        }
        public string NuevoArchivo()
        { return _carpeta + (_linux ? "/" : "\\") + "F018_transfer_" + Guid.NewGuid().ToString("N") + ".bak"; }
        private void ValidarPropio(string ruta)
        {
            string prefijo = _carpeta + (_linux ? "/" : "\\");
            if (!ruta.StartsWith(prefijo, StringComparison.Ordinal) || !Regex.IsMatch(ruta.Substring(prefijo.Length), "^F018_transfer_[a-f0-9]{32}\\.bak$"))
                throw new InvalidOperationException();
        }
        public async Task ImportarAsync(string local, string servidor)
        {
            ValidarPropio(servidor);
            if (_linux) await Docker("cp", Path.GetFullPath(local), _contenedor + ":" + servidor).ConfigureAwait(false);
            else await Task.Run(() => File.Copy(local, servidor, false)).ConfigureAwait(false);
        }
        public async Task ExportarAsync(string servidor, string temporal)
        {
            ValidarPropio(servidor);
            if (_linux) await Docker("cp", _contenedor + ":" + servidor, temporal).ConfigureAwait(false);
            else await Task.Run(() => File.Copy(servidor, temporal, false)).ConfigureAwait(false);
        }
        public async Task LimpiarAsync(string servidor)
        {
            ValidarPropio(servidor);
            if (_linux) await Docker("exec", _contenedor, "rm", "--", servidor).ConfigureAwait(false);
            else File.Delete(servidor);
        }
        public static void Publicar(string temporal, string destino, bool sobrescribir)
        {
            if (sobrescribir) File.Replace(temporal, destino, null);
            else File.Move(temporal, destino);
        }
        private static string Citar(string valor)
        {
            if (valor.IndexOfAny(new[] { '\0', '\r', '\n' }) >= 0) throw new ArgumentException();
            return "\"" + Regex.Replace(valor, "(\\\\*)\"", "$1$1\\\"").TrimEnd('\\')
                + new string('\\', valor.Reverse().TakeWhile(x => x == '\\').Count() * 2) + "\"";
        }
        private static async Task<string> Docker(params string[] argumentos)
        {
            using (var p = new Process())
            {
                p.StartInfo = new ProcessStartInfo("docker.exe", string.Join(" ", argumentos.Select(Citar)))
                { UseShellExecute = false, CreateNoWindow = true, RedirectStandardOutput = true, RedirectStandardError = true };
                var salida = new StringBuilder(); var error = new StringBuilder();
                p.OutputDataReceived += (s, e) => { if (e.Data != null) lock (salida) { if (salida.Length < 16384) salida.AppendLine(e.Data); } };
                p.ErrorDataReceived += (s, e) => { if (e.Data != null) lock (error) { if (error.Length < 2048) error.AppendLine(e.Data); } };
                p.Start(); p.BeginOutputReadLine(); p.BeginErrorReadLine();
                await Task.Run(() => p.WaitForExit()).ConfigureAwait(false);
                if (p.ExitCode != 0) throw new IOException("Docker file transfer failed.");
                return salida.ToString();
            }
        }
    }
}
