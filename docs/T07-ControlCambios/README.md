# T07 - Control de Cambios (Auditoría de Traducciones e Idiomas)

## a. Objetivo

Registrar todas las modificaciones realizadas sobre las tablas `Idiomas` y `Traducciones`, almacenando los valores anteriores y nuevos junto con el usuario y la fecha del cambio. El sistema debe permitir visualizar el historial de cambios y restaurar versiones anteriores de forma individual, manteniendo la integridad referencial mediante restauración en cascada.

---

## b. Descripción detallada de cómo funciona

### Conceptos fundamentales

- **ControlCambio:** Entidad que registra un cambio atómico sobre una fila de `Idiomas` o `Traducciones`. Almacena tabla afectada, ID del registro, campo modificado, valor anterior, valor nuevo, usuario, fecha y tipo de cambio (INSERT/UPDATE/DELETE).

- **Restauración en cascada:** Al restaurar una traducción cuyo idioma fue eliminado, primero se restaura el idioma automáticamente para mantener la integridad referencial.

- **Nombre del idioma:** La grilla muestra el nombre del idioma mediante un `LEFT JOIN` con la tabla `Idiomas`. Si el idioma fue eliminado, se muestra `"(eliminado)"` usando un `CASE WHEN`.

### Funcionamiento general

1. **Registro automático:** Cada operación CRUD sobre idiomas y traducciones registra un cambio:
   - `IdiomaService.CrearIdioma()` → registra INSERT
   - `IdiomaService.EliminarIdioma()` → registra DELETE (antes de borrar)
   - `IdiomaService.GuardarTraduccion()` → registra INSERT (nueva) o UPDATE (existente)

2. **Visualización:** `FrmControlCambios` muestra una grilla con columnas: Tabla, Idioma, Fecha, Usuario, Tipo, Clave, Valor Anterior, Valor Nuevo. Permisos `CONTROL_CAMBIOS_VER` para ver y `CONTROL_CAMBIOS_RESTAURAR` para restaurar.

3. **Restauración individual:** El usuario selecciona un cambio y hace clic en "Restaurar":
   - **UPDATE:** Se restaura el valor anterior en la tabla afectada y se registra un nuevo cambio UPDATE (swap de valores).
   - **INSERT:** Se elimina la fila creada (DELETE lógico sobre traducciones).
   - **DELETE:** Se reinserta la fila eliminada.

4. **Restauración en cascada:** Si al restaurar una traducción el idioma no existe (porque fue eliminado previamente), se busca un cambio de tipo DELETE sobre ese idioma en `ControlCambios` y se restaura primero el idioma, preservando su ID original mediante `SET IDENTITY_INSERT`.

5. **Restricciones:** No se permite restaurar la creación de un idioma (INSERT sobre `Idiomas`) porque implicaría eliminar el idioma y todas sus traducciones. Se muestra un mensaje indicando que debe eliminarse manualmente.

6. **Notificación al restaurar idioma:** Al restaurar un idioma eliminado, se llama a `SesionIdioma.ActualizarObservadores()` para que `FrmPrincipal` refresque el menú de idiomas y los formularios abiertos actualicen sus traducciones.

7. **Seguridad:** Permisos `CONTROL_CAMBIOS_VER` y `CONTROL_CAMBIOS_RESTAURAR` asignados a la familia `Gestion traducciones`.

---

## c. Diagrama de clases

![T07 Class Diagram](T07-ClassDiagram.puml)

```
Clases principales:
- ControlCambio: Entidad que almacena cada cambio atómico
- ControlCambioService: Orquestador de registro y restauración con cascada
- ControlCambioRepository: Persistencia en tabla ControlCambios
- IdiomaService: Inyecta registros en ControlCambioService al operar
```

---

## d. DER (Diagrama de Entidad-Relación)

![T07 DER](T07-DER.puml)

**Tablas:**

| Tabla | Descripción |
|-------|-------------|
| **ControlCambios** | Registro de cambios sobre Idiomas y Traducciones. |
| **Idiomas** | Referencia para mostrar nombre del idioma (LEFT JOIN). |
| **Traducciones** | Tabla afectada por los cambios de tipo INSERT/UPDATE/DELETE. |

---

## e. Diagramas de secuencia

Los siguientes diagramas de secuencia están disponibles en `diagrams/`:

| Archivo | Descripción |
|---------|-------------|
| `27-ControlCambiosFormLoad.puml` | Carga del formulario de control de cambios |
| `28-ControlCambiosRestaurar.puml` | Restauración de un cambio (con cascada) |

---

## f. Casos de Uso

### CU-03: Consultar Historial de Cambios (Diagrama 27)

| Campo | Descripción |
|-------|-------------|
| **DESCRIPCIÓN** | El usuario visualiza el historial completo de cambios realizados sobre las tablas Idiomas y Traducciones, mostrando valores anteriores y nuevos junto con usuario, fecha y tipo de operación. |
| **ACTOR PRINCIPAL** | Usuario (con permiso `CONTROL_CAMBIOS_VER`) |
| **ACTORES SECUNDARIOS** | Sistema (ControlCambioService, ControlCambioRepository) |
| **PRECONDICIONES** | El usuario tiene el permiso `CONTROL_CAMBIOS_VER`. |
| **PUNTOS DE EXTENSIÓN** | Restaurar cambio (CU-04) |
| **ESCENARIO PRINCIPAL** | 1. El usuario hace clic en "Control de Cambios". 2. El sistema verifica el permiso. 3. El sistema abre FrmControlCambios modal. 4. El sistema llama a ControlCambioService.Listar(). 5. El servicio delega en ControlCambioRepository.Listar() con LEFT JOIN a Idiomas. 6. El repositorio devuelve List&lt;ControlCambio&gt;. 7. El servicio retorna la lista al formulario. 8. El formulario bindea el DataGridView. 9. El formulario se muestra en modo modal. |
| **FLUJOS ALTERNATIVOS** | *Sin cambios registrados (A1):* La grilla se muestra vacía. |
| **POSTCONDICIONES** | El formulario se muestra con el historial. No hay modificaciones en BD. |

### CU-04: Restaurar Cambio (Diagrama 28)

| Campo | Descripción |
|-------|-------------|
| **DESCRIPCIÓN** | El usuario restaura un cambio específico del historial, revirtiendo los valores de una traducción o reinsertando un idioma eliminado. Si la traducción referencia un idioma eliminado, se restaura primero en cascada. |
| **ACTOR PRINCIPAL** | Usuario (con permiso `CONTROL_CAMBIOS_RESTAURAR`) |
| **ACTORES SECUNDARIOS** | Sistema (ControlCambioService, ControlCambioRepository, IdiomaRepository, BitacoraService, SesionIdioma) |
| **PRECONDICIONES** | El usuario tiene permiso `CONTROL_CAMBIOS_RESTAURAR`. El formulario de Control de Cambios está abierto (CU-03). El usuario seleccionó un cambio. |
| **PUNTOS DE EXTENSIÓN** | Ninguno |
| **ESCENARIO PRINCIPAL (UPDATE)** | 1. El usuario selecciona un cambio de tipo UPDATE y presiona "Restaurar". 2. El sistema verifica el permiso y confirma con MessageBox. 3. El sistema obtiene el cambio desde el repositorio. 4. El sistema actualiza la traducción con el valor_anterior mediante IdiomaRepository. 5. El sistema inserta un nuevo cambio en ControlCambios con los valores intercambiados. 6. El sistema registra "Cambio restaurado" en bitácora. 7. El sistema muestra mensaje de éxito y refresca la grilla. |
| **FLUJOS ALTERNATIVOS** | *Tipo INSERT (A1):* El sistema muestra mensaje "Debe eliminarse manualmente" y no ejecuta la restauración. *Tipo DELETE traducción con idioma existente (A2):* El sistema reinserta la traducción, inserta nuevo cambio y refresca grilla. *Tipo DELETE traducción con idioma eliminado (A3):* El sistema busca un DELETE de idioma en ControlCambios, restaura el idioma primero (SET IDENTITY_INSERT), notifica a SesionIdioma, luego reinserta la traducción. *Tipo DELETE idioma (A4):* El sistema reinserta el idioma (SET IDENTITY_INSERT), inserta nuevo cambio, notifica a SesionIdioma. |
| **POSTCONDICIONES** | El cambio fue revertido. Se registró un nuevo cambio en ControlCambios (swap). Se registró en bitácora. Si se restauró un idioma, SesionIdioma notificó a los observadores. |
