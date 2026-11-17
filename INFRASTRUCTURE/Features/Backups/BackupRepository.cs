using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace REPOSITORY.Features.Backups
{
    public sealed class BackupRecuperacionException : Exception
    {
        public string Original { get; private set; }
        public string Candidato { get; private set; }
        public bool Compensado { get; private set; }
        public BackupRecuperacionException(string original, string candidato, bool compensado)
            : base("Backup recovery requires operator inspection.")
        { Original = original; Candidato = candidato; Compensado = compensado; }
    }

    public sealed class BackupServidor
    {
        public string Maquina { get; internal set; }
        public string CarpetaBackup { get; internal set; }
        public string CarpetaDatos { get; internal set; }
        public string CarpetaLog { get; internal set; }
        public bool Linux { get { return CarpetaDatos.StartsWith("/", StringComparison.Ordinal); } }
    }

    public sealed class BackupRepository
    {
        private readonly string _master;
        public string BaseDatos { get; private set; }
        public string Servidor { get; private set; }
        public BackupRepository() : this(ConfigurationManager.ConnectionStrings["UrlDB"].ConnectionString) { }
        public BackupRepository(string conexion)
        {
            var b = new SqlConnectionStringBuilder(conexion);
            BaseDatos = b.InitialCatalog;
            Identificador(BaseDatos);
            if (string.IsNullOrWhiteSpace(BaseDatos) || BaseDatos.Equals("master", StringComparison.OrdinalIgnoreCase)
                || BaseDatos.Equals("model", StringComparison.OrdinalIgnoreCase) || BaseDatos.Equals("msdb", StringComparison.OrdinalIgnoreCase)
                || BaseDatos.Equals("tempdb", StringComparison.OrdinalIgnoreCase)) throw new InvalidOperationException();
            Servidor = b.DataSource;
            b.InitialCatalog = "master"; b.Pooling = false; b.Enlist = false;
            b.ConnectTimeout = 10; b.ApplicationName = "TPIntegrador Backups";
            _master = b.ConnectionString;
        }
        private SqlConnection Conexion() { return new SqlConnection(_master); }
        public static string Identificador(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor) || valor.Length > 128 || System.Linq.Enumerable.Any(valor, char.IsControl))
                throw new ArgumentException("Invalid database identifier.");
            return "[" + valor.Replace("]", "]]") + "]";
        }
        private static SqlCommand Comando(SqlConnection c, string sql, params SqlParameter[] parametros)
        {
            var q = c.CreateCommand(); q.CommandText = sql; q.CommandTimeout = 0;
            q.Parameters.AddRange(parametros); return q;
        }
        private static SqlParameter P(string nombre, string valor) { return new SqlParameter(nombre, SqlDbType.NVarChar, 4000) { Value = valor }; }
        private static async Task Ejecutar(SqlConnection c, string sql, params SqlParameter[] p)
        { using (var q = Comando(c, sql, p)) { await q.ExecuteNonQueryAsync().ConfigureAwait(false); } }
        private static async Task<DataTable> Consultar(SqlConnection c, string sql, params SqlParameter[] p)
        {
            using (var q = Comando(c, sql, p))
            using (var r = await q.ExecuteReaderAsync().ConfigureAwait(false))
            { var t = new DataTable(); t.Load(r); return t; }
        }
        public async Task<BackupServidor> ObtenerServidorAsync()
        {
            using (var c = Conexion())
            {
                await c.OpenAsync().ConfigureAwait(false);
                var t = await Consultar(c, @"SELECT CONVERT(nvarchar(128),SERVERPROPERTY('MachineName')) maquina,
                    CONVERT(nvarchar(4000),SERVERPROPERTY('InstanceDefaultBackupPath')) backup_path,
                    CONVERT(nvarchar(4000),SERVERPROPERTY('InstanceDefaultDataPath')) data_path,
                    CONVERT(nvarchar(4000),SERVERPROPERTY('InstanceDefaultLogPath')) log_path;").ConfigureAwait(false);
                var r = t.Rows[0];
                var s = new BackupServidor { Maquina = r["maquina"].ToString(), CarpetaBackup = r["backup_path"].ToString(),
                    CarpetaDatos = r["data_path"].ToString(), CarpetaLog = r["log_path"].ToString() };
                if (string.IsNullOrWhiteSpace(s.CarpetaDatos) || string.IsNullOrWhiteSpace(s.CarpetaLog)) throw new InvalidOperationException();
                if (string.IsNullOrWhiteSpace(s.CarpetaBackup)) s.CarpetaBackup = s.CarpetaDatos;
                return s;
            }
        }
        public async Task CrearAsync(string archivo)
        {
            using (var c = Conexion())
            {
                await c.OpenAsync().ConfigureAwait(false);
                await Ejecutar(c, "BACKUP DATABASE " + Identificador(BaseDatos) + " TO DISK=@p WITH COPY_ONLY,CHECKSUM,INIT;", P("@p", archivo)).ConfigureAwait(false);
                await Ejecutar(c, "RESTORE VERIFYONLY FROM DISK=@p WITH CHECKSUM;", P("@p", archivo)).ConfigureAwait(false);
            }
        }
        public async Task<string> PrepararRestauracionAsync(string archivo, BackupServidor servidor, CancellationToken cancelacion)
        {
            using (var c = Conexion())
            {
                await c.OpenAsync().ConfigureAwait(false);
                var h = await Consultar(c, "RESTORE HEADERONLY FROM DISK=@p;", P("@p", archivo)).ConfigureAwait(false);
                if (h.Rows.Count != 1) throw new InvalidOperationException();
                var r = h.Rows[0];
                if (Convert.ToInt32(r["BackupType"]) != 1 || !Convert.ToBoolean(r["HasBackupChecksums"])
                    || Convert.ToBoolean(r["IsDamaged"]) || (h.Columns.Contains("KeyAlgorithm") && r["KeyAlgorithm"] != DBNull.Value
                    && !string.IsNullOrEmpty(r["KeyAlgorithm"].ToString()))) throw new InvalidOperationException();
                await Ejecutar(c, "RESTORE VERIFYONLY FROM DISK=@p WITH CHECKSUM;", P("@p", archivo)).ConfigureAwait(false);
                var f = await Consultar(c, "RESTORE FILELISTONLY FROM DISK=@p;", P("@p", archivo)).ConfigureAwait(false);
                if (f.Rows.Count < 2) throw new InvalidOperationException();
                string candidato = BaseDatos.Substring(0, Math.Min(70, BaseDatos.Length)) + "_F018_candidate_" + Guid.NewGuid().ToString("N");
                var sql = new StringBuilder("RESTORE DATABASE " + Identificador(candidato) + " FROM DISK=@p WITH CHECKSUM,RECOVERY");
                var parametros = new List<SqlParameter> { P("@p", archivo) };
                int i = 0; bool datos = false, log = false;
                foreach (DataRow file in f.Rows)
                {
                    string tipo = file["Type"].ToString();
                    if (tipo != "D" && tipo != "L") throw new InvalidOperationException();
                    datos |= tipo == "D"; log |= tipo == "L";
                    string carpeta = tipo == "L" ? servidor.CarpetaLog : servidor.CarpetaDatos;
                    string destino = carpeta.TrimEnd('/', '\\') + (servidor.Linux ? "/" : "\\") + candidato + "_" + i + (tipo == "L" ? ".ldf" : ".mdf");
                    sql.Append(", MOVE @l" + i + " TO @f" + i);
                    parametros.Add(P("@l" + i, file["LogicalName"].ToString())); parametros.Add(P("@f" + i, destino)); i++;
                }
                if (!datos || !log) throw new InvalidOperationException();
                cancelacion.ThrowIfCancellationRequested();
                await Ejecutar(c, sql.ToString(), parametros.ToArray()).ConfigureAwait(false);
                cancelacion.ThrowIfCancellationRequested();
                var estado = await Consultar(c, "SELECT state_desc FROM sys.databases WHERE name=@n;", P("@n", candidato)).ConfigureAwait(false);
                if (estado.Rows.Count != 1 || estado.Rows[0][0].ToString() != "ONLINE") throw new InvalidOperationException();
                var errores = await Consultar(c, "DBCC CHECKDB (" + Identificador(candidato) + ") WITH TABLERESULTS,NO_INFOMSGS,ALL_ERRORMSGS;").ConfigureAwait(false);
                if (errores.Rows.Count != 0) throw new InvalidOperationException();
                string[] tablas = { "Usuarios", "Permisos", "PermisoComposicion", "UsuarioPermisos", "Idiomas", "Palabras", "Traducciones", "Bitacora", "ControlCambios", "DigitosVerticales", "TiposEquipo", "Marcas", "Clientes", "Equipos", "OrdenesServicio", "HistorialOrdenes", "Diagnosticos", "Presupuestos", "PresupuestoDetalle", "Entregas", "Repuestos", "MovimientosStock", "Proveedores", "Compras", "CompraDetalle", "Reparaciones", "ReparacionRepuesto", "Pruebas", "Garantias", "EvaluacionesGarantia" };
                foreach (string tabla in tablas)
                    await Ejecutar(c, "SELECT TOP (0) * FROM " + Identificador(candidato) + ".dbo." + Identificador(tabla) + ";").ConfigureAwait(false);
                await Ejecutar(c, "SELECT TOP (0) id_usuario,username,password,activo,dvh FROM " + Identificador(candidato) + ".dbo.Usuarios;").ConfigureAwait(false);
                return candidato;
            }
        }
        private static async Task<int> Id(SqlConnection c, string nombre)
        {
            using (var q = Comando(c, "SELECT ISNULL(DB_ID(@n),0);", P("@n", nombre)))
                return Convert.ToInt32(await q.ExecuteScalarAsync().ConfigureAwait(false));
        }
        public async Task<string> ActivarAsync(string candidato)
        {
            if (candidato == null || !candidato.StartsWith(BaseDatos.Substring(0, Math.Min(70, BaseDatos.Length)) + "_F018_candidate_", StringComparison.Ordinal) || !System.Text.RegularExpressions.Regex.IsMatch(candidato, "_F018_candidate_[a-f0-9]{32}$")) throw new ArgumentException();
            string recuperacion = BaseDatos.Substring(0, Math.Min(70, BaseDatos.Length)) + "_F018_recovery_" + Guid.NewGuid().ToString("N");
            int originalId = 0, candidatoId = 0; bool inicio = false;
            using (var c = Conexion())
            {
                await c.OpenAsync().ConfigureAwait(false);
                await Ejecutar(c, @"DECLARE @r int; EXEC @r=sys.sp_getapplock @Resource=@n,@LockMode='Exclusive',@LockOwner='Session',@LockTimeout=0; IF @r<0 THROW 50000,'Maintenance busy',1;", P("@n", "TPIntegrador.Backup." + BaseDatos)).ConfigureAwait(false);
                originalId = await Id(c, BaseDatos).ConfigureAwait(false); candidatoId = await Id(c, candidato).ConfigureAwait(false);
                if (originalId <= 4 || candidatoId <= 4) throw new InvalidOperationException();
                try
                {
                    SqlConnection.ClearAllPools();
                    await Ejecutar(c, @"IF EXISTS(SELECT 1 FROM sys.dm_exec_sessions WHERE is_user_process=1 AND session_id<>@@SPID AND database_id=DB_ID(@n))
                        OR EXISTS(SELECT 1 FROM sys.dm_tran_locks WHERE resource_database_id=DB_ID(@n) AND request_session_id<>@@SPID)
                        THROW 50000,'Database in use',1;", P("@n", BaseDatos)).ConfigureAwait(false);
                    string carpeta = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TPIntegrador", "Backups");
                    System.IO.Directory.CreateDirectory(carpeta);
                    System.IO.File.WriteAllText(System.IO.Path.Combine(carpeta, recuperacion + ".txt"),
                        "UTC: " + DateTime.UtcNow.ToString("O") + "\r\nServer: " + Servidor + "\r\nTarget: " + BaseDatos
                        + "\r\nOriginal database ID: " + originalId + "\r\nRecovery name: " + recuperacion
                        + "\r\nCandidate database ID: " + candidatoId + "\r\nCandidate name: " + candidato
                        + "\r\nInspect sys.databases by these IDs before recovery. No automatic rollback after process loss. Do not delete retained databases.");
                    inicio = true;
                    await Ejecutar(c, "ALTER DATABASE " + Identificador(BaseDatos) + " SET SINGLE_USER WITH NO_WAIT;").ConfigureAwait(false);
                    await Ejecutar(c, "ALTER DATABASE " + Identificador(BaseDatos) + " MODIFY NAME = " + Identificador(recuperacion) + ";").ConfigureAwait(false);
                    await Ejecutar(c, "ALTER DATABASE " + Identificador(candidato) + " SET SINGLE_USER WITH NO_WAIT; ALTER DATABASE " + Identificador(candidato) + " MODIFY NAME = " + Identificador(BaseDatos) + ";").ConfigureAwait(false);
                    await Ejecutar(c, "ALTER DATABASE " + Identificador(BaseDatos) + " SET MULTI_USER WITH NO_WAIT; ALTER DATABASE " + Identificador(recuperacion) + " SET MULTI_USER WITH NO_WAIT;").ConfigureAwait(false);
                    return recuperacion;
                }
                catch
                {
                    if (!inicio) throw;
                    bool compensado = false;
                    try { c.Close(); } catch { }
                    try
                    {
                        using (var rc = Conexion())
                        {
                            await rc.OpenAsync().ConfigureAwait(false);
                            await Ejecutar(rc, @"DECLARE @r int; EXEC @r=sys.sp_getapplock @Resource=@n,@LockMode='Exclusive',@LockOwner='Session',@LockTimeout=0; IF @r<0 THROW 50000,'Maintenance busy',1;", P("@n", "TPIntegrador.Backup." + BaseDatos)).ConfigureAwait(false);
                            if (await Id(rc, BaseDatos).ConfigureAwait(false) == candidatoId)
                            {
                                if (await Id(rc, candidato).ConfigureAwait(false) != 0) throw new InvalidOperationException();
                                await Ejecutar(rc, "ALTER DATABASE " + Identificador(BaseDatos) + " SET SINGLE_USER WITH NO_WAIT; ALTER DATABASE " + Identificador(BaseDatos) + " MODIFY NAME = " + Identificador(candidato) + ";").ConfigureAwait(false);
                            }
                            if (await Id(rc, BaseDatos).ConfigureAwait(false) == 0 && await Id(rc, recuperacion).ConfigureAwait(false) == originalId)
                                await Ejecutar(rc, "ALTER DATABASE " + Identificador(recuperacion) + " MODIFY NAME = " + Identificador(BaseDatos) + ";").ConfigureAwait(false);
                            if (await Id(rc, BaseDatos).ConfigureAwait(false) != originalId) throw new InvalidOperationException();
                            await Ejecutar(rc, "ALTER DATABASE " + Identificador(BaseDatos) + " SET MULTI_USER WITH NO_WAIT;").ConfigureAwait(false);
                            compensado = true;
                        }
                    }
                    catch { }
                    throw new BackupRecuperacionException(recuperacion, candidato, compensado);
                }
            }
        }
    }
}
