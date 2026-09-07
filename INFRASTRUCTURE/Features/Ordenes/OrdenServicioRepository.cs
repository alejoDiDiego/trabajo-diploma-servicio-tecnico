using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DOMAIN.Features.Ordenes;

namespace REPOSITORY.Features.Ordenes
{
    public class OrdenServicioRepository
    {
        private readonly SqlHelper _db;

        public OrdenServicioRepository()
            : this(ConfigurationManager.ConnectionStrings["UrlDB"].ConnectionString)
        {
        }

        public OrdenServicioRepository(string cadenaConexion)
        {
            _db = new SqlHelper(cadenaConexion);
        }

        public void Inicializar()
        {
            // Crea la tabla OrdenesServicio de forma idempotente. FKs con NO ACTION para conservar historia.
            string query = @"
                IF OBJECT_ID('OrdenesServicio', 'U') IS NULL
                BEGIN
                    CREATE TABLE OrdenesServicio (
                        id_orden int IDENTITY(1,1) NOT NULL PRIMARY KEY,
                        numero_orden int NOT NULL,
                        id_cliente int NOT NULL,
                        id_equipo int NOT NULL,
                        id_tecnico_asignado int NULL,
                        id_orden_origen int NULL,
                        tipo_orden nvarchar(20) NOT NULL CONSTRAINT DF_OrdenesServicio_TipoOrden DEFAULT 'Normal',
                        estado nvarchar(50) NOT NULL,
                        resultado nvarchar(50) NULL,
                        fecha_ingreso datetime NOT NULL CONSTRAINT DF_OrdenesServicio_FechaIngreso DEFAULT GETDATE(),
                        problema_informado nvarchar(max) NOT NULL,
                        estado_fisico_ingreso nvarchar(max) NULL,
                        accesorios_ingreso nvarchar(max) NULL,
                        observaciones_ingreso nvarchar(max) NULL,
                        observacion_resultado nvarchar(max) NULL,
                        id_usuario_alta int NOT NULL,
                        CONSTRAINT FK_OrdenesServicio_Clientes FOREIGN KEY (id_cliente)
                            REFERENCES Clientes(id_cliente),
                        CONSTRAINT FK_OrdenesServicio_Equipos FOREIGN KEY (id_equipo)
                            REFERENCES Equipos(id_equipo),
                        CONSTRAINT FK_OrdenesServicio_Tecnico FOREIGN KEY (id_tecnico_asignado)
                            REFERENCES Usuarios(id_usuario),
                        CONSTRAINT FK_OrdenesServicio_Origen FOREIGN KEY (id_orden_origen)
                            REFERENCES OrdenesServicio(id_orden),
                        CONSTRAINT FK_OrdenesServicio_UsuarioAlta FOREIGN KEY (id_usuario_alta)
                            REFERENCES Usuarios(id_usuario)
                    );
                END
                ELSE
                BEGIN
                    IF COL_LENGTH('OrdenesServicio', 'numero_orden') IS NULL
                        ALTER TABLE OrdenesServicio ADD numero_orden int NOT NULL CONSTRAINT DF_OrdenesServicio_Numero DEFAULT 0;

                    IF COL_LENGTH('OrdenesServicio', 'id_cliente') IS NULL
                        ALTER TABLE OrdenesServicio ADD id_cliente int NOT NULL CONSTRAINT DF_OrdenesServicio_IdCliente DEFAULT 0;

                    IF COL_LENGTH('OrdenesServicio', 'id_equipo') IS NULL
                        ALTER TABLE OrdenesServicio ADD id_equipo int NOT NULL CONSTRAINT DF_OrdenesServicio_IdEquipo DEFAULT 0;

                    IF COL_LENGTH('OrdenesServicio', 'id_tecnico_asignado') IS NULL
                        ALTER TABLE OrdenesServicio ADD id_tecnico_asignado int NULL;

                    IF COL_LENGTH('OrdenesServicio', 'id_orden_origen') IS NULL
                        ALTER TABLE OrdenesServicio ADD id_orden_origen int NULL;

                    IF COL_LENGTH('OrdenesServicio', 'tipo_orden') IS NULL
                        ALTER TABLE OrdenesServicio ADD tipo_orden nvarchar(20) NOT NULL CONSTRAINT DF_OrdenesServicio_TipoOrden DEFAULT 'Normal';

                    IF COL_LENGTH('OrdenesServicio', 'estado') IS NULL
                        ALTER TABLE OrdenesServicio ADD estado nvarchar(50) NOT NULL CONSTRAINT DF_OrdenesServicio_Estado DEFAULT 'Recibido';

                    IF COL_LENGTH('OrdenesServicio', 'resultado') IS NULL
                        ALTER TABLE OrdenesServicio ADD resultado nvarchar(50) NULL;

                    IF COL_LENGTH('OrdenesServicio', 'fecha_ingreso') IS NULL
                        ALTER TABLE OrdenesServicio ADD fecha_ingreso datetime NOT NULL CONSTRAINT DF_OrdenesServicio_FechaIngreso DEFAULT GETDATE();

                    IF COL_LENGTH('OrdenesServicio', 'problema_informado') IS NULL
                        ALTER TABLE OrdenesServicio ADD problema_informado nvarchar(max) NOT NULL CONSTRAINT DF_OrdenesServicio_Problema DEFAULT '';

                    IF COL_LENGTH('OrdenesServicio', 'estado_fisico_ingreso') IS NULL
                        ALTER TABLE OrdenesServicio ADD estado_fisico_ingreso nvarchar(max) NULL;

                    IF COL_LENGTH('OrdenesServicio', 'accesorios_ingreso') IS NULL
                        ALTER TABLE OrdenesServicio ADD accesorios_ingreso nvarchar(max) NULL;

                    IF COL_LENGTH('OrdenesServicio', 'observaciones_ingreso') IS NULL
                        ALTER TABLE OrdenesServicio ADD observaciones_ingreso nvarchar(max) NULL;

                    IF COL_LENGTH('OrdenesServicio', 'observacion_resultado') IS NULL
                        ALTER TABLE OrdenesServicio ADD observacion_resultado nvarchar(max) NULL;

                    IF COL_LENGTH('OrdenesServicio', 'id_usuario_alta') IS NULL
                        ALTER TABLE OrdenesServicio ADD id_usuario_alta int NOT NULL CONSTRAINT DF_OrdenesServicio_IdUsuarioAlta DEFAULT 0;

                    IF NOT EXISTS (
                        SELECT 1 FROM sys.foreign_keys
                        WHERE name = 'FK_OrdenesServicio_Clientes'
                          AND parent_object_id = OBJECT_ID('OrdenesServicio')
                    )
                    BEGIN
                        ALTER TABLE OrdenesServicio WITH CHECK
                        ADD CONSTRAINT FK_OrdenesServicio_Clientes FOREIGN KEY (id_cliente)
                            REFERENCES Clientes(id_cliente);
                    END

                    IF NOT EXISTS (
                        SELECT 1 FROM sys.foreign_keys
                        WHERE name = 'FK_OrdenesServicio_Equipos'
                          AND parent_object_id = OBJECT_ID('OrdenesServicio')
                    )
                    BEGIN
                        ALTER TABLE OrdenesServicio WITH CHECK
                        ADD CONSTRAINT FK_OrdenesServicio_Equipos FOREIGN KEY (id_equipo)
                            REFERENCES Equipos(id_equipo);
                    END

                    IF NOT EXISTS (
                        SELECT 1 FROM sys.foreign_keys
                        WHERE name = 'FK_OrdenesServicio_Tecnico'
                          AND parent_object_id = OBJECT_ID('OrdenesServicio')
                    )
                    BEGIN
                        ALTER TABLE OrdenesServicio WITH CHECK
                        ADD CONSTRAINT FK_OrdenesServicio_Tecnico FOREIGN KEY (id_tecnico_asignado)
                            REFERENCES Usuarios(id_usuario);
                    END

                    IF NOT EXISTS (
                        SELECT 1 FROM sys.foreign_keys
                        WHERE name = 'FK_OrdenesServicio_Origen'
                          AND parent_object_id = OBJECT_ID('OrdenesServicio')
                    )
                    BEGIN
                        ALTER TABLE OrdenesServicio WITH CHECK
                        ADD CONSTRAINT FK_OrdenesServicio_Origen FOREIGN KEY (id_orden_origen)
                            REFERENCES OrdenesServicio(id_orden);
                    END

                    IF NOT EXISTS (
                        SELECT 1 FROM sys.foreign_keys
                        WHERE name = 'FK_OrdenesServicio_UsuarioAlta'
                          AND parent_object_id = OBJECT_ID('OrdenesServicio')
                    )
                    BEGIN
                        ALTER TABLE OrdenesServicio WITH CHECK
                        ADD CONSTRAINT FK_OrdenesServicio_UsuarioAlta FOREIGN KEY (id_usuario_alta)
                            REFERENCES Usuarios(id_usuario);
                    END
                END

                IF NOT EXISTS (
                    SELECT 1 FROM sys.indexes
                    WHERE name = 'UX_OrdenesServicio_Numero'
                      AND object_id = OBJECT_ID('OrdenesServicio')
                )
                BEGIN
                    CREATE UNIQUE INDEX UX_OrdenesServicio_Numero
                    ON OrdenesServicio(numero_orden);
                END

                SELECT 0;
            ";

            _db.ExecuteTransaction(query);
        }

        public OrdenServicio CrearConHistorial(OrdenServicio orden, int idUsuario, string observacion)
        {
            // Batch atomico: INSERT orden con numero temporal, numero=id, INSERT historial.
            string query = @"
                INSERT INTO OrdenesServicio (numero_orden, id_cliente, id_equipo, id_tecnico_asignado,
                    id_orden_origen, tipo_orden, estado, resultado, fecha_ingreso, problema_informado,
                    estado_fisico_ingreso, accesorios_ingreso, observaciones_ingreso,
                    observacion_resultado, id_usuario_alta)
                VALUES (0, @IdCliente, @IdEquipo, @IdTecnico, @IdOrdenOrigen, @TipoOrden, @Estado, NULL,
                    @FechaIngreso, @Problema, @EstadoFisico, @Accesorios, @ObsIngreso, NULL, @IdUsuarioAlta);

                DECLARE @Id int = CAST(SCOPE_IDENTITY() AS int);

                UPDATE OrdenesServicio SET numero_orden = @Id WHERE id_orden = @Id;

                INSERT INTO HistorialOrdenes (id_orden, estado_anterior, estado_nuevo, fecha_hora, id_usuario, observacion)
                VALUES (@Id, NULL, @EstadoNuevo, GETDATE(), @IdUsuario, @Observacion);

                SELECT @Id;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdCliente", orden.IdCliente),
                new SqlParameter("@IdEquipo", orden.IdEquipo),
                new SqlParameter("@IdTecnico", orden.IdTecnicoAsignado.HasValue ? (object)orden.IdTecnicoAsignado.Value : DBNull.Value),
                new SqlParameter("@IdOrdenOrigen", orden.IdOrdenOrigen.HasValue ? (object)orden.IdOrdenOrigen.Value : DBNull.Value),
                new SqlParameter("@TipoOrden", orden.TipoOrden),
                new SqlParameter("@Estado", orden.Estado),
                new SqlParameter("@FechaIngreso", orden.FechaIngreso),
                new SqlParameter("@Problema", orden.ProblemaInformado),
                new SqlParameter("@EstadoFisico", (object)orden.EstadoFisicoIngreso ?? DBNull.Value),
                new SqlParameter("@Accesorios", (object)orden.AccesoriosIngreso ?? DBNull.Value),
                new SqlParameter("@ObsIngreso", (object)orden.ObservacionesIngreso ?? DBNull.Value),
                new SqlParameter("@IdUsuarioAlta", orden.IdUsuarioAlta),
                new SqlParameter("@EstadoNuevo", orden.Estado),
                new SqlParameter("@IdUsuario", idUsuario),
                new SqlParameter("@Observacion", (object)observacion ?? DBNull.Value)
            };

            int id = _db.ExecuteTransaction(query, sqlParameters);

            return ObtenerPorId(id);
        }

        public void CambiarEstadoConHistorial(int idOrden, string estadoAnterior, string estadoNuevo,
            int idUsuario, string observacion, string resultado, string observacionResultado, int? idTecnico)
        {
            // Batch atomico: UPDATE orden + INSERT historial. Si falla el historial, rollback total.
            string query = @"
                UPDATE OrdenesServicio
                SET estado = @EstadoNuevo,
                    resultado = COALESCE(@Resultado, resultado),
                    observacion_resultado = COALESCE(@ObservacionResultado, observacion_resultado),
                    id_tecnico_asignado = COALESCE(@IdTecnico, id_tecnico_asignado)
                WHERE id_orden = @Id;

                INSERT INTO HistorialOrdenes (id_orden, estado_anterior, estado_nuevo, fecha_hora, id_usuario, observacion)
                VALUES (@Id, @EstadoAnterior, @EstadoNuevo, GETDATE(), @IdUsuario, @Observacion);

                SELECT 0;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Id", idOrden),
                new SqlParameter("@EstadoAnterior", (object)estadoAnterior ?? DBNull.Value),
                new SqlParameter("@EstadoNuevo", estadoNuevo),
                new SqlParameter("@IdUsuario", idUsuario),
                new SqlParameter("@Observacion", (object)observacion ?? DBNull.Value),
                new SqlParameter("@Resultado", (object)resultado ?? DBNull.Value),
                new SqlParameter("@ObservacionResultado", (object)observacionResultado ?? DBNull.Value),
                new SqlParameter("@IdTecnico", idTecnico.HasValue ? (object)idTecnico.Value : DBNull.Value)
            };

            _db.ExecuteTransaction(query, sqlParameters);
        }

        public void ReabrirPruebasConTransicion(int idOrden, string estadoAnterior, string estadoNuevo,
            int idUsuario, string observacion)
        {
            // Batch atomico: UPDATE orden (ListoRetiro/Reparado -> EnPruebas/NULL) + INSERT historial.
            // Metodo propio porque CambiarEstadoConHistorial usa COALESCE y no puede escribir NULL explicito.
            string query = @"
                UPDATE OrdenesServicio
                SET estado = @EstadoNuevo,
                    resultado = NULL
                WHERE id_orden = @Id;

                INSERT INTO HistorialOrdenes (id_orden, estado_anterior, estado_nuevo, fecha_hora, id_usuario, observacion)
                VALUES (@Id, @EstadoAnterior, @EstadoNuevo, GETDATE(), @IdUsuario, @Observacion);

                SELECT 0;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Id", idOrden),
                new SqlParameter("@EstadoAnterior", (object)estadoAnterior ?? DBNull.Value),
                new SqlParameter("@EstadoNuevo", estadoNuevo),
                new SqlParameter("@IdUsuario", idUsuario),
                new SqlParameter("@Observacion", (object)observacion ?? DBNull.Value)
            };

            _db.ExecuteTransaction(query, sqlParameters);
        }

        public void AsignarTecnico(int idOrden, int idTecnico)
        {
            string query = @"
                UPDATE OrdenesServicio SET id_tecnico_asignado = @IdTecnico WHERE id_orden = @Id;
                SELECT 0;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Id", idOrden),
                new SqlParameter("@IdTecnico", idTecnico)
            };

            _db.ExecuteTransaction(query, sqlParameters);
        }

        public void ActualizarRecepcion(int idOrden, string problema, string estadoFisico,
            string accesorios, string observacionesIngreso)
        {
            string query = @"
                UPDATE OrdenesServicio
                SET problema_informado = @Problema,
                    estado_fisico_ingreso = @EstadoFisico,
                    accesorios_ingreso = @Accesorios,
                    observaciones_ingreso = @ObsIngreso
                WHERE id_orden = @Id;
                SELECT 0;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Id", idOrden),
                new SqlParameter("@Problema", problema),
                new SqlParameter("@EstadoFisico", (object)estadoFisico ?? DBNull.Value),
                new SqlParameter("@Accesorios", (object)accesorios ?? DBNull.Value),
                new SqlParameter("@ObsIngreso", (object)observacionesIngreso ?? DBNull.Value)
            };

            _db.ExecuteTransaction(query, sqlParameters);
        }

        public OrdenServicio ObtenerPorId(int id)
        {
            string query = @"
                SELECT id_orden, numero_orden, id_cliente, id_equipo, id_tecnico_asignado,
                       id_orden_origen, tipo_orden, estado, resultado, fecha_ingreso,
                       problema_informado, estado_fisico_ingreso, accesorios_ingreso,
                       observaciones_ingreso, observacion_resultado, id_usuario_alta
                FROM OrdenesServicio WHERE id_orden = @Id;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Id", id)
            };

            DataTable dt = _db.ExecuteQuery(query, sqlParameters);

            if (dt.Rows.Count <= 0)
                return null;

            return Mapear(dt.Rows[0]);
        }

        public List<OrdenServicio> Listar(bool incluirEntregadas = false)
        {
            string query = @"
                SELECT id_orden, numero_orden, id_cliente, id_equipo, id_tecnico_asignado,
                       id_orden_origen, tipo_orden, estado, resultado, fecha_ingreso,
                       problema_informado, estado_fisico_ingreso, accesorios_ingreso,
                       observaciones_ingreso, observacion_resultado, id_usuario_alta
                FROM OrdenesServicio
                WHERE (@IncluirEntregadas = 1 OR estado <> 'Entregado')
                ORDER BY id_orden;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IncluirEntregadas", incluirEntregadas ? 1 : 0)
            };

            DataTable dt = _db.ExecuteQuery(query, sqlParameters);
            List<OrdenServicio> ordenes = new List<OrdenServicio>();

            foreach (DataRow fila in dt.Rows)
                ordenes.Add(Mapear(fila));

            return ordenes;
        }

        public List<OrdenServicio> ListarPorCliente(int idCliente, bool incluirEntregadas = false)
        {
            string query = @"
                SELECT id_orden, numero_orden, id_cliente, id_equipo, id_tecnico_asignado,
                       id_orden_origen, tipo_orden, estado, resultado, fecha_ingreso,
                       problema_informado, estado_fisico_ingreso, accesorios_ingreso,
                       observaciones_ingreso, observacion_resultado, id_usuario_alta
                FROM OrdenesServicio
                WHERE id_cliente = @IdCliente
                  AND (@IncluirEntregadas = 1 OR estado <> 'Entregado')
                ORDER BY id_orden;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdCliente", idCliente),
                new SqlParameter("@IncluirEntregadas", incluirEntregadas ? 1 : 0)
            };

            DataTable dt = _db.ExecuteQuery(query, sqlParameters);
            List<OrdenServicio> ordenes = new List<OrdenServicio>();

            foreach (DataRow fila in dt.Rows)
                ordenes.Add(Mapear(fila));

            return ordenes;
        }

        private OrdenServicio Mapear(DataRow fila)
        {
            return OrdenServicio.CargarDesdeDB(
                Convert.ToInt32(fila["id_orden"]),
                Convert.ToInt32(fila["numero_orden"]),
                Convert.ToInt32(fila["id_cliente"]),
                Convert.ToInt32(fila["id_equipo"]),
                fila["id_tecnico_asignado"] == DBNull.Value ? (int?)null : Convert.ToInt32(fila["id_tecnico_asignado"]),
                fila["id_orden_origen"] == DBNull.Value ? (int?)null : Convert.ToInt32(fila["id_orden_origen"]),
                fila["tipo_orden"] == DBNull.Value ? null : fila["tipo_orden"].ToString(),
                fila["estado"] == DBNull.Value ? null : fila["estado"].ToString(),
                fila["resultado"] == DBNull.Value ? null : fila["resultado"].ToString(),
                Convert.ToDateTime(fila["fecha_ingreso"]),
                fila["problema_informado"] == DBNull.Value ? "" : fila["problema_informado"].ToString(),
                fila["estado_fisico_ingreso"] == DBNull.Value ? "" : fila["estado_fisico_ingreso"].ToString(),
                fila["accesorios_ingreso"] == DBNull.Value ? "" : fila["accesorios_ingreso"].ToString(),
                fila["observaciones_ingreso"] == DBNull.Value ? "" : fila["observaciones_ingreso"].ToString(),
                fila["observacion_resultado"] == DBNull.Value ? null : fila["observacion_resultado"].ToString(),
                Convert.ToInt32(fila["id_usuario_alta"])
            );
        }
    }
}
