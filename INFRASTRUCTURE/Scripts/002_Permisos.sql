IF OBJECT_ID('Permisos', 'U') IS NULL
BEGIN
    CREATE TABLE Permisos (
        id_permiso int IDENTITY(1,1) NOT NULL PRIMARY KEY,
        nombre nvarchar(100) NOT NULL,
        codigo nvarchar(100) NULL,
        es_familia bit NOT NULL
    );
END
ELSE
BEGIN
    IF COL_LENGTH('Permisos', 'nombre') IS NULL
        ALTER TABLE Permisos ADD nombre nvarchar(100) NULL;

    IF COL_LENGTH('Permisos', 'codigo') IS NULL
        ALTER TABLE Permisos ADD codigo nvarchar(100) NULL;

    IF COL_LENGTH('Permisos', 'es_familia') IS NULL
        ALTER TABLE Permisos ADD es_familia bit NOT NULL CONSTRAINT DF_Permisos_EsFamilia DEFAULT 0;
END

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'UX_Permisos_Nombre'
      AND object_id = OBJECT_ID('Permisos')
)
    CREATE UNIQUE INDEX UX_Permisos_Nombre ON Permisos(nombre);

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'UX_Permisos_Codigo'
      AND object_id = OBJECT_ID('Permisos')
)
    CREATE UNIQUE INDEX UX_Permisos_Codigo ON Permisos(codigo) WHERE codigo IS NOT NULL;

IF OBJECT_ID('PermisoComposicion', 'U') IS NULL
BEGIN
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
    CREATE UNIQUE INDEX UX_PermisoComposicion_Raiz_Hijo
    ON PermisoComposicion(id_permiso_hijo)
    WHERE id_permiso_padre IS NULL;

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'UX_PermisoComposicion_Padre_Hijo'
      AND object_id = OBJECT_ID('PermisoComposicion')
)
    CREATE UNIQUE INDEX UX_PermisoComposicion_Padre_Hijo
    ON PermisoComposicion(id_permiso_padre, id_permiso_hijo)
    WHERE id_permiso_padre IS NOT NULL;

CREATE TABLE #PermisosSimples (
    nombre nvarchar(100),
    codigo nvarchar(100),
    nombre_legacy nvarchar(100),
    codigo_legacy nvarchar(100)
);

INSERT INTO #PermisosSimples (nombre, codigo, nombre_legacy, codigo_legacy) VALUES
('Ver usuarios', 'USUARIOS_VER', 'Administrar usuarios', 'USUARIOS_ADMINISTRAR'),
('Crear usuarios', 'USUARIOS_CREAR', 'Crear usuarios', 'USUARIOS_CREAR'),
('Editar usuarios', 'USUARIOS_EDITAR', 'Editar usuarios', 'USUARIOS_EDITAR'),
('Eliminar usuarios', 'USUARIOS_ELIMINAR', 'Eliminar usuarios', 'USUARIOS_ELIMINAR'),
('Ver permisos', 'PERMISOS_VER', 'Administrar permisos', 'PERMISOS_ADMINISTRAR'),
('Crear familias', 'PERMISOS_CREAR', 'Crear familias', 'PERMISOS_CREAR'),
('Editar familias', 'PERMISOS_EDITAR', 'Editar familias', 'PERMISOS_EDITAR'),
('Eliminar familias', 'PERMISOS_ELIMINAR', 'Eliminar familias', 'PERMISOS_ELIMINAR'),
('Componer familias', 'PERMISOS_COMPONER', 'Componer familias', 'PERMISOS_MOVER'),
('Asignar permisos a usuarios', 'PERMISOS_ASIGNAR_USUARIOS', NULL, NULL),
('Ver idiomas', 'IDIOMAS_VER', NULL, NULL),
('Cambiar idioma', 'IDIOMAS_CAMBIAR', 'Cambiar idioma', 'IDIOMAS_CAMBIAR'),
('Crear idiomas', 'IDIOMAS_CREAR', NULL, NULL),
('Editar idiomas', 'IDIOMAS_EDITAR', NULL, NULL),
('Eliminar idiomas', 'IDIOMAS_ELIMINAR', NULL, NULL),
('Ver traducciones', 'TRADUCCIONES_VER', 'Administrar traducciones', 'TRADUCCIONES_ADMINISTRAR'),
('Crear traducciones', 'TRADUCCIONES_CREAR', NULL, NULL),
('Editar traducciones', 'TRADUCCIONES_EDITAR', NULL, NULL),
('Eliminar traducciones', 'TRADUCCIONES_ELIMINAR', NULL, NULL);

UPDATE p
SET p.nombre = s.nombre,
    p.codigo = s.codigo,
    p.es_familia = 0
FROM Permisos p
INNER JOIN #PermisosSimples s ON p.codigo = s.codigo;

UPDATE p
SET p.nombre = s.nombre,
    p.codigo = s.codigo,
    p.es_familia = 0
FROM Permisos p
INNER JOIN #PermisosSimples s ON p.codigo = s.codigo_legacy
WHERE s.codigo_legacy IS NOT NULL
  AND p.codigo <> s.codigo
  AND NOT EXISTS (SELECT 1 FROM Permisos px WHERE px.codigo = s.codigo);

UPDATE p
SET p.nombre = s.nombre,
    p.codigo = s.codigo,
    p.es_familia = 0
FROM Permisos p
INNER JOIN #PermisosSimples s ON UPPER(p.nombre) = UPPER(s.nombre_legacy)
WHERE p.codigo IS NULL
  AND s.nombre_legacy IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM Permisos px WHERE px.codigo = s.codigo);

INSERT INTO Permisos (nombre, codigo, es_familia)
SELECT s.nombre, s.codigo, 0
FROM #PermisosSimples s
WHERE NOT EXISTS (SELECT 1 FROM Permisos p WHERE p.codigo = s.codigo)
  AND NOT EXISTS (SELECT 1 FROM Permisos p WHERE UPPER(p.nombre) = UPPER(s.nombre));

CREATE TABLE #Familias (nombre nvarchar(100));

INSERT INTO #Familias (nombre) VALUES
('Administrador'),
('Gestion usuarios'),
('Gestion permisos'),
('Gestion idiomas'),
('Gestion traducciones'),
('Lectura general');

INSERT INTO Permisos (nombre, codigo, es_familia)
SELECT f.nombre, NULL, 1
FROM #Familias f
WHERE NOT EXISTS (SELECT 1 FROM Permisos p WHERE UPPER(p.nombre) = UPPER(f.nombre));

INSERT INTO PermisoComposicion (id_permiso_padre, id_permiso_hijo)
SELECT NULL, p.id_permiso
FROM #Familias f
INNER JOIN Permisos p ON UPPER(p.nombre) = UPPER(f.nombre) AND p.es_familia = 1
WHERE NOT EXISTS (
    SELECT 1
    FROM PermisoComposicion pc
    WHERE pc.id_permiso_padre IS NULL
      AND pc.id_permiso_hijo = p.id_permiso
);

CREATE TABLE #Composicion (
    padre nvarchar(100),
    hijo_codigo nvarchar(100) NULL,
    hijo_familia nvarchar(100) NULL
);

INSERT INTO #Composicion (padre, hijo_codigo, hijo_familia) VALUES
('Gestion usuarios', 'USUARIOS_VER', NULL),
('Gestion usuarios', 'USUARIOS_CREAR', NULL),
('Gestion usuarios', 'USUARIOS_EDITAR', NULL),
('Gestion usuarios', 'USUARIOS_ELIMINAR', NULL),
('Gestion permisos', 'PERMISOS_VER', NULL),
('Gestion permisos', 'PERMISOS_CREAR', NULL),
('Gestion permisos', 'PERMISOS_EDITAR', NULL),
('Gestion permisos', 'PERMISOS_ELIMINAR', NULL),
('Gestion permisos', 'PERMISOS_COMPONER', NULL),
('Gestion permisos', 'PERMISOS_ASIGNAR_USUARIOS', NULL),
('Gestion idiomas', 'IDIOMAS_VER', NULL),
('Gestion idiomas', 'IDIOMAS_CAMBIAR', NULL),
('Gestion idiomas', 'IDIOMAS_CREAR', NULL),
('Gestion idiomas', 'IDIOMAS_EDITAR', NULL),
('Gestion idiomas', 'IDIOMAS_ELIMINAR', NULL),
('Gestion traducciones', 'TRADUCCIONES_VER', NULL),
('Gestion traducciones', 'TRADUCCIONES_CREAR', NULL),
('Gestion traducciones', 'TRADUCCIONES_EDITAR', NULL),
('Gestion traducciones', 'TRADUCCIONES_ELIMINAR', NULL),
('Lectura general', 'USUARIOS_VER', NULL),
('Lectura general', 'PERMISOS_VER', NULL),
('Lectura general', 'IDIOMAS_VER', NULL),
('Lectura general', 'IDIOMAS_CAMBIAR', NULL),
('Lectura general', 'TRADUCCIONES_VER', NULL),
('Administrador', NULL, 'Gestion usuarios'),
('Administrador', NULL, 'Gestion permisos'),
('Administrador', NULL, 'Gestion idiomas'),
('Administrador', NULL, 'Gestion traducciones');

INSERT INTO PermisoComposicion (id_permiso_padre, id_permiso_hijo)
SELECT padre.id_permiso, hijo.id_permiso
FROM #Composicion c
INNER JOIN Permisos padre ON UPPER(padre.nombre) = UPPER(c.padre) AND padre.es_familia = 1
INNER JOIN Permisos hijo ON (
    (c.hijo_codigo IS NOT NULL AND hijo.codigo = c.hijo_codigo)
    OR
    (c.hijo_familia IS NOT NULL AND UPPER(hijo.nombre) = UPPER(c.hijo_familia) AND hijo.es_familia = 1)
)
WHERE NOT EXISTS (
    SELECT 1
    FROM PermisoComposicion pc
    WHERE pc.id_permiso_padre = padre.id_permiso
      AND pc.id_permiso_hijo = hijo.id_permiso
);

IF OBJECT_ID('Usuarios', 'U') IS NOT NULL
BEGIN
    IF OBJECT_ID('UsuarioPermisos', 'U') IS NULL
    BEGIN
        CREATE TABLE UsuarioPermisos (
            id_usuario_permiso int IDENTITY(1,1) NOT NULL PRIMARY KEY,
            id_usuario int NOT NULL,
            id_permiso_familia int NOT NULL,
            CONSTRAINT FK_UsuarioPermisos_Usuarios FOREIGN KEY (id_usuario)
                REFERENCES Usuarios(id_usuario) ON DELETE CASCADE,
            CONSTRAINT FK_UsuarioPermisos_Permisos FOREIGN KEY (id_permiso_familia)
                REFERENCES Permisos(id_permiso) ON DELETE CASCADE
        );
    END

    IF NOT EXISTS (
        SELECT 1 FROM sys.indexes
        WHERE name = 'UX_UsuarioPermisos_Usuario_Familia'
          AND object_id = OBJECT_ID('UsuarioPermisos')
    )
        CREATE UNIQUE INDEX UX_UsuarioPermisos_Usuario_Familia
        ON UsuarioPermisos(id_usuario, id_permiso_familia);
END

IF OBJECT_ID('Idiomas', 'U') IS NOT NULL
   AND OBJECT_ID('Palabras', 'U') IS NOT NULL
   AND OBJECT_ID('Traducciones', 'U') IS NOT NULL
BEGIN
    DECLARE @SeedTraducciones TABLE (
        idioma nvarchar(100),
        clave nvarchar(200),
        texto nvarchar(max)
    );

    INSERT INTO @SeedTraducciones (idioma, clave, texto) VALUES
    ('Espanol', 'Menu.AdministrarPermisos', 'Administrar permisos'),
    ('Ingles', 'Menu.AdministrarPermisos', 'Manage permissions'),
    ('Espanol', 'Menu.AsignarPermisosUsuarios', 'Asignar permisos a usuarios'),
    ('Ingles', 'Menu.AsignarPermisosUsuarios', 'Assign user permissions'),
    ('Espanol', 'FrmAdministrarPermisos.Text', 'Administracion de permisos'),
    ('Ingles', 'FrmAdministrarPermisos.Text', 'Permission administration'),
    ('Espanol', 'FrmAsignarPermisosUsuario.Text', 'Asignacion de permisos a usuarios'),
    ('Ingles', 'FrmAsignarPermisosUsuario.Text', 'User permission assignment'),
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
    ('Espanol', 'AsignarPermisos.Titulo', 'Asignacion de permisos a usuarios'),
    ('Ingles', 'AsignarPermisos.Titulo', 'User permission assignment'),
    ('Espanol', 'AsignarPermisos.Usuarios', 'Usuarios'),
    ('Ingles', 'AsignarPermisos.Usuarios', 'Users'),
    ('Espanol', 'AsignarPermisos.FamiliasDisponibles', 'Familias disponibles'),
    ('Ingles', 'AsignarPermisos.FamiliasDisponibles', 'Available families'),
    ('Espanol', 'AsignarPermisos.FamiliasAsignadas', 'Familias asignadas'),
    ('Ingles', 'AsignarPermisos.FamiliasAsignadas', 'Assigned families'),
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
    ('Espanol', 'Mensaje.SeleccioneUsuario', 'Seleccione un usuario.'),
    ('Ingles', 'Mensaje.SeleccioneUsuario', 'Select a user.'),
    ('Espanol', 'Mensaje.FamiliaAsignada', 'Familia asignada exitosamente.'),
    ('Ingles', 'Mensaje.FamiliaAsignada', 'Family assigned successfully.'),
    ('Espanol', 'Mensaje.FamiliaQuitadaUsuario', 'Familia quitada exitosamente.'),
    ('Ingles', 'Mensaje.FamiliaQuitadaUsuario', 'Family removed successfully.'),
    ('Espanol', 'Mensaje.ConfirmarEliminarFamilia', 'Estas seguro de eliminar completamente la familia ''{0}''? Se quitaran todas sus apariciones.'),
    ('Ingles', 'Mensaje.ConfirmarEliminarFamilia', 'Are you sure you want to delete family ''{0}'' completely? All its appearances will be removed.'),
    ('Espanol', 'Mensaje.ConfirmarQuitarComponente', 'Estas seguro de quitar ''{0}'' del nivel seleccionado?'),
    ('Ingles', 'Mensaje.ConfirmarQuitarComponente', 'Are you sure you want to remove ''{0}'' from the selected level?'),
    ('Espanol', 'Mensaje.ErrorPermiso', 'Error al gestionar permisos: {0}'),
    ('Ingles', 'Mensaje.ErrorPermiso', 'Permission management error: {0}'),
    ('Espanol', 'Mensaje.ErrorAsignarPermisos', 'Error al asignar permisos: {0}'),
    ('Ingles', 'Mensaje.ErrorAsignarPermisos', 'Permission assignment error: {0}');

    INSERT INTO Idiomas (nombre)
    SELECT DISTINCT s.idioma
    FROM @SeedTraducciones s
    WHERE NOT EXISTS (SELECT 1 FROM Idiomas i WHERE i.nombre = s.idioma);

    INSERT INTO Palabras (texto)
    SELECT DISTINCT s.clave
    FROM @SeedTraducciones s
    WHERE NOT EXISTS (SELECT 1 FROM Palabras p WHERE p.texto = s.clave);

    INSERT INTO Traducciones (id_idioma, id_palabra, palabra_traducida)
    SELECT i.id_idioma, p.id_palabra, s.texto
    FROM @SeedTraducciones s
    INNER JOIN Idiomas i ON i.nombre = s.idioma
    INNER JOIN Palabras p ON p.texto = s.clave
    WHERE NOT EXISTS (
        SELECT 1
        FROM Traducciones t
        WHERE t.id_idioma = i.id_idioma
          AND t.id_palabra = p.id_palabra
    );
END
