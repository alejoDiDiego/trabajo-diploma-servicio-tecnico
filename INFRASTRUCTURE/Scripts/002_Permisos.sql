IF OBJECT_ID('Permisos', 'U') IS NULL
BEGIN
    -- Catalogo de componentes del Composite: familias y permisos simples.
    -- No se usa codigo ni descripcion; el nombre es unico y el id es la identidad tecnica.
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

IF OBJECT_ID('PermisoComposicion', 'U') IS NULL
BEGIN
    -- Relaciones padre-hijo del Composite.
    -- id_permiso_padre NULL representa la raiz virtual, no una fila de Permisos.
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
    -- Evita el mismo componente repetido directamente debajo de la raiz.
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
    -- Evita duplicados en el mismo nivel, pero permite repetir el componente en otra rama.
    CREATE UNIQUE INDEX UX_PermisoComposicion_Padre_Hijo
    ON PermisoComposicion(id_permiso_padre, id_permiso_hijo)
    WHERE id_permiso_padre IS NOT NULL;
END

IF COL_LENGTH('Permisos', 'id_permiso_padre') IS NOT NULL
BEGIN
    -- Migracion desde el modelo viejo: cada Permiso tenia un unico padre.
    -- En el modelo nuevo, la jerarquia vive en PermisoComposicion.
    INSERT INTO PermisoComposicion (id_permiso_padre, id_permiso_hijo)
    SELECT p.id_permiso_padre, p.id_permiso
    FROM Permisos p
    WHERE p.id_permiso_padre IS NOT NULL
      AND NOT EXISTS (
          SELECT 1
          FROM PermisoComposicion pc
          WHERE pc.id_permiso_padre = p.id_permiso_padre
            AND pc.id_permiso_hijo = p.id_permiso
      );

    -- Las familias sin padre viejo pasan a colgar de la raiz virtual.
    INSERT INTO PermisoComposicion (id_permiso_padre, id_permiso_hijo)
    SELECT NULL, p.id_permiso
    FROM Permisos p
    WHERE p.id_permiso_padre IS NULL
      AND p.es_familia = 1
      AND NOT EXISTS (
          SELECT 1
          FROM PermisoComposicion pc
          WHERE pc.id_permiso_padre IS NULL
            AND pc.id_permiso_hijo = p.id_permiso
      );
END

CREATE TABLE #PermisosSimples (
    nombre nvarchar(100),
    -- Solo se usa para reconocer permisos de bases viejas antes de eliminar codigo.
    codigo_legacy nvarchar(100)
);

INSERT INTO #PermisosSimples (nombre, codigo_legacy) VALUES
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
    DECLARE @SeedSql nvarchar(max);

    SET @SeedSql = N'
    UPDATE p
    SET p.nombre = s.nombre,
        p.es_familia = 0
    FROM Permisos p
    INNER JOIN #PermisosSimples s ON s.codigo_legacy = p.codigo;

    INSERT INTO Permisos (nombre, es_familia)
    SELECT s.nombre, 0
    FROM #PermisosSimples s
    WHERE NOT EXISTS (SELECT 1 FROM Permisos p WHERE p.nombre = s.nombre)
      AND NOT EXISTS (SELECT 1 FROM Permisos p WHERE p.codigo = s.codigo_legacy);';

    EXEC sp_executesql @SeedSql;
END
ELSE
BEGIN
    INSERT INTO Permisos (nombre, es_familia)
    SELECT s.nombre, 0
    FROM #PermisosSimples s
    WHERE NOT EXISTS (SELECT 1 FROM Permisos p WHERE p.nombre = s.nombre);
END

DECLARE @sql nvarchar(max);

IF COL_LENGTH('Permisos', 'id_permiso_padre') IS NOT NULL
BEGIN
    -- Se elimina la columna legacy: el padre ya no pertenece al catalogo Permisos.
    SET @sql = N'';

    SELECT @sql = @sql + N'ALTER TABLE ' + QUOTENAME(SCHEMA_NAME(t.schema_id)) + N'.' + QUOTENAME(t.name)
        + N' DROP CONSTRAINT ' + QUOTENAME(fk.name) + N';'
    FROM sys.foreign_keys fk
    INNER JOIN sys.tables t ON fk.parent_object_id = t.object_id
    INNER JOIN sys.foreign_key_columns fkc ON fk.object_id = fkc.constraint_object_id
    INNER JOIN sys.columns c ON fkc.parent_object_id = c.object_id AND fkc.parent_column_id = c.column_id
    WHERE t.name = 'Permisos'
      AND c.name = 'id_permiso_padre';

    IF LEN(@sql) > 0
        EXEC sp_executesql @sql;

    ALTER TABLE Permisos DROP COLUMN id_permiso_padre;
END

IF COL_LENGTH('Permisos', 'codigo') IS NOT NULL
BEGIN
    -- Se elimina codigo porque el sistema queda por id + nombre unico.
    SET @sql = N'';

    SELECT @sql = @sql + N'ALTER TABLE ' + QUOTENAME(SCHEMA_NAME(t.schema_id)) + N'.' + QUOTENAME(t.name)
        + N' DROP CONSTRAINT ' + QUOTENAME(kc.name) + N';'
    FROM sys.key_constraints kc
    INNER JOIN sys.tables t ON kc.parent_object_id = t.object_id
    INNER JOIN sys.index_columns ic ON kc.parent_object_id = ic.object_id AND kc.unique_index_id = ic.index_id
    INNER JOIN sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
    WHERE t.name = 'Permisos'
      AND c.name = 'codigo';

    IF LEN(@sql) > 0
        EXEC sp_executesql @sql;

    SET @sql = N'';

    SELECT @sql = @sql + N'DROP INDEX ' + QUOTENAME(i.name) + N' ON '
        + QUOTENAME(SCHEMA_NAME(t.schema_id)) + N'.' + QUOTENAME(t.name) + N';'
    FROM sys.indexes i
    INNER JOIN sys.tables t ON i.object_id = t.object_id
    INNER JOIN sys.index_columns ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id
    INNER JOIN sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
    WHERE t.name = 'Permisos'
      AND c.name = 'codigo'
      AND i.is_primary_key = 0
      AND i.is_unique_constraint = 0;

    IF LEN(@sql) > 0
        EXEC sp_executesql @sql;

    ALTER TABLE Permisos DROP COLUMN codigo;
END

IF COL_LENGTH('Permisos', 'descripcion') IS NOT NULL
    -- descripcion no participa del nuevo modelo de permisos.
    ALTER TABLE Permisos DROP COLUMN descripcion;

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'UX_Permisos_Nombre'
      AND object_id = OBJECT_ID('Permisos')
)
BEGIN
    -- Nombre unico para familias y permisos simples.
    CREATE UNIQUE INDEX UX_Permisos_Nombre
    ON Permisos(nombre);
END

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
