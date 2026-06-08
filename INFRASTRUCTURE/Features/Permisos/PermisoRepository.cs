using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DOMAIN.Exceptions;
using DOMAIN.Features.Permisos;

namespace REPOSITORY.Features.Permisos
{
    public class PermisoRepository
    {
        private readonly SqlHelper _db;

        public PermisoRepository()
            : this(ConfigurationManager.ConnectionStrings["UrlDB"].ConnectionString)
        {
        }

        public PermisoRepository(string cadenaConexion)
        {
            _db = new SqlHelper(cadenaConexion);
        }

        public void Inicializar()
        {
            // Permisos es el catalogo; PermisoComposicion guarda los vinculos del arbol.
            CrearTablaPermisos();
            CrearTablaComposicion();
            AgregarPermisosSimplesBase();
            AgregarTraduccionesPermisos();
        }

        public List<PermisoComponent> ListarArbol()
        {
            Dictionary<int, DataRow> permisos = ObtenerFilasPermisos();
            DataTable relaciones = ObtenerRelaciones();

            // idPadre null representa la raiz virtual del TreeView.
            return CrearHijos(null, permisos, relaciones, new List<int>());
        }

        public List<PermisoComponent> ListarSubArbol(int idRaiz)
        {
            Dictionary<int, DataRow> permisos = ObtenerFilasPermisos();
            DataTable relaciones = ObtenerRelaciones();

            if (!permisos.ContainsKey(idRaiz))
                return new List<PermisoComponent>();

            // Arma un arbol parcial desde una familia/permiso concreto.
            // Se usa para validar ciclos antes de insertar una nueva relacion.
            PermisoComponent raiz = CrearComponente(permisos[idRaiz]);

            if (raiz.EsFamilia)
            {
                List<int> idsEnRama = new List<int>();
                idsEnRama.Add(raiz.Id);

                // La raiz del subarbol ya queda marcada para no volver a entrar en ella.
                foreach (PermisoComponent hijo in CrearHijos(raiz.Id, permisos, relaciones, idsEnRama))
                    raiz.AgregarHijo(hijo);
            }

            return new List<PermisoComponent> { raiz };
        }

        public List<FamiliaPermiso> ListarFamilias()
        {
            string query = @"
                SELECT id_permiso, nombre, es_familia
                FROM Permisos
                WHERE es_familia = 1
                ORDER BY nombre;
            ";

            DataTable dt = _db.ExecuteQuery(query);
            List<FamiliaPermiso> familias = new List<FamiliaPermiso>();

            foreach (DataRow fila in dt.Rows)
                familias.Add((FamiliaPermiso)CrearComponente(fila));

            return familias;
        }

        public List<PermisoSimple> ListarPermisosSimples()
        {
            string query = @"
                SELECT id_permiso, nombre, es_familia
                FROM Permisos
                WHERE es_familia = 0
                ORDER BY nombre;
            ";

            DataTable dt = _db.ExecuteQuery(query);
            List<PermisoSimple> permisos = new List<PermisoSimple>();

            foreach (DataRow fila in dt.Rows)
                permisos.Add((PermisoSimple)CrearComponente(fila));

            return permisos;
        }

        public FamiliaPermiso AgregarFamilia(string nombre)
        {
            if (ExistePermisoConNombre(nombre, null))
                throw new ReglaNegocioException("Ya existe un permiso o familia con ese nombre.");

            string query = @"
                INSERT INTO Permisos (nombre, es_familia)
                VALUES (@Nombre, 1);
                SELECT CAST(SCOPE_IDENTITY() AS int);
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Nombre", nombre)
            };

            try
            {
                int id = _db.ExecuteTransaction(query, sqlParameters);
                return FamiliaPermiso.CargarDesdeDB(id, nombre);
            }
            catch (SqlException ex) when (ex.Number == 2601 || ex.Number == 2627)
            {
                throw new ReglaNegocioException("Ya existe un permiso o familia con ese nombre.");
            }
        }

        public void ModificarFamilia(int id, string nombre)
        {
            if (ExistePermisoConNombre(nombre, id))
                throw new ReglaNegocioException("Ya existe un permiso o familia con ese nombre.");

            string query = @"
                UPDATE Permisos
                SET nombre=@Nombre
                WHERE id_permiso=@Id AND es_familia=1;
                SELECT @@ROWCOUNT;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Id", id),
                new SqlParameter("@Nombre", nombre)
            };

            int filas = _db.ExecuteTransaction(query, sqlParameters);

            if (filas <= 0)
                throw new ReglaNegocioException("La familia seleccionada no existe.");
        }

        public void EliminarFamilia(int id)
        {
            string query = @"
                -- Borra apariciones e hijos de la familia, pero no elimina los componentes hijos del catalogo.
                DELETE FROM PermisoComposicion
                WHERE id_permiso_padre=@Id OR id_permiso_hijo=@Id;

                DELETE FROM Permisos
                WHERE id_permiso=@Id AND es_familia=1;

                SELECT @@ROWCOUNT;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Id", id)
            };

            int filas = _db.ExecuteTransaction(query, sqlParameters);

            if (filas <= 0)
                throw new ReglaNegocioException("La familia seleccionada no existe.");
        }

        public void AgregarComponente(int? idPadre, int idHijo)
        {
            string query = @"
                -- id_permiso_padre NULL significa raiz virtual; con valor significa familia padre.
                INSERT INTO PermisoComposicion (id_permiso_padre, id_permiso_hijo)
                VALUES (@IdPadre, @IdHijo);
                SELECT CAST(SCOPE_IDENTITY() AS int);
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdPadre", idPadre.HasValue ? (object)idPadre.Value : DBNull.Value),
                new SqlParameter("@IdHijo", idHijo)
            };

            try
            {
                _db.ExecuteTransaction(query, sqlParameters);
            }
            catch (SqlException ex) when (ex.Number == 2601 || ex.Number == 2627)
            {
                throw new ReglaNegocioException("El permiso ya forma parte del nivel seleccionado.");
            }
        }

        public void QuitarComponente(int? idPadre, int idHijo)
        {
            string query = idPadre.HasValue
                ? @"
                    DELETE FROM PermisoComposicion
                    WHERE id_permiso_padre=@IdPadre AND id_permiso_hijo=@IdHijo;
                    SELECT @@ROWCOUNT;
                "
                : @"
                    DELETE FROM PermisoComposicion
                    WHERE id_permiso_padre IS NULL AND id_permiso_hijo=@IdHijo;
                    SELECT @@ROWCOUNT;
                ";

            SqlParameter[] sqlParameters = idPadre.HasValue
                ? new SqlParameter[]
                {
                    new SqlParameter("@IdPadre", idPadre.Value),
                    new SqlParameter("@IdHijo", idHijo)
                }
                : new SqlParameter[]
                {
                    new SqlParameter("@IdHijo", idHijo)
                };

            int filas = _db.ExecuteTransaction(query, sqlParameters);

            if (filas <= 0)
                throw new ReglaNegocioException("El componente no forma parte del nivel seleccionado.");
        }

        public bool ExisteRelacion(int? idPadre, int idHijo)
        {
            string query = idPadre.HasValue
                ? @"
                    SELECT COUNT(1)
                    FROM PermisoComposicion
                    WHERE id_permiso_padre=@IdPadre AND id_permiso_hijo=@IdHijo;
                "
                : @"
                    SELECT COUNT(1)
                    FROM PermisoComposicion
                    WHERE id_permiso_padre IS NULL AND id_permiso_hijo=@IdHijo;
                ";

            SqlParameter[] sqlParameters = idPadre.HasValue
                ? new SqlParameter[]
                {
                    new SqlParameter("@IdPadre", idPadre.Value),
                    new SqlParameter("@IdHijo", idHijo)
                }
                : new SqlParameter[]
                {
                    new SqlParameter("@IdHijo", idHijo)
                };

            return _db.ExecuteTransaction(query, sqlParameters) > 0;
        }

        public PermisoComponent ObtenerPorId(int id)
        {
            string query = @"
                SELECT id_permiso, nombre, es_familia
                FROM Permisos
                WHERE id_permiso=@Id;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Id", id)
            };

            DataTable dt = _db.ExecuteQuery(query, sqlParameters);

            if (dt.Rows.Count <= 0)
                return null;

            return CrearComponente(dt.Rows[0]);
        }

        private void CrearTablaPermisos()
        {
            string query = @"
                IF OBJECT_ID('Permisos', 'U') IS NULL
                BEGIN
                    -- Catalogo de componentes: familias y permisos simples viven en la misma tabla.
                    CREATE TABLE Permisos (
                        id_permiso int IDENTITY(1,1) NOT NULL PRIMARY KEY,
                        nombre nvarchar(100) NOT NULL,
                        es_familia bit NOT NULL
                    );
                END
                ELSE
                BEGIN
                    IF COL_LENGTH('Permisos', 'nombre') IS NULL
                        ALTER TABLE Permisos ADD nombre nvarchar(100) NULL;

                    IF COL_LENGTH('Permisos', 'es_familia') IS NULL
                        ALTER TABLE Permisos ADD es_familia bit NOT NULL CONSTRAINT DF_Permisos_EsFamilia DEFAULT 0;
                END

                SELECT 0;
            ";

            _db.ExecuteTransaction(query);
        }

        private void CrearTablaComposicion()
        {
            string query = @"
                IF OBJECT_ID('PermisoComposicion', 'U') IS NULL
                BEGIN
                    -- Relaciones del Composite. Permite que un mismo hijo aparezca en ramas distintas.
                    CREATE TABLE PermisoComposicion (
                        id_permiso_composicion int IDENTITY(1,1) NOT NULL PRIMARY KEY,
                        id_permiso_padre int NULL,
                        id_permiso_hijo int NOT NULL,
                        CONSTRAINT FK_PermisoComposicion_Padre FOREIGN KEY (id_permiso_padre)
                            REFERENCES Permisos(id_permiso),
                        CONSTRAINT FK_PermisoComposicion_Hijo FOREIGN KEY (id_permiso_hijo)
                            REFERENCES Permisos(id_permiso)
                    );
                END

                IF NOT EXISTS (
                    SELECT 1 FROM sys.indexes
                    WHERE name = 'UX_PermisoComposicion_Raiz_Hijo'
                      AND object_id = OBJECT_ID('PermisoComposicion')
                )
                BEGIN
                    -- Evita repetir el mismo componente directamente debajo de la raiz.
                    CREATE UNIQUE INDEX UX_PermisoComposicion_Raiz_Hijo
                    ON PermisoComposicion(id_permiso_hijo)
                    WHERE id_permiso_padre IS NULL;
                END

                IF NOT EXISTS (
                    SELECT 1 FROM sys.indexes
                    WHERE name = 'UX_PermisoComposicion_Padre_Hijo'
                      AND object_id = OBJECT_ID('PermisoComposicion')
                )
                BEGIN
                    -- Evita repetir el mismo hijo bajo el mismo padre, pero no en otros padres.
                    CREATE UNIQUE INDEX UX_PermisoComposicion_Padre_Hijo
                    ON PermisoComposicion(id_permiso_padre, id_permiso_hijo)
                    WHERE id_permiso_padre IS NOT NULL;
                END

                SELECT 0;
            ";

            _db.ExecuteTransaction(query);
        }

        private void AgregarPermisosSimplesBase()
        {
            string query = @"
                -- Permisos simples mantenidos por desarrolladores. La UI no los crea ni edita.
                CREATE TABLE #Seed (
                    nombre nvarchar(100),
                    codigo_legacy nvarchar(100)
                );

                INSERT INTO #Seed (nombre, codigo_legacy) VALUES
                ('Administrar usuarios', 'USUARIOS_ADMINISTRAR'),
                ('Crear usuarios', 'USUARIOS_CREAR'),
                ('Editar usuarios', 'USUARIOS_EDITAR'),
                ('Eliminar usuarios', 'USUARIOS_ELIMINAR'),
                ('Administrar traducciones', 'TRADUCCIONES_ADMINISTRAR'),
                ('Cambiar idioma', 'IDIOMAS_CAMBIAR'),
                ('Administrar permisos', 'PERMISOS_ADMINISTRAR'),
                ('Crear familias', 'PERMISOS_CREAR'),
                ('Editar familias', 'PERMISOS_EDITAR'),
                ('Eliminar familias', 'PERMISOS_ELIMINAR'),
                ('Componer familias', 'PERMISOS_MOVER');

                IF COL_LENGTH('Permisos', 'codigo') IS NOT NULL
                BEGIN
                    -- codigo_legacy solo sirve para reconocer datos existentes antes de borrar codigo.
                    DECLARE @sql nvarchar(max);

                    SET @sql = N'
                    UPDATE p
                    SET p.nombre = s.nombre,
                        p.es_familia = 0
                    FROM Permisos p
                    INNER JOIN #Seed s ON s.codigo_legacy = p.codigo;

                    INSERT INTO Permisos (nombre, es_familia)
                    SELECT s.nombre, 0
                    FROM #Seed s
                    WHERE NOT EXISTS (SELECT 1 FROM Permisos p WHERE p.nombre = s.nombre)
                      AND NOT EXISTS (SELECT 1 FROM Permisos p WHERE p.codigo = s.codigo_legacy);';

                    EXEC sp_executesql @sql;
                END
                ELSE
                BEGIN
                    INSERT INTO Permisos (nombre, es_familia)
                    SELECT s.nombre, 0
                    FROM #Seed s
                    WHERE NOT EXISTS (SELECT 1 FROM Permisos p WHERE p.nombre = s.nombre);
                END

                SELECT 0;
            ";

            _db.ExecuteTransaction(query);
        }

        private void AgregarTraduccionesPermisos()
        {
            string query = @"
                IF OBJECT_ID('Idiomas', 'U') IS NOT NULL
                   AND OBJECT_ID('Palabras', 'U') IS NOT NULL
                   AND OBJECT_ID('Traducciones', 'U') IS NOT NULL
                BEGIN
                    DECLARE @Seed TABLE (
                        idioma nvarchar(100),
                        clave nvarchar(200),
                        texto nvarchar(max)
                    );

                    INSERT INTO @Seed (idioma, clave, texto) VALUES
                    ('Espanol', 'Menu.AdministrarPermisos', 'Administrar permisos'),
                    ('Ingles', 'Menu.AdministrarPermisos', 'Manage permissions'),
                    ('Espanol', 'FrmAdministrarPermisos.Text', 'Administracion de permisos'),
                    ('Ingles', 'FrmAdministrarPermisos.Text', 'Permission administration'),
                    ('Espanol', 'Permisos.Titulo', 'Administracion de permisos'),
                    ('Ingles', 'Permisos.Titulo', 'Permission administration'),
                    ('Espanol', 'Permisos.Arbol', 'Arbol de permisos'),
                    ('Ingles', 'Permisos.Arbol', 'Permission tree'),
                    ('Espanol', 'Permisos.Familia', 'Familia'),
                    ('Ingles', 'Permisos.Familia', 'Family'),
                    ('Espanol', 'Permisos.Catalogo', 'Catalogo'),
                    ('Ingles', 'Permisos.Catalogo', 'Catalog'),
                    ('Espanol', 'Permisos.Composicion', 'Composicion'),
                    ('Ingles', 'Permisos.Composicion', 'Composition'),
                    ('Espanol', 'Permisos.NombreFamilia', 'Nombre familia'),
                    ('Ingles', 'Permisos.NombreFamilia', 'Family name'),
                    ('Espanol', 'Permisos.Familias', 'Familias'),
                    ('Ingles', 'Permisos.Familias', 'Families'),
                    ('Espanol', 'Permisos.PermisosSimples', 'Permisos simples'),
                    ('Ingles', 'Permisos.PermisosSimples', 'Simple permissions'),
                    ('Espanol', 'Permisos.Destino', 'Destino'),
                    ('Ingles', 'Permisos.Destino', 'Target'),
                    ('Espanol', 'Permisos.Raiz', 'Raiz'),
                    ('Ingles', 'Permisos.Raiz', 'Root'),
                    ('Espanol', 'Permisos.SeleccioneDestino', 'Seleccione una familia o la raiz'),
                    ('Ingles', 'Permisos.SeleccioneDestino', 'Select a family or root'),
                    ('Espanol', 'Permisos.SeleccionPermisoSimple', 'Los permisos simples no pueden contener hijos'),
                    ('Ingles', 'Permisos.SeleccionPermisoSimple', 'Simple permissions cannot contain children'),
                    ('Espanol', 'Permisos.CrearFamilia', 'Crear familia'),
                    ('Ingles', 'Permisos.CrearFamilia', 'Create family'),
                    ('Espanol', 'Permisos.EditarFamilia', 'Editar familia'),
                    ('Ingles', 'Permisos.EditarFamilia', 'Edit family'),
                    ('Espanol', 'Permisos.EliminarFamilia', 'Eliminar familia'),
                    ('Ingles', 'Permisos.EliminarFamilia', 'Delete family'),
                    ('Espanol', 'Permisos.AgregarFamilia', 'Agregar familia'),
                    ('Ingles', 'Permisos.AgregarFamilia', 'Add family'),
                    ('Espanol', 'Permisos.AgregarPermiso', 'Agregar permiso'),
                    ('Ingles', 'Permisos.AgregarPermiso', 'Add permission'),
                    ('Espanol', 'Permisos.QuitarSeleccionado', 'Quitar seleccionado'),
                    ('Ingles', 'Permisos.QuitarSeleccionado', 'Remove selected'),
                    ('Espanol', 'Permisos.Limpiar', 'Limpiar'),
                    ('Ingles', 'Permisos.Limpiar', 'Clear'),
                    ('Espanol', 'Mensaje.FamiliaCreada', 'Familia creada exitosamente.'),
                    ('Ingles', 'Mensaje.FamiliaCreada', 'Family created successfully.'),
                    ('Espanol', 'Mensaje.FamiliaEditada', 'Familia editada exitosamente.'),
                    ('Ingles', 'Mensaje.FamiliaEditada', 'Family edited successfully.'),
                    ('Espanol', 'Mensaje.FamiliaEliminada', 'Familia eliminada exitosamente.'),
                    ('Ingles', 'Mensaje.FamiliaEliminada', 'Family deleted successfully.'),
                    ('Espanol', 'Mensaje.ComponenteAgregado', 'Componente agregado exitosamente.'),
                    ('Ingles', 'Mensaje.ComponenteAgregado', 'Component added successfully.'),
                    ('Espanol', 'Mensaje.ComponenteQuitado', 'Componente quitado exitosamente.'),
                    ('Ingles', 'Mensaje.ComponenteQuitado', 'Component removed successfully.'),
                    ('Espanol', 'Mensaje.SeleccioneFamilia', 'Seleccione una familia.'),
                    ('Ingles', 'Mensaje.SeleccioneFamilia', 'Select a family.'),
                    ('Espanol', 'Mensaje.SeleccioneComponente', 'Seleccione un componente.'),
                    ('Ingles', 'Mensaje.SeleccioneComponente', 'Select a component.'),
                    ('Espanol', 'Mensaje.SeleccioneDestino', 'Seleccione una familia destino o la raiz.'),
                    ('Ingles', 'Mensaje.SeleccioneDestino', 'Select a target family or root.'),
                    ('Espanol', 'Mensaje.ConfirmarEliminarFamilia', 'Estas seguro de eliminar completamente la familia ''{0}''? Se quitaran todas sus apariciones.'),
                    ('Ingles', 'Mensaje.ConfirmarEliminarFamilia', 'Are you sure you want to delete family ''{0}'' completely? All its appearances will be removed.'),
                    ('Espanol', 'Mensaje.ConfirmarQuitarComponente', 'Estas seguro de quitar ''{0}'' del nivel seleccionado?'),
                    ('Ingles', 'Mensaje.ConfirmarQuitarComponente', 'Are you sure you want to remove ''{0}'' from the selected level?'),
                    ('Espanol', 'Mensaje.ErrorPermiso', 'Error al gestionar permisos: {0}'),
                    ('Ingles', 'Mensaje.ErrorPermiso', 'Permission management error: {0}');

                    INSERT INTO Idiomas (nombre)
                    SELECT DISTINCT s.idioma
                    FROM @Seed s
                    WHERE NOT EXISTS (SELECT 1 FROM Idiomas i WHERE i.nombre = s.idioma);

                    INSERT INTO Palabras (texto)
                    SELECT DISTINCT s.clave
                    FROM @Seed s
                    WHERE NOT EXISTS (SELECT 1 FROM Palabras p WHERE p.texto = s.clave);

                    INSERT INTO Traducciones (id_idioma, id_palabra, palabra_traducida)
                    SELECT i.id_idioma, p.id_palabra, s.texto
                    FROM @Seed s
                    INNER JOIN Idiomas i ON i.nombre = s.idioma
                    INNER JOIN Palabras p ON p.texto = s.clave
                    WHERE NOT EXISTS (
                        SELECT 1
                        FROM Traducciones t
                        WHERE t.id_idioma = i.id_idioma
                          AND t.id_palabra = p.id_palabra
                    );
                END
            ";

            _db.ExecuteTransaction(query);
        }

        private bool ExistePermisoConNombre(string nombre, int? idExcluir)
        {
            string query = @"
                SELECT COUNT(1)
                FROM Permisos
                WHERE UPPER(nombre)=UPPER(@Nombre)
                  AND (@IdExcluir IS NULL OR id_permiso<>@IdExcluir);
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Nombre", nombre),
                new SqlParameter("@IdExcluir", idExcluir.HasValue ? (object)idExcluir.Value : DBNull.Value)
            };

            return _db.ExecuteTransaction(query, sqlParameters) > 0;
        }

        private Dictionary<int, DataRow> ObtenerFilasPermisos()
        {
            string query = @"
                SELECT id_permiso, nombre, es_familia
                FROM Permisos
                ORDER BY nombre;
            ";

            DataTable dt = _db.ExecuteQuery(query);
            Dictionary<int, DataRow> permisos = new Dictionary<int, DataRow>();

            foreach (DataRow fila in dt.Rows)
                permisos[Convert.ToInt32(fila["id_permiso"])] = fila;

            return permisos;
        }

        private DataTable ObtenerRelaciones()
        {
            string query = @"
                SELECT pc.id_permiso_padre, pc.id_permiso_hijo
                FROM PermisoComposicion pc
                INNER JOIN Permisos p ON p.id_permiso = pc.id_permiso_hijo
                ORDER BY pc.id_permiso_padre, p.nombre;
            ";

            return _db.ExecuteQuery(query);
        }

        private List<PermisoComponent> CrearHijos(int? idPadre, Dictionary<int, DataRow> permisos, DataTable relaciones, List<int> idsEnRama)
        {
            List<PermisoComponent> hijos = new List<PermisoComponent>();

            foreach (DataRow relacion in relaciones.Rows)
            {
                // En cada llamada solo interesan las relaciones del padre actual.
                if (!PerteneceAlPadre(relacion, idPadre))
                    continue;

                int idHijo = Convert.ToInt32(relacion["id_permiso_hijo"]);

                // Si la relacion apunta a un permiso inexistente o repetido en esta rama, se ignora.
                // La repeticion en la misma rama indicaria un ciclo.
                if (!permisos.ContainsKey(idHijo) || idsEnRama.Contains(idHijo))
                    continue;

                PermisoComponent hijo = CrearComponente(permisos[idHijo]);

                if (hijo.EsFamilia)
                {
                    // Cada rama lleva su propia lista de visitados.
                    // Asi se permite repetir componentes en ramas distintas, pero no generar ciclos.
                    List<int> idsRamaHija = new List<int>(idsEnRama);
                    idsRamaHija.Add(hijo.Id);

                    // Recursion: si el hijo es familia, se buscan sus hijos y se agregan al Composite.
                    foreach (PermisoComponent nieto in CrearHijos(hijo.Id, permisos, relaciones, idsRamaHija))
                        hijo.AgregarHijo(nieto);
                }

                hijos.Add(hijo);
            }

            hijos.Sort((x, y) => string.Compare(x.Nombre, y.Nombre, StringComparison.OrdinalIgnoreCase));
            return hijos;
        }

        private bool PerteneceAlPadre(DataRow relacion, int? idPadre)
        {
            // idPadre null representa raiz; en base se guarda como id_permiso_padre NULL.
            if (!idPadre.HasValue)
                return relacion["id_permiso_padre"] == DBNull.Value;

            if (relacion["id_permiso_padre"] == DBNull.Value)
                return false;

            return Convert.ToInt32(relacion["id_permiso_padre"]) == idPadre.Value;
        }

        private PermisoComponent CrearComponente(DataRow fila)
        {
            int id = Convert.ToInt32(fila["id_permiso"]);
            string nombre = fila["nombre"].ToString();
            bool esFamilia = Convert.ToBoolean(fila["es_familia"]);

            if (esFamilia)
                return FamiliaPermiso.CargarDesdeDB(id, nombre);

            return PermisoSimple.CargarDesdeDB(id, nombre);
        }
    }
}
