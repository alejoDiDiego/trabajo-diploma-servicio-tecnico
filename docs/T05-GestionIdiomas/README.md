# T05 - Gestión de Múltiples Idiomas

## a. Objetivo

Permitir el cambio dinámico de idioma de todas las leyendas, títulos y textos visibles en las interfaces de usuario del sistema, utilizando el patrón **Observer** con un modelo desacoplado de la UI. El sistema debe permitir incorporar nuevos idiomas y administrar las traducciones asociadas sin utilizar hojas de recursos estáticos (archivos `.resx`).

---

## b. Descripción detallada de cómo funciona

### Conceptos fundamentales

- **Idioma (Idioma):** Representa un idioma del sistema (ej: "Espanol", "Ingles"). Cada idioma posee su propio conjunto de traducciones.

- **Palabra (Palabra):** Clave única que identifica un texto traducible en el sistema (ej: `"Menu.IniciarSesion"`, `"FrmLogin.Text"`). Es el catálogo mantenido por los desarrolladores.

- **Traducción (Traduccion):** Relaciona un idioma con una palabra, almacenando el texto traducido para esa clave.

- **SesionIdioma (Sujeto Observable):** Singleton que mantiene el idioma actual del sistema. Cuando se cambia el idioma, notifica a todos los observadores registrados.

- **Observadores (IObservador):** Todos los formularios del sistema implementan `IObservador.Actualizar()`. Cuando reciben la notificación, recorren sus controles y actualizan los textos usando la clave almacenada en la propiedad `Tag` de cada control.

### Funcionamiento general

1. **Inicialización:** Al arrancar, `IdiomaRepository.Inicializar()` crea las tablas `Idiomas`, `Palabras` y `Traducciones`, y siembra datos iniciales (Español e Inglés con ~150 traducciones cada uno para cubrir todos los textos del sistema).

2. **Registro como observador:** Cada formulario, en su evento `Load`, se registra como observador mediante `SesionIdioma.GetInstance().RegistrarObservador(this)`. Cuando el formulario se cierra, se desregistra en `OnFormClosed`.

3. **Cambio de idioma:**
   - Desde `FrmPrincipal`, el usuario selecciona un idioma del menú `Idioma`.
   - `SesionIdioma.CambiarIdioma(idioma)` actualiza la propiedad `idioma` y llama a `ActualizarObservadores()`.
   - El método recorre la lista de observadores registrados y ejecuta `observador.Actualizar(idioma)` en cada uno.
   - Cada formulario traduce sus controles usando `idioma.BuscarTraduccion(Tag.ToString())`.

4. **Traducción de controles:**
   - Cada control (label, botón, título de ventana) tiene asignada una clave en su propiedad `Tag` (ej: `"Menu.AdministrarUsuarios"`).
   - En `Actualizar()`, el formulario busca la traducción de cada clave con `idioma.BuscarTraduccion(clave)`.
   - Si la clave no existe, se devuelve la propia clave como fallback.

5. **Administración de traducciones:** `FrmAdministrarTraducciones` permite:
   - *Editar Traducción:* Modifica el texto traducido para una palabra en el idioma seleccionado. Luego refresca el idioma actual para que todos los formularios abiertos reflejen los cambios inmediatamente (notificación Observer).
   - *Mantener el idioma actual sincronizado:* Después de crear/editar/eliminar un idioma, se refresca el idioma actual desde la base de datos llamando a `RefrescarIdiomaActual()`, que obtiene el idioma actualizado y dispara el cambio a todos los observadores.

6. **Administración de idiomas:**
   - *Crear:* Agrega un nuevo idioma al catálogo.
   - *Editar:* Cambia el nombre del idioma.
   - *Eliminar:* Elimina el idioma y sus traducciones (ON DELETE CASCADE). No permite eliminar el último idioma restante.

7. **Seguridad:** Cada operación verifica permisos específicos mediante `CodigosPermiso` (ej: `IDIOMAS_CREAR`, `IDIOMAS_EDITAR`, `TRADUCCIONES_EDITAR`). Los botones se ocultan/deshabilitan según los permisos del usuario logueado.

---

## c. Diagrama de clases

![T05 Class Diagram](T05-ClassDiagram.puml)

```
Patrón Observer:
- IObservado (Sujeto abstracto): Define registrar, desregistrar y notificar
- SesionIdioma (Sujeto concreto - Singleton): Mantiene el idioma actual y notifica cambios
- IObservador: Define el método Actualizar(IIdioma)
- Todos los Forms implementan IObservador

Modelo de idiomas:
- IIdioma / Idioma: Representa un idioma con sus traducciones
- ITraduccion / Traduccion: Texto traducido para una palabra en un idioma
- IPalabra / Palabra: Clave única del catálogo de textos traducibles
```

---

## d. DER (Diagrama de Entidad-Relación)

![T05 DER](T05-DER.puml)

**Tablas:**

| Tabla | Descripción |
|-------|-------------|
| **Idiomas** | Catálogo de idiomas disponibles en el sistema. |
| **Palabras** | Catálogo de claves de traducción (mantenidas por desarrolladores). |
| **Traducciones** | Texto traducido para cada combinación idioma-palabra. |

---

## e. Diagramas de secuencia

Los siguientes diagramas de secuencia están disponibles en `diagrams/`:

| Archivo | Descripción |
|---------|-------------|
| `13-TraduccionesFormLoad.puml` | Carga del formulario de administración de traducciones |
| `14-AdministrarTraduccionesMDI.puml` | Apertura desde el menú MDI |
| `15-IdiomaMDI.puml` | Cambio de idioma (notificación a todos los observadores) |
| `16-EditarTraduccion.puml` | Edición de una traducción con refresco de observers |
| `17-LimpiarTraduccion.puml` | Limpieza del panel de traducción |
| `18-CrearIdioma.puml` | Creación de un nuevo idioma |
| `19-EditarIdioma.puml` | Edición del nombre de un idioma |
| `20-EliminarIdioma.puml` | Eliminación de un idioma (con verificación) |
| `21-LimpiarIdioma.puml` | Limpieza del panel de idioma |
