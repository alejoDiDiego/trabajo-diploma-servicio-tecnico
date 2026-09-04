using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DOMAIN.Features.Ordenes;

namespace REPOSITORY.Features.Ordenes
{
    public class HistorialEstadoOrdenRepository
    {
        private readonly SqlHelper _db;

        public HistorialEstadoOrdenRepository()
            : this(ConfigurationManager.ConnectionStrings["UrlDB"].ConnectionString)
        {
        }

        public HistorialEstadoOrdenRepository(string cadenaConexion)
        {
            _db = new SqlHelper(cadenaConexion);
        }

        public void Inicializar()
        {
            string query = @"
                IF OBJECT_ID('HistorialOrdenes', 'U') IS NULL
                BEGIN
                    CREATE TABLE HistorialOrdenes (
                        id_historial int IDENTITY(1,1) NOT NULL PRIMARY KEY,
                        id_orden int NOT NULL,
                        estado_anterior nvarchar(50) NULL,
                        estado_nuevo nvarchar(50) NOT NULL,
                        fecha_hora datetime NOT NULL CONSTRAINT DF_HistorialOrdenes_FechaHora DEFAULT GETDATE(),
                        id_usuario int NOT NULL,
                        observacion nvarchar(max) NULL,
                        CONSTRAINT FK_HistorialOrdenes_Orden FOREIGN KEY (id_orden)
                            REFERENCES OrdenesServicio(id_orden),
                        CONSTRAINT FK_HistorialOrdenes_Usuario FOREIGN KEY (id_usuario)
                            REFERENCES Usuarios(id_usuario)
                    );
                END
                ELSE
                BEGIN
                    IF COL_LENGTH('HistorialOrdenes', 'id_orden') IS NULL
                        ALTER TABLE HistorialOrdenes ADD id_orden int NOT NULL CONSTRAINT DF_HistorialOrdenes_IdOrden DEFAULT 0;

                    IF COL_LENGTH('HistorialOrdenes', 'estado_anterior') IS NULL
                        ALTER TABLE HistorialOrdenes ADD estado_anterior nvarchar(50) NULL;

                    IF COL_LENGTH('HistorialOrdenes', 'estado_nuevo') IS NULL
                        ALTER TABLE HistorialOrdenes ADD estado_nuevo nvarchar(50) NOT NULL CONSTRAINT DF_HistorialOrdenes_EstadoNuevo DEFAULT '';

                    IF COL_LENGTH('HistorialOrdenes', 'fecha_hora') IS NULL
                        ALTER TABLE HistorialOrdenes ADD fecha_hora datetime NOT NULL CONSTRAINT DF_HistorialOrdenes_FechaHora DEFAULT GETDATE();

                    IF COL_LENGTH('HistorialOrdenes', 'id_usuario') IS NULL
                        ALTER TABLE HistorialOrdenes ADD id_usuario int NOT NULL CONSTRAINT DF_HistorialOrdenes_IdUsuario DEFAULT 0;

                    IF COL_LENGTH('HistorialOrdenes', 'observacion') IS NULL
                        ALTER TABLE HistorialOrdenes ADD observacion nvarchar(max) NULL;

                    IF NOT EXISTS (
                        SELECT 1 FROM sys.foreign_keys
                        WHERE name = 'FK_HistorialOrdenes_Orden'
                          AND parent_object_id = OBJECT_ID('HistorialOrdenes')
                    )
                    BEGIN
                        ALTER TABLE HistorialOrdenes WITH CHECK
                        ADD CONSTRAINT FK_HistorialOrdenes_Orden FOREIGN KEY (id_orden)
                            REFERENCES OrdenesServicio(id_orden);
                    END

                    IF NOT EXISTS (
                        SELECT 1 FROM sys.foreign_keys
                        WHERE name = 'FK_HistorialOrdenes_Usuario'
                          AND parent_object_id = OBJECT_ID('HistorialOrdenes')
                    )
                    BEGIN
                        ALTER TABLE HistorialOrdenes WITH CHECK
                        ADD CONSTRAINT FK_HistorialOrdenes_Usuario FOREIGN KEY (id_usuario)
                            REFERENCES Usuarios(id_usuario);
                    END
                END

                SELECT 0;
            ";

            _db.ExecuteTransaction(query);
        }

        public HistorialEstadoOrden Insertar(HistorialEstadoOrden historial)
        {
            string query = @"
                INSERT INTO HistorialOrdenes (id_orden, estado_anterior, estado_nuevo, fecha_hora, id_usuario, observacion)
                VALUES (@IdOrden, @EstadoAnterior, @EstadoNuevo, @FechaHora, @IdUsuario, @Observacion);
                SELECT CAST(SCOPE_IDENTITY() AS int);
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdOrden", historial.IdOrden),
                new SqlParameter("@EstadoAnterior", (object)historial.EstadoAnterior ?? DBNull.Value),
                new SqlParameter("@EstadoNuevo", historial.EstadoNuevo),
                new SqlParameter("@FechaHora", historial.FechaHora),
                new SqlParameter("@IdUsuario", historial.IdUsuario),
                new SqlParameter("@Observacion", (object)historial.Observacion ?? DBNull.Value)
            };

            int id = _db.ExecuteTransaction(query, sqlParameters);

            return HistorialEstadoOrden.CargarDesdeDB(id, historial.IdOrden, historial.EstadoAnterior,
                historial.EstadoNuevo, historial.FechaHora, historial.IdUsuario, historial.Observacion);
        }

        public List<HistorialEstadoOrden> ListarPorOrden(int idOrden)
        {
            string query = @"
                SELECT id_historial, id_orden, estado_anterior, estado_nuevo, fecha_hora, id_usuario, observacion
                FROM HistorialOrdenes
                WHERE id_orden = @IdOrden
                ORDER BY fecha_hora, id_historial;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdOrden", idOrden)
            };

            DataTable dt = _db.ExecuteQuery(query, sqlParameters);
            List<HistorialEstadoOrden> historial = new List<HistorialEstadoOrden>();

            foreach (DataRow fila in dt.Rows)
            {
                historial.Add(HistorialEstadoOrden.CargarDesdeDB(
                    Convert.ToInt32(fila["id_historial"]),
                    Convert.ToInt32(fila["id_orden"]),
                    fila["estado_anterior"] == DBNull.Value ? null : fila["estado_anterior"].ToString(),
                    fila["estado_nuevo"] == DBNull.Value ? "" : fila["estado_nuevo"].ToString(),
                    Convert.ToDateTime(fila["fecha_hora"]),
                    Convert.ToInt32(fila["id_usuario"]),
                    fila["observacion"] == DBNull.Value ? "" : fila["observacion"].ToString()
                ));
            }

            return historial;
        }
    }
}
