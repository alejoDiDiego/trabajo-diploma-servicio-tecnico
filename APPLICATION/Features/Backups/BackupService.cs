using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DOMAIN.Features.Permisos;
using REPOSITORY.Features.Backups;
using SERVICES.Auth;

namespace APPLICATION.Features.Backups
{
    public sealed class BackupOperacionException : Exception
    {
        public string Clave { get; private set; }
        public string Recuperacion { get; private set; }
        public bool RequiereSalida { get; private set; }
        public BackupOperacionException(string clave, string recuperacion = null, bool requiereSalida = false)
            : base(clave) { Clave = clave; Recuperacion = recuperacion; RequiereSalida = requiereSalida; }
    }
    public sealed class BackupService
    {
        private static readonly SemaphoreSlim Exclusivo = new SemaphoreSlim(1, 1);
        private readonly BackupRepository _repo;
        public BackupService() { _repo = new BackupRepository(); }
        public BackupService(string conexion) { _repo = new BackupRepository(conexion); }
        public string Destino { get { return _repo.Servidor + " / " + _repo.BaseDatos; } }
        public static bool Puede(string permiso)
        { return SessionManager.HaySesionActiva() && SessionManager.TienePermiso(permiso); }
        private static void Autorizar(string permiso)
        { if (!Puede(permiso)) throw new BackupOperacionException("Mensaje.SinPermisos"); }
        public async Task CrearAsync(string destino, bool sobrescribir, IProgress<string> progreso, CancellationToken cancelar)
        {
            Autorizar(CodigosPermiso.BackupsCrear);
            if (!await Exclusivo.WaitAsync(0)) throw new BackupOperacionException("Backups.Ocupado");
            BackupArchivoTransferencia transferencia = null; string servidor = null, temporal = null;
            try
            {
                Autorizar(CodigosPermiso.BackupsCrear);
                destino = Path.GetFullPath(destino);
                if (!Directory.Exists(Path.GetDirectoryName(destino))) throw new IOException();
                temporal = Path.Combine(Path.GetDirectoryName(destino), ".F018_" + Guid.NewGuid().ToString("N") + ".tmp");
                progreso.Report("Backups.Preparando"); cancelar.ThrowIfCancellationRequested();
                var info = await _repo.ObtenerServidorAsync();
                transferencia = await BackupArchivoTransferencia.CrearAsync(_repo.Servidor, info);
                servidor = transferencia.NuevoArchivo();
                progreso.Report("Backups.Creando"); cancelar.ThrowIfCancellationRequested();
                await _repo.CrearAsync(servidor);
                cancelar.ThrowIfCancellationRequested();
                progreso.Report("Backups.Copiando"); await transferencia.ExportarAsync(servidor, temporal);
                cancelar.ThrowIfCancellationRequested(); Autorizar(CodigosPermiso.BackupsCrear);
                BackupArchivoTransferencia.Publicar(temporal, destino, sobrescribir);
            }
            catch (OperationCanceledException) { throw; }
            catch (BackupOperacionException) { throw; }
            catch { throw new BackupOperacionException("Backups.ErrorCrear"); }
            finally
            {
                if (transferencia != null && servidor != null) { try { await transferencia.LimpiarAsync(servidor); } catch { } }
                if (temporal != null) { try { File.Delete(temporal); } catch { } }
                Exclusivo.Release();
            }
        }
        public async Task<string> RestaurarAsync(string archivo, Func<string, Task<bool>> confirmar, IProgress<string> progreso, CancellationToken cancelar)
        {
            Autorizar(CodigosPermiso.BackupsRestaurar);
            if (!await Exclusivo.WaitAsync(0)) throw new BackupOperacionException("Backups.Ocupado");
            BackupArchivoTransferencia transferencia = null; string servidor = null;
            try
            {
                Autorizar(CodigosPermiso.BackupsRestaurar); cancelar.ThrowIfCancellationRequested();
                archivo = Path.GetFullPath(archivo);
                if (!File.Exists(archivo)) throw new IOException();
                progreso.Report("Backups.Preparando");
                var info = await _repo.ObtenerServidorAsync();
                transferencia = await BackupArchivoTransferencia.CrearAsync(_repo.Servidor, info);
                servidor = transferencia.NuevoArchivo();
                await transferencia.ImportarAsync(archivo, servidor);
                cancelar.ThrowIfCancellationRequested(); progreso.Report("Backups.Validando");
                string candidato = await _repo.PrepararRestauracionAsync(servidor, info, cancelar);
                cancelar.ThrowIfCancellationRequested(); Autorizar(CodigosPermiso.BackupsRestaurar);
                if (!await confirmar(Destino)) return null;
                cancelar.ThrowIfCancellationRequested(); Autorizar(CodigosPermiso.BackupsRestaurar);
                progreso.Report("Backups.Activando");
                string recuperacion = await _repo.ActivarAsync(candidato);
                return recuperacion;
            }
            catch (OperationCanceledException) { throw; }
            catch (BackupRecuperacionException ex)
            {
                throw new BackupOperacionException(ex.Compensado ? "Backups.Compensado" : "Backups.Recuperacion",
                    "Original/recovery: " + ex.Original + "\r\nCandidate: " + ex.Candidato + "\r\nTarget: " + Destino, true);
            }
            catch (BackupOperacionException) { throw; }
            catch { throw new BackupOperacionException("Backups.ErrorRestaurar"); }
            finally
            {
                if (transferencia != null && servidor != null) { try { await transferencia.LimpiarAsync(servidor); } catch { } }
                Exclusivo.Release();
            }
        }
    }
}
