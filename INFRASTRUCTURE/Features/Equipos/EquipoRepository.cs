using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DOMAIN.Features.Equipos;

namespace REPOSITORY.Features.Equipos
{
    public class EquipoRepository
    {
        private readonly SqlHelper _db;

        public EquipoRepository()
            : this(ConfigurationManager.ConnectionStrings["UrlDB"].ConnectionString)
        {
        }

        public EquipoRepository(string cadenaConexion)
        {
            _db = new SqlHelper(cadenaConexion);
        }

        public void Inicializar()
        {
            // Crea la tabla Equipos de forma idempotente. FKs con NO ACTION para conservar historia.
            string query = @"
                IF OBJECT_ID('Equipos', 'U') IS NULL
                BEGIN
                    CREATE TABLE Equipos (
                        id_equipo int IDENTITY(1,1) NOT NULL PRIMARY KEY,
                        id_cliente int NOT NULL,
                        id_tipo_equipo int NOT NULL,
                        id_marca int NOT NULL,
                        modelo nvarchar(150) NULL,
                        numero_serie nvarchar(150) NULL,
                        imei nvarchar(100) NULL,
                        color nvarchar(100) NULL,
                        observaciones nvarchar(max) NULL,
                        activo bit NOT NULL CONSTRAINT DF_Equipos_Activo DEFAULT 1,
                        CONSTRAINT FK_Equipos_Clientes FOREIGN KEY (id_cliente)
                            REFERENCES Clientes(id_cliente),
                        CONSTRAINT FK_Equipos_TiposEquipo FOREIGN KEY (id_tipo_equipo)
                            REFERENCES TiposEquipo(id_tipo_equipo),
                        CONSTRAINT FK_Equipos_Marcas FOREIGN KEY (id_marca)
                            REFERENCES Marcas(id_marca)
                    );
                END
                ELSE
                BEGIN
                    IF COL_LENGTH('Equipos', 'id_cliente') IS NULL
                        ALTER TABLE Equipos ADD id_cliente int NOT NULL CONSTRAINT DF_Equipos_IdCliente DEFAULT 0;

                    IF COL_LENGTH('Equipos', 'id_tipo_equipo') IS NULL
                        ALTER TABLE Equipos ADD id_tipo_equipo int NOT NULL CONSTRAINT DF_Equipos_IdTipo DEFAULT 0;

                    IF COL_LENGTH('Equipos', 'id_marca') IS NULL
                        ALTER TABLE Equipos ADD id_marca int NOT NULL CONSTRAINT DF_Equipos_IdMarca DEFAULT 0;

                    IF COL_LENGTH('Equipos', 'modelo') IS NULL
                        ALTER TABLE Equipos ADD modelo nvarchar(150) NULL;

                    IF COL_LENGTH('Equipos', 'numero_serie') IS NULL
                        ALTER TABLE Equipos ADD numero_serie nvarchar(150) NULL;

                    IF COL_LENGTH('Equipos', 'imei') IS NULL
                        ALTER TABLE Equipos ADD imei nvarchar(100) NULL;

                    IF COL_LENGTH('Equipos', 'color') IS NULL
                        ALTER TABLE Equipos ADD color nvarchar(100) NULL;

                    IF COL_LENGTH('Equipos', 'observaciones') IS NULL
                        ALTER TABLE Equipos ADD observaciones nvarchar(max) NULL;

                    IF COL_LENGTH('Equipos', 'activo') IS NULL
                        ALTER TABLE Equipos ADD activo bit NOT NULL CONSTRAINT DF_Equipos_Activo DEFAULT 1;

                    IF NOT EXISTS (
                        SELECT 1 FROM sys.foreign_keys
                        WHERE name = 'FK_Equipos_Clientes'
                          AND parent_object_id = OBJECT_ID('Equipos')
                    )
                    BEGIN
                        ALTER TABLE Equipos WITH CHECK
                        ADD CONSTRAINT FK_Equipos_Clientes FOREIGN KEY (id_cliente)
                            REFERENCES Clientes(id_cliente);
                    END

                    IF NOT EXISTS (
                        SELECT 1 FROM sys.foreign_keys
                        WHERE name = 'FK_Equipos_TiposEquipo'
                          AND parent_object_id = OBJECT_ID('Equipos')
                    )
                    BEGIN
                        ALTER TABLE Equipos WITH CHECK
                        ADD CONSTRAINT FK_Equipos_TiposEquipo FOREIGN KEY (id_tipo_equipo)
                            REFERENCES TiposEquipo(id_tipo_equipo);
                    END

                    IF NOT EXISTS (
                        SELECT 1 FROM sys.foreign_keys
                        WHERE name = 'FK_Equipos_Marcas'
                          AND parent_object_id = OBJECT_ID('Equipos')
                    )
                    BEGIN
                        ALTER TABLE Equipos WITH CHECK
                        ADD CONSTRAINT FK_Equipos_Marcas FOREIGN KEY (id_marca)
                            REFERENCES Marcas(id_marca);
                    END
                END

                SELECT 0;
            ";

            _db.ExecuteTransaction(query);
        }

        public Equipo Agregar(Equipo equipo)
        {
            string query = @"
                INSERT INTO Equipos (id_cliente, id_tipo_equipo, id_marca, modelo, numero_serie, imei, color, observaciones, activo)
                VALUES (@IdCliente, @IdTipoEquipo, @IdMarca, @Modelo, @NumeroSerie, @Imei, @Color, @Observaciones, 1);
                SELECT CAST(SCOPE_IDENTITY() AS int);
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdCliente", equipo.IdCliente),
                new SqlParameter("@IdTipoEquipo", equipo.IdTipoEquipo),
                new SqlParameter("@IdMarca", equipo.IdMarca),
                new SqlParameter("@Modelo", (object)equipo.Modelo ?? DBNull.Value),
                new SqlParameter("@NumeroSerie", (object)equipo.NumeroSerie ?? DBNull.Value),
                new SqlParameter("@Imei", (object)equipo.Imei ?? DBNull.Value),
                new SqlParameter("@Color", (object)equipo.Color ?? DBNull.Value),
                new SqlParameter("@Observaciones", (object)equipo.Observaciones ?? DBNull.Value)
            };

            int id = _db.ExecuteTransaction(query, sqlParameters);

            return ObtenerPorId(id);
        }

        public void Modificar(Equipo equipo)
        {
            string query = @"
                UPDATE Equipos
                SET id_cliente=@IdCliente, id_tipo_equipo=@IdTipoEquipo, id_marca=@IdMarca,
                    modelo=@Modelo, numero_serie=@NumeroSerie, imei=@Imei,
                    color=@Color, observaciones=@Observaciones
                WHERE id_equipo=@Id;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Id", equipo.Id),
                new SqlParameter("@IdCliente", equipo.IdCliente),
                new SqlParameter("@IdTipoEquipo", equipo.IdTipoEquipo),
                new SqlParameter("@IdMarca", equipo.IdMarca),
                new SqlParameter("@Modelo", (object)equipo.Modelo ?? DBNull.Value),
                new SqlParameter("@NumeroSerie", (object)equipo.NumeroSerie ?? DBNull.Value),
                new SqlParameter("@Imei", (object)equipo.Imei ?? DBNull.Value),
                new SqlParameter("@Color", (object)equipo.Color ?? DBNull.Value),
                new SqlParameter("@Observaciones", (object)equipo.Observaciones ?? DBNull.Value)
            };

            _db.ExecuteTransaction(query, sqlParameters);
        }

        public void Desactivar(int id)
        {
            string query = @"
                UPDATE Equipos SET activo=0 WHERE id_equipo=@Id;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Id", id)
            };

            _db.ExecuteTransaction(query, sqlParameters);
        }

        public void Reactivar(int id)
        {
            string query = @"
                UPDATE Equipos SET activo=1 WHERE id_equipo=@Id;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Id", id)
            };

            _db.ExecuteTransaction(query, sqlParameters);
        }

        public Equipo ObtenerPorId(int id)
        {
            string query = @"
                SELECT id_equipo, id_cliente, id_tipo_equipo, id_marca, modelo,
                       numero_serie, imei, color, observaciones, activo
                FROM Equipos WHERE id_equipo=@Id;
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

        public List<Equipo> Listar(bool incluirInactivos = false)
        {
            string query = @"
                SELECT id_equipo, id_cliente, id_tipo_equipo, id_marca, modelo,
                       numero_serie, imei, color, observaciones, activo
                FROM Equipos
                WHERE (@IncluirInactivos = 1 OR activo = 1)
                ORDER BY id_equipo;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IncluirInactivos", incluirInactivos ? 1 : 0)
            };

            DataTable dt = _db.ExecuteQuery(query, sqlParameters);
            List<Equipo> equipos = new List<Equipo>();

            foreach (DataRow fila in dt.Rows)
                equipos.Add(Mapear(fila));

            return equipos;
        }

        public List<Equipo> ListarPorCliente(int idCliente, bool incluirInactivos = false)
        {
            string query = @"
                SELECT id_equipo, id_cliente, id_tipo_equipo, id_marca, modelo,
                       numero_serie, imei, color, observaciones, activo
                FROM Equipos
                WHERE id_cliente=@IdCliente
                  AND (@IncluirInactivos = 1 OR activo = 1)
                ORDER BY id_equipo;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdCliente", idCliente),
                new SqlParameter("@IncluirInactivos", incluirInactivos ? 1 : 0)
            };

            DataTable dt = _db.ExecuteQuery(query, sqlParameters);
            List<Equipo> equipos = new List<Equipo>();

            foreach (DataRow fila in dt.Rows)
                equipos.Add(Mapear(fila));

            return equipos;
        }

        private Equipo Mapear(DataRow fila)
        {
            return Equipo.CargarDesdeDB(
                Convert.ToInt32(fila["id_equipo"]),
                Convert.ToInt32(fila["id_cliente"]),
                Convert.ToInt32(fila["id_tipo_equipo"]),
                Convert.ToInt32(fila["id_marca"]),
                fila["modelo"] == DBNull.Value ? "" : fila["modelo"].ToString(),
                fila["numero_serie"] == DBNull.Value ? "" : fila["numero_serie"].ToString(),
                fila["imei"] == DBNull.Value ? "" : fila["imei"].ToString(),
                fila["color"] == DBNull.Value ? "" : fila["color"].ToString(),
                fila["observaciones"] == DBNull.Value ? "" : fila["observaciones"].ToString(),
                Convert.ToBoolean(fila["activo"])
            );
        }
    }
}
