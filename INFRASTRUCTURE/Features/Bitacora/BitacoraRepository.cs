using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DOMAIN.Features.Bitacora;

namespace REPOSITORY.Features.Bitacora
{
    public class BitacoraRepository
    {
        private readonly SqlHelper _db;

        public BitacoraRepository()
            : this(ConfigurationManager.ConnectionStrings["UrlDB"].ConnectionString)
        {
        }

        public BitacoraRepository(string cadenaConexion)
        {
            _db = new SqlHelper(cadenaConexion);
        }

        public void Inicializar()
        {
            string query = @"
                IF OBJECT_ID('Bitacora', 'U') IS NULL
                BEGIN
                    CREATE TABLE Bitacora (
                        id_bitacora int IDENTITY(1,1) NOT NULL PRIMARY KEY,
                        fecha datetime NOT NULL,
                        usuario nvarchar(100) NOT NULL,
                        actividad nvarchar(200) NOT NULL,
                        detalle nvarchar(max) NOT NULL DEFAULT '',
                        tipo_actividad nvarchar(50) NOT NULL
                    );
                END
            ";

            _db.ExecuteTransaction(query);
        }

        public void Insertar(EntradaBitacora entrada)
        {
            string query = @"
                INSERT INTO Bitacora (fecha, usuario, actividad, detalle, tipo_actividad)
                VALUES (@Fecha, @Usuario, @Actividad, @Detalle, @TipoActividad);
                SELECT CAST(SCOPE_IDENTITY() AS int);
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Fecha", entrada.Fecha),
                new SqlParameter("@Usuario", entrada.Usuario),
                new SqlParameter("@Actividad", entrada.Actividad),
                new SqlParameter("@Detalle", entrada.Detalle ?? (object)DBNull.Value),
                new SqlParameter("@TipoActividad", entrada.TipoActividad)
            };

            _db.ExecuteTransaction(query, sqlParameters);
        }

        public List<EntradaBitacora> Listar()
        {
            string query = @"
                SELECT id_bitacora, fecha, usuario, actividad, detalle, tipo_actividad
                FROM Bitacora
                ORDER BY fecha DESC;
            ";

            DataTable dt = _db.ExecuteQuery(query);
            List<EntradaBitacora> entradas = new List<EntradaBitacora>();

            foreach (DataRow fila in dt.Rows)
                entradas.Add(CrearDesdeFila(fila));

            return entradas;
        }

        public List<EntradaBitacora> Buscar(string usuario, DateTime? desde, DateTime? hasta, string tipoActividad)
        {
            string query = @"
                SELECT id_bitacora, fecha, usuario, actividad, detalle, tipo_actividad
                FROM Bitacora
                WHERE (@Usuario IS NULL OR usuario LIKE '%' + @Usuario + '%')
                  AND (@Desde IS NULL OR fecha >= @Desde)
                  AND (@Hasta IS NULL OR fecha <= @Hasta)
                  AND (@TipoActividad IS NULL OR tipo_actividad = @TipoActividad)
                ORDER BY fecha DESC;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Usuario", string.IsNullOrWhiteSpace(usuario) ? (object)DBNull.Value : usuario),
                new SqlParameter("@Desde", desde.HasValue ? (object)desde.Value : DBNull.Value),
                new SqlParameter("@Hasta", hasta.HasValue ? (object)hasta.Value : DBNull.Value),
                new SqlParameter("@TipoActividad", string.IsNullOrWhiteSpace(tipoActividad) ? (object)DBNull.Value : tipoActividad)
            };

            DataTable dt = _db.ExecuteQuery(query, sqlParameters);
            List<EntradaBitacora> entradas = new List<EntradaBitacora>();

            foreach (DataRow fila in dt.Rows)
                entradas.Add(CrearDesdeFila(fila));

            return entradas;
        }

        private EntradaBitacora CrearDesdeFila(DataRow fila)
        {
            return EntradaBitacora.Crear(
                Convert.ToInt32(fila["id_bitacora"]),
                Convert.ToDateTime(fila["fecha"]),
                fila["usuario"].ToString(),
                fila["actividad"].ToString(),
                fila["detalle"] == DBNull.Value ? "" : fila["detalle"].ToString(),
                fila["tipo_actividad"].ToString()
            );
        }
    }
}
