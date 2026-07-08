using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DOMAIN.Features.ControlCambios;

namespace REPOSITORY.Features.ControlCambios
{
    public class ControlCambioRepository
    {
        private readonly SqlHelper _db;

        public ControlCambioRepository()
            : this(ConfigurationManager.ConnectionStrings["UrlDB"].ConnectionString)
        {
        }

        public ControlCambioRepository(string cadenaConexion)
        {
            _db = new SqlHelper(cadenaConexion);
        }

        public void Inicializar()
        {
            string query = @"
                IF OBJECT_ID('ControlCambios', 'U') IS NULL
                BEGIN
                    CREATE TABLE ControlCambios (
                        id_cambio int IDENTITY(1,1) NOT NULL PRIMARY KEY,
                        tabla_afectada nvarchar(100) NOT NULL,
                        id_idioma int NOT NULL,
                        id_palabra int NOT NULL,
                        clave_registro nvarchar(200) NOT NULL,
                        campo_modificado nvarchar(100) NOT NULL,
                        valor_anterior nvarchar(max) NULL,
                        valor_nuevo nvarchar(max) NULL,
                        usuario_modifico nvarchar(100) NOT NULL,
                        fecha_cambio datetime NOT NULL,
                        tipo_cambio nvarchar(10) NOT NULL
                    );
                END

                SELECT 0;
            ";

            _db.ExecuteTransaction(query);
        }

        public void Insertar(ControlCambio cambio)
        {
            string query = @"
                INSERT INTO ControlCambios (tabla_afectada, id_idioma, id_palabra, clave_registro,
                    campo_modificado, valor_anterior, valor_nuevo, usuario_modifico, fecha_cambio, tipo_cambio)
                VALUES (@TablaAfectada, @IdIdioma, @IdPalabra, @ClaveRegistro,
                    @CampoModificado, @ValorAnterior, @ValorNuevo, @UsuarioModifico, @FechaCambio, @TipoCambio);

                SELECT CAST(SCOPE_IDENTITY() AS int);
            ";

            SqlParameter[] parametros = new SqlParameter[]
            {
                new SqlParameter("@TablaAfectada", cambio.TablaAfectada),
                new SqlParameter("@IdIdioma", cambio.IdIdioma),
                new SqlParameter("@IdPalabra", cambio.IdPalabra),
                new SqlParameter("@ClaveRegistro", cambio.ClaveRegistro),
                new SqlParameter("@CampoModificado", cambio.CampoModificado),
                new SqlParameter("@ValorAnterior", (object)cambio.ValorAnterior ?? DBNull.Value),
                new SqlParameter("@ValorNuevo", (object)cambio.ValorNuevo ?? DBNull.Value),
                new SqlParameter("@UsuarioModifico", cambio.UsuarioModifico),
                new SqlParameter("@FechaCambio", cambio.FechaCambio),
                new SqlParameter("@TipoCambio", cambio.TipoCambio)
            };

            _db.ExecuteTransaction(query, parametros);
        }

        public List<ControlCambio> Listar()
        {
            string query = @"
                SELECT cc.id_cambio, cc.tabla_afectada, cc.id_idioma, cc.id_palabra, cc.clave_registro,
                    cc.campo_modificado, cc.valor_anterior, cc.valor_nuevo, cc.usuario_modifico,
                    cc.fecha_cambio, cc.tipo_cambio,
                    CASE WHEN i.id_idioma IS NULL THEN '(eliminado)' ELSE i.nombre END AS nombre_idioma
                FROM ControlCambios cc
                LEFT JOIN Idiomas i ON i.id_idioma = cc.id_idioma
                ORDER BY cc.fecha_cambio DESC
            ";

            DataTable dt = _db.ExecuteQuery(query);
            List<ControlCambio> resultado = new List<ControlCambio>();

            foreach (DataRow fila in dt.Rows)
            {
                resultado.Add(CargarDesdeDB(fila));
            }

            return resultado;
        }

        public ControlCambio ObtenerPorTablaYRegistro(string tablaAfectada, int idIdioma, string tipoCambio)
        {
            string query = @"
                SELECT TOP 1 cc.id_cambio, cc.tabla_afectada, cc.id_idioma, cc.id_palabra, cc.clave_registro,
                    cc.campo_modificado, cc.valor_anterior, cc.valor_nuevo, cc.usuario_modifico,
                    cc.fecha_cambio, cc.tipo_cambio,
                    CASE WHEN i.id_idioma IS NULL THEN '(eliminado)' ELSE i.nombre END AS nombre_idioma
                FROM ControlCambios cc
                LEFT JOIN Idiomas i ON i.id_idioma = cc.id_idioma
                WHERE cc.tabla_afectada = @TablaAfectada
                  AND cc.id_idioma = @IdIdioma
                  AND cc.tipo_cambio = @TipoCambio
                ORDER BY cc.fecha_cambio DESC
            ";

            SqlParameter[] parametros = new SqlParameter[]
            {
                new SqlParameter("@TablaAfectada", tablaAfectada),
                new SqlParameter("@IdIdioma", idIdioma),
                new SqlParameter("@TipoCambio", tipoCambio)
            };

            DataTable dt = _db.ExecuteQuery(query, parametros);

            if (dt.Rows.Count == 0)
                return null;

            return CargarDesdeDB(dt.Rows[0]);
        }

        public ControlCambio ObtenerPorId(int id)
        {
            string query = @"
                SELECT cc.id_cambio, cc.tabla_afectada, cc.id_idioma, cc.id_palabra, cc.clave_registro,
                    cc.campo_modificado, cc.valor_anterior, cc.valor_nuevo, cc.usuario_modifico,
                    cc.fecha_cambio, cc.tipo_cambio,
                    CASE WHEN i.id_idioma IS NULL THEN '(eliminado)' ELSE i.nombre END AS nombre_idioma
                FROM ControlCambios cc
                LEFT JOIN Idiomas i ON i.id_idioma = cc.id_idioma
                WHERE cc.id_cambio = @Id
            ";

            SqlParameter[] parametros = new SqlParameter[]
            {
                new SqlParameter("@Id", id)
            };

            DataTable dt = _db.ExecuteQuery(query, parametros);

            if (dt.Rows.Count == 0)
                return null;

            return CargarDesdeDB(dt.Rows[0]);
        }

        private ControlCambio CargarDesdeDB(DataRow fila)
        {
            return ControlCambio.Crear(
                Convert.ToInt32(fila["id_cambio"]),
                fila["tabla_afectada"].ToString(),
                Convert.ToInt32(fila["id_idioma"]),
                Convert.ToInt32(fila["id_palabra"]),
                fila["clave_registro"].ToString(),
                fila["campo_modificado"].ToString(),
                fila["valor_anterior"] == DBNull.Value ? null : fila["valor_anterior"].ToString(),
                fila["valor_nuevo"] == DBNull.Value ? null : fila["valor_nuevo"].ToString(),
                fila["usuario_modifico"].ToString(),
                Convert.ToDateTime(fila["fecha_cambio"]),
                fila["tipo_cambio"].ToString(),
                fila["nombre_idioma"].ToString()
            );
        }
    }
}
