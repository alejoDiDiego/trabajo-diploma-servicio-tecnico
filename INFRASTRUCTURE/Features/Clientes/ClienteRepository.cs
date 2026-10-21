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

            // Lote separado: nunca referenciar columnas nuevas en el mismo lote que su ALTER.
            // UX_Clientes_Documento es la barrera de persistencia del documento (activos e
            // inactivos). Si ya existen duplicados historicos no se instala y el guard
            // transaccional de Agregar/Modificar sigue validando, sin borrar ni fusionar filas.
            string queryIndiceDocumento = @"
                IF OBJECT_ID('Clientes', 'U') IS NOT NULL
                   AND NOT EXISTS (
                        SELECT 1 FROM sys.indexes
                        WHERE name = 'UX_Clientes_Documento'
                          AND object_id = OBJECT_ID('Clientes')
                   )
                   AND NOT EXISTS (
                        SELECT 1 FROM Clientes GROUP BY documento HAVING COUNT(*) > 1
                   )
                BEGIN
                    CREATE UNIQUE INDEX UX_Clientes_Documento ON Clientes(documento);
                END

                SELECT 0;
            ";

            _db.ExecuteTransaction(queryIndiceDocumento);
        }

        public Cliente Agregar(Cliente cliente)
        {
            // Un solo lote: TABLOCKX+HOLDLOCK serializa el alta, revisa el documento en toda la
            // tabla (activos e inactivos) y recien entonces inserta. En conflicto no ejecuta DML
            // y devuelve -id del cliente que ya tiene el documento.
            string query = @"
                SET NOCOUNT ON;

                DECLARE @Conflicto int = 0;

                SELECT @Conflicto = ISNULL(MIN(id_cliente), 0)
                FROM Clientes WITH (TABLOCKX, HOLDLOCK)
                WHERE documento = @Documento;

                IF @Conflicto > 0
                BEGIN
                    SELECT -@Conflicto;
                END
                ELSE
                BEGIN
                    INSERT INTO Clientes (nombre, apellido, documento, telefono, email, direccion, observaciones, activo)
                    VALUES (@Nombre, @Apellido, @Documento, @Telefono, @Email, @Direccion, @Observaciones, 1);
                    SELECT CAST(SCOPE_IDENTITY() AS int);
                END
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

            try
            {
                int resultado = _db.ExecuteTransaction(query, sqlParameters);

                if (resultado < 0)
                    throw CrearConflictoDocumento(-resultado, cliente.Documento);

                return ObtenerPorId(resultado);
            }
            catch (SqlException ex) when (ex.Number == 2601 || ex.Number == 2627)
            {
                // Respaldo de UX_Clientes_Documento: mensaje de negocio, sin texto SQL.
                throw new ClienteDocumentoDuplicadoException(cliente.Documento);
            }
        }

        public void Modificar(Cliente cliente)
        {
            // Mismo lote: si el documento no cambia se permite guardar aunque existan duplicados
            // historicos (comparacion SQL); si cambia, no puede pertenecer a otro id_cliente.
            string query = @"
                SET NOCOUNT ON;

                DECLARE @DocumentoActual nvarchar(50);
                DECLARE @Conflicto int = 0;

                SELECT TOP 1 @DocumentoActual = documento
                FROM Clientes WITH (TABLOCKX, HOLDLOCK)
                WHERE id_cliente = @Id;

                IF @DocumentoActual IS NOT NULL AND @DocumentoActual <> @Documento
                BEGIN
                    SELECT @Conflicto = ISNULL(MIN(id_cliente), 0)
                    FROM Clientes WITH (TABLOCKX, HOLDLOCK)
                    WHERE documento = @Documento AND id_cliente <> @Id;
                END

                IF @Conflicto > 0
                BEGIN
                    SELECT -@Conflicto;
                END
                ELSE
                BEGIN
                    UPDATE Clientes
                    SET nombre=@Nombre, apellido=@Apellido, documento=@Documento,
                        telefono=@Telefono, email=@Email, direccion=@Direccion,
                        observaciones=@Observaciones
                    WHERE id_cliente=@Id;

                    SELECT 0;
                END
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

            try
            {
                int resultado = _db.ExecuteTransaction(query, sqlParameters);

                if (resultado < 0)
                    throw CrearConflictoDocumento(-resultado, cliente.Documento);
            }
            catch (SqlException ex) when (ex.Number == 2601 || ex.Number == 2627)
            {
                // Respaldo de UX_Clientes_Documento: mensaje de negocio, sin texto SQL.
                throw new ClienteDocumentoDuplicadoException(cliente.Documento);
            }
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

        // Busca quien ya ocupa el documento propuesto, en activos e inactivos.
        // idExcluir > 0 excluye la fila editada y, si esa fila conserva su propio documento
        // (igualdad SQL, collation de la base), no hay conflicto: los duplicados historicos
        // siguen siendo visibles y editables sin reescribir su identidad.
        public Cliente ObtenerConflictoDocumento(string documento, int idExcluir = 0)
        {
            string query = @"
                DECLARE @MismoDocumento bit = 0;

                IF @IdExcluir > 0
                   AND EXISTS (
                        SELECT 1 FROM Clientes
                        WHERE id_cliente = @IdExcluir AND documento = @Documento
                   )
                    SET @MismoDocumento = 1;

                SELECT TOP 1 id_cliente, nombre, apellido, documento, telefono, email,
                       direccion, observaciones, activo, fecha_alta
                FROM Clientes
                WHERE @MismoDocumento = 0
                  AND documento = @Documento
                  AND id_cliente <> @IdExcluir
                ORDER BY id_cliente;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Documento", (object)documento ?? DBNull.Value),
                new SqlParameter("@IdExcluir", idExcluir)
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

        private ClienteDocumentoDuplicadoException CrearConflictoDocumento(int idConflicto, string documento)
        {
            Cliente existente = ObtenerPorId(idConflicto);

            if (existente == null)
                return new ClienteDocumentoDuplicadoException(documento);

            return new ClienteDocumentoDuplicadoException(
                existente.Id, existente.Nombre, existente.Apellido, existente.Documento, existente.Activo);
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
