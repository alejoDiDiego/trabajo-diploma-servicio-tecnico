using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DOMAIN.Features.Proveedores;

namespace REPOSITORY.Features.Proveedores
{
    public class ProveedorRepository
    {
        private readonly SqlHelper _db;

        public ProveedorRepository()
            : this(ConfigurationManager.ConnectionStrings["UrlDB"].ConnectionString)
        {
        }

        public ProveedorRepository(string cadenaConexion)
        {
            _db = new SqlHelper(cadenaConexion);
        }

        public void Inicializar()
        {
            // Crea la tabla Proveedores de forma idempotente (IF OBJECT_ID + ALTER defensivos).
            // UNIQUE en razon_social (como Marcas): la razon social identifica al proveedor en la UI.
            string query = @"
                IF OBJECT_ID('Proveedores', 'U') IS NULL
                BEGIN
                    CREATE TABLE Proveedores (
                        id_proveedor int IDENTITY(1,1) NOT NULL PRIMARY KEY,
                        razon_social nvarchar(200) NOT NULL,
                        cuit nvarchar(20) NULL,
                        telefono nvarchar(100) NULL,
                        email nvarchar(200) NULL,
                        direccion nvarchar(300) NULL,
                        contacto nvarchar(200) NULL,
                        activo bit NOT NULL CONSTRAINT DF_Proveedores_Activo DEFAULT 1
                    );
                END
                ELSE
                BEGIN
                    IF COL_LENGTH('Proveedores', 'razon_social') IS NULL
                        ALTER TABLE Proveedores ADD razon_social nvarchar(200) NOT NULL CONSTRAINT DF_Proveedores_RazonSocial DEFAULT '';

                    IF COL_LENGTH('Proveedores', 'cuit') IS NULL
                        ALTER TABLE Proveedores ADD cuit nvarchar(20) NULL;

                    IF COL_LENGTH('Proveedores', 'telefono') IS NULL
                        ALTER TABLE Proveedores ADD telefono nvarchar(100) NULL;

                    IF COL_LENGTH('Proveedores', 'email') IS NULL
                        ALTER TABLE Proveedores ADD email nvarchar(200) NULL;

                    IF COL_LENGTH('Proveedores', 'direccion') IS NULL
                        ALTER TABLE Proveedores ADD direccion nvarchar(300) NULL;

                    IF COL_LENGTH('Proveedores', 'contacto') IS NULL
                        ALTER TABLE Proveedores ADD contacto nvarchar(200) NULL;

                    IF COL_LENGTH('Proveedores', 'activo') IS NULL
                        ALTER TABLE Proveedores ADD activo bit NOT NULL CONSTRAINT DF_Proveedores_Activo DEFAULT 1;
                END

                IF NOT EXISTS (
                    SELECT 1 FROM sys.indexes
                    WHERE name = 'UX_Proveedores_RazonSocial'
                      AND object_id = OBJECT_ID('Proveedores')
                )
                BEGIN
                    CREATE UNIQUE INDEX UX_Proveedores_RazonSocial
                    ON Proveedores(razon_social);
                END

                SELECT 0;
            ";

            _db.ExecuteTransaction(query);
        }

        public Proveedor Agregar(Proveedor proveedor)
        {
            string query = @"
                INSERT INTO Proveedores (razon_social, cuit, telefono, email, direccion, contacto, activo)
                VALUES (@RazonSocial, @Cuit, @Telefono, @Email, @Direccion, @Contacto, 1);
                SELECT CAST(SCOPE_IDENTITY() AS int);
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@RazonSocial", proveedor.RazonSocial),
                new SqlParameter("@Cuit", (object)proveedor.Cuit ?? DBNull.Value),
                new SqlParameter("@Telefono", (object)proveedor.Telefono ?? DBNull.Value),
                new SqlParameter("@Email", (object)proveedor.Email ?? DBNull.Value),
                new SqlParameter("@Direccion", (object)proveedor.Direccion ?? DBNull.Value),
                new SqlParameter("@Contacto", (object)proveedor.Contacto ?? DBNull.Value)
            };

            int id = _db.ExecuteTransaction(query, sqlParameters);

            return ObtenerPorId(id);
        }

        public void Modificar(Proveedor proveedor)
        {
            string query = @"
                UPDATE Proveedores
                SET razon_social=@RazonSocial, cuit=@Cuit, telefono=@Telefono,
                    email=@Email, direccion=@Direccion, contacto=@Contacto
                WHERE id_proveedor=@Id;
                SELECT 0;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Id", proveedor.Id),
                new SqlParameter("@RazonSocial", proveedor.RazonSocial),
                new SqlParameter("@Cuit", (object)proveedor.Cuit ?? DBNull.Value),
                new SqlParameter("@Telefono", (object)proveedor.Telefono ?? DBNull.Value),
                new SqlParameter("@Email", (object)proveedor.Email ?? DBNull.Value),
                new SqlParameter("@Direccion", (object)proveedor.Direccion ?? DBNull.Value),
                new SqlParameter("@Contacto", (object)proveedor.Contacto ?? DBNull.Value)
            };

            _db.ExecuteTransaction(query, sqlParameters);
        }

        public void Desactivar(int id)
        {
            string query = @"
                UPDATE Proveedores SET activo=0 WHERE id_proveedor=@Id;
                SELECT 0;
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
                UPDATE Proveedores SET activo=1 WHERE id_proveedor=@Id;
                SELECT 0;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Id", id)
            };

            _db.ExecuteTransaction(query, sqlParameters);
        }

        public Proveedor ObtenerPorId(int id)
        {
            string query = @"
                SELECT id_proveedor, razon_social, cuit, telefono, email, direccion, contacto, activo
                FROM Proveedores WHERE id_proveedor=@Id;
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

        public List<Proveedor> Listar(bool incluirInactivos = false)
        {
            string query = @"
                SELECT id_proveedor, razon_social, cuit, telefono, email, direccion, contacto, activo
                FROM Proveedores
                WHERE (@IncluirInactivos = 1 OR activo = 1)
                ORDER BY razon_social;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IncluirInactivos", incluirInactivos ? 1 : 0)
            };

            DataTable dt = _db.ExecuteQuery(query, sqlParameters);
            List<Proveedor> proveedores = new List<Proveedor>();

            foreach (DataRow fila in dt.Rows)
                proveedores.Add(Mapear(fila));

            return proveedores;
        }

        private Proveedor Mapear(DataRow fila)
        {
            return Proveedor.CargarDesdeDB(
                Convert.ToInt32(fila["id_proveedor"]),
                fila["razon_social"] == DBNull.Value ? "" : fila["razon_social"].ToString(),
                fila["cuit"] == DBNull.Value ? "" : fila["cuit"].ToString(),
                fila["telefono"] == DBNull.Value ? "" : fila["telefono"].ToString(),
                fila["email"] == DBNull.Value ? "" : fila["email"].ToString(),
                fila["direccion"] == DBNull.Value ? "" : fila["direccion"].ToString(),
                fila["contacto"] == DBNull.Value ? "" : fila["contacto"].ToString(),
                Convert.ToBoolean(fila["activo"])
            );
        }
    }
}
