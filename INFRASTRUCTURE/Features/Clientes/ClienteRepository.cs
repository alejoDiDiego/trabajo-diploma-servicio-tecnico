using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DOMAIN.Features.Clientes;

namespace REPOSITORY.Features.Clientes
{
    public class ClienteRepository
    {
        private readonly SqlHelper _db;

        public ClienteRepository()
            : this(ConfigurationManager.ConnectionStrings["UrlDB"].ConnectionString)
        {
        }

        public ClienteRepository(string cadenaConexion)
        {
            _db = new SqlHelper(cadenaConexion);
        }

        public void Inicializar()
        {
            // Crea la tabla Clientes de forma idempotente (IF OBJECT_ID + ALTER defensivos).
            string query = @"
                IF OBJECT_ID('Clientes', 'U') IS NULL
                BEGIN
                    CREATE TABLE Clientes (
                        id_cliente int IDENTITY(1,1) NOT NULL PRIMARY KEY,
                        nombre nvarchar(100) NOT NULL,
                        apellido nvarchar(100) NOT NULL,
                        documento nvarchar(50) NOT NULL,
                        telefono nvarchar(100) NULL,
                        email nvarchar(200) NULL,
                        direccion nvarchar(300) NULL,
                        observaciones nvarchar(max) NULL,
                        activo bit NOT NULL CONSTRAINT DF_Clientes_Activo DEFAULT 1,
                        fecha_alta datetime NOT NULL CONSTRAINT DF_Clientes_FechaAlta DEFAULT GETDATE()
                    );
                END
                ELSE
                BEGIN
                    IF COL_LENGTH('Clientes', 'nombre') IS NULL
                        ALTER TABLE Clientes ADD nombre nvarchar(100) NOT NULL CONSTRAINT DF_Clientes_Nombre DEFAULT '';

                    IF COL_LENGTH('Clientes', 'apellido') IS NULL
                        ALTER TABLE Clientes ADD apellido nvarchar(100) NOT NULL CONSTRAINT DF_Clientes_Apellido DEFAULT '';

                    IF COL_LENGTH('Clientes', 'documento') IS NULL
                        ALTER TABLE Clientes ADD documento nvarchar(50) NOT NULL CONSTRAINT DF_Clientes_Documento DEFAULT '';

                    IF COL_LENGTH('Clientes', 'telefono') IS NULL
                        ALTER TABLE Clientes ADD telefono nvarchar(100) NULL;

                    IF COL_LENGTH('Clientes', 'email') IS NULL
                        ALTER TABLE Clientes ADD email nvarchar(200) NULL;

                    IF COL_LENGTH('Clientes', 'direccion') IS NULL
                        ALTER TABLE Clientes ADD direccion nvarchar(300) NULL;

                    IF COL_LENGTH('Clientes', 'observaciones') IS NULL
                        ALTER TABLE Clientes ADD observaciones nvarchar(max) NULL;

                    IF COL_LENGTH('Clientes', 'activo') IS NULL
                        ALTER TABLE Clientes ADD activo bit NOT NULL CONSTRAINT DF_Clientes_Activo DEFAULT 1;

                    IF COL_LENGTH('Clientes', 'fecha_alta') IS NULL
                        ALTER TABLE Clientes ADD fecha_alta datetime NOT NULL CONSTRAINT DF_Clientes_FechaAlta DEFAULT GETDATE();
                END

                SELECT 0;
            ";

            _db.ExecuteTransaction(query);
        }

        public Cliente Agregar(Cliente cliente)
        {
            string query = @"
                INSERT INTO Clientes (nombre, apellido, documento, telefono, email, direccion, observaciones, activo)
                VALUES (@Nombre, @Apellido, @Documento, @Telefono, @Email, @Direccion, @Observaciones, 1);
                SELECT CAST(SCOPE_IDENTITY() AS int);
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Nombre", cliente.Nombre),
                new SqlParameter("@Apellido", cliente.Apellido),
                new SqlParameter("@Documento", cliente.Documento),
                new SqlParameter("@Telefono", (object)cliente.Telefono ?? DBNull.Value),
                new SqlParameter("@Email", (object)cliente.Email ?? DBNull.Value),
                new SqlParameter("@Direccion", (object)cliente.Direccion ?? DBNull.Value),
                new SqlParameter("@Observaciones", (object)cliente.Observaciones ?? DBNull.Value)
            };

            int id = _db.ExecuteTransaction(query, sqlParameters);

            return ObtenerPorId(id);
        }

        public void Modificar(Cliente cliente)
        {
            string query = @"
                UPDATE Clientes
                SET nombre=@Nombre, apellido=@Apellido, documento=@Documento,
                    telefono=@Telefono, email=@Email, direccion=@Direccion,
                    observaciones=@Observaciones
                WHERE id_cliente=@Id;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Id", cliente.Id),
                new SqlParameter("@Nombre", cliente.Nombre),
                new SqlParameter("@Apellido", cliente.Apellido),
                new SqlParameter("@Documento", cliente.Documento),
                new SqlParameter("@Telefono", (object)cliente.Telefono ?? DBNull.Value),
                new SqlParameter("@Email", (object)cliente.Email ?? DBNull.Value),
                new SqlParameter("@Direccion", (object)cliente.Direccion ?? DBNull.Value),
                new SqlParameter("@Observaciones", (object)cliente.Observaciones ?? DBNull.Value)
            };

            _db.ExecuteTransaction(query, sqlParameters);
        }

        public void Desactivar(int id)
        {
            string query = @"
                UPDATE Clientes SET activo=0 WHERE id_cliente=@Id;
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
                UPDATE Clientes SET activo=1 WHERE id_cliente=@Id;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Id", id)
            };

            _db.ExecuteTransaction(query, sqlParameters);
        }

        public Cliente ObtenerPorId(int id)
        {
            string query = @"
                SELECT id_cliente, nombre, apellido, documento, telefono, email,
                       direccion, observaciones, activo, fecha_alta
                FROM Clientes WHERE id_cliente=@Id;
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

        public List<Cliente> Listar(bool incluirInactivos = false)
        {
            string query = @"
                SELECT id_cliente, nombre, apellido, documento, telefono, email,
                       direccion, observaciones, activo, fecha_alta
                FROM Clientes
                WHERE (@IncluirInactivos = 1 OR activo = 1)
                ORDER BY apellido, nombre;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IncluirInactivos", incluirInactivos ? 1 : 0)
            };

            DataTable dt = _db.ExecuteQuery(query, sqlParameters);
            List<Cliente> clientes = new List<Cliente>();

            foreach (DataRow fila in dt.Rows)
                clientes.Add(Mapear(fila));

            return clientes;
        }

        private Cliente Mapear(DataRow fila)
        {
            return Cliente.CargarDesdeDB(
                Convert.ToInt32(fila["id_cliente"]),
                fila["nombre"] == DBNull.Value ? "" : fila["nombre"].ToString(),
                fila["apellido"] == DBNull.Value ? "" : fila["apellido"].ToString(),
                fila["documento"] == DBNull.Value ? "" : fila["documento"].ToString(),
                fila["telefono"] == DBNull.Value ? "" : fila["telefono"].ToString(),
                fila["email"] == DBNull.Value ? "" : fila["email"].ToString(),
                fila["direccion"] == DBNull.Value ? "" : fila["direccion"].ToString(),
                fila["observaciones"] == DBNull.Value ? "" : fila["observaciones"].ToString(),
                Convert.ToBoolean(fila["activo"]),
                Convert.ToDateTime(fila["fecha_alta"])
            );
        }
    }
}
