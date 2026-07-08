# T04 - Gestión de Perfiles de Usuario

## a. Objetivo

Administrar los permisos del sistema mediante una estructura jerárquica que permita agrupar permisos atómicos en familias (perfiles), asignar dichos perfiles a usuarios, y componer la arquitectura de autorización utilizando el patrón **Composite**. El sistema debe exponer un árbol de permisos en un control `TreeView` mediante funciones recursivas.

---

## b. Descripción detallada de cómo funciona

### Conceptos fundamentales

- **Permiso atómico (PermisoSimple):** Representa una funcionalidad indivisible del sistema (ej: "Crear usuario", "Ver permisos"). Cada permiso atómico posee un código único (`USUARIOS_CREAR`, `PERMISOS_VER`, etc.) que se utiliza en las validaciones de autorización desde el código.

- **Permiso compuesto / Familia (FamiliaPermiso):** Agrupa permisos atómicos y/o subfamilias. Una familia actúa como un perfil reutilizable (ej: "Gestion usuarios", "Administrador").

- **Raíz virtual:** Nodo raíz del `TreeView` que no existe como registro en la base de datos, pero sirve como contenedor visual de todas las familias de primer nivel.

### Funcionamiento general

1. **Inicialización del sistema:** Al arrancar, se crean las tablas `Permisos` (catálogo de componentes) y `PermisoComposicion` (relaciones padre-hijo). Se cargan permisos atómicos base (19 permisos de semilla agrupados en 6 familias iniciales: Administrador, Gestion usuarios, Gestion permisos, Gestion idiomas, Gestion traducciones, Lectura general).

2. **Visualización del árbol:** `PermisoService.ListarArbol()` construye la estructura completa del Composite mediante una función recursiva (`PermisoRepository.CrearHijos`) que recorre las relaciones de `PermisoComposicion`, crea objetos de dominio y los asigna como hijos de sus respectivas familias. El resultado se vuelca en un `TreeView` de Windows Forms.

3. **CRUD de familias:**
   - *Crear:* Agrega una fila a `Permisos` con `es_familia=1`. No aparece en el árbol hasta que se vinculan como hijas de algún padre.
   - *Editar:* Cambia el nombre de la familia en el catálogo; todas sus apariciones en el árbol se actualizan automáticamente porque comparten el mismo ID.
   - *Eliminar:* Borra la fila de `Permisos` y todas sus relaciones en `PermisoComposicion`. No elimina los permisos hijos del catálogo.

4. **Composición del árbol:**
   - *Agregar Familia:* Vincula una familia existente como hija de la raíz o de otra familia. Valida duplicados en el mismo nivel, autocontención y relaciones circulares.
   - *Agregar Permiso:* Vincula un permiso atómico a una familia (no a la raíz, ya que la raíz solo puede contener familias).
   - *Quitar Seleccionado:* Elimina la relación de composición, no el componente del catálogo.

5. **Asignación a usuarios:**
   - En `FrmAsignarPermisosUsuario` se muestran dos listas: **disponibles** (componentes del catálogo no asignados al usuario) y **asignados** (componentes ya vinculados).
   - Al asignar: `UsuarioPermisoService.AsignarPermiso()` inserta en `UsuarioPermisos`.
   - Al quitar: `UsuarioPermisoService.QuitarPermiso()` elimina el vínculo.
   - Los permisos efectivos se calculan recursivamente expandiendo las familias asignadas hasta obtener los códigos atómicos.

6. **Seguridad de la pantalla:** Cada botón verifica permisos específicos (`CodigosPermiso`) antes de ejecutar su acción, mostrando "Acceso denegado" si corresponde.

---

## c. Diagrama de clases

![T04 Class Diagram](T04-ClassDiagram.puml)

```
Patrón Composite:
- IPermisoComponent: Interfaz común (Component)
- PermisoComponent: Clase abstracta (Component)
- PermisoSimple: Hoja (Leaf) - Permiso atómico sin hijos
- FamiliaPermiso: Compuesto (Composite) - Puede contener hijos

Clases de soporte:
- Usuario: Representa un usuario del sistema
- CodigosPermiso: Constantes con los códigos de permiso
```

---

## d. DER (Diagrama de Entidad-Relación)

![T04 DER](T04-DER.puml)

**Tablas:**

| Tabla | Descripción |
|-------|-------------|
| **Permisos** | Catálogo único de todos los componentes (familias y atómicos). `es_familia` distingue el tipo. |
| **PermisoComposicion** | Relaciones padre-hijo. `id_permiso_padre` NULL indica raíz virtual. |
| **Usuarios** | Usuarios del sistema. |
| **UsuarioPermisos** | Asignación directa de componentes a usuarios. |

---

## e. Diagramas de secuencia

Los siguientes diagramas de secuencia están disponibles en `diagrams/`:

| Archivo | Descripción |
|---------|-------------|
| `01-PermisosFormLoad.puml` | Carga inicial del formulario de permisos |
| `02-AdministrarPermisosMDI.puml` | Apertura desde el menú MDI |
| `03-CrearFamilia.puml` | Creación de una familia en el catálogo |
| `04-EditarFamilia.puml` | Edición del nombre de una familia |
| `05-EliminarFamilia.puml` | Eliminación de una familia (con confirmación) |
| `06-AgregarFamilia.puml` | Vinculación de familia en el árbol |
| `07-AgregarPermiso.puml` | Vinculación de permiso atómico en una familia |
| `08-QuitarSeleccionado.puml` | Desvinculación de componente del árbol |
| `09-Limpiar.puml` | Limpieza de selección |
| `10-AsignarPermisosFormLoad.puml` | Carga del formulario de asignación |
| `11-BotonSeleccionarPermiso.puml` | Asignación de permiso a usuario (>) |
| `12-BotonDesasignarPermiso.puml` | Desasignación de permiso a usuario (<) |
