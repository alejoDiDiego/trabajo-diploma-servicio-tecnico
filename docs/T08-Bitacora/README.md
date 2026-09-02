# T08 - Bitácora de Actividades

## a. Objetivo

Registrar todas las operaciones significativas realizadas por los usuarios en el sistema, almacenando usuario, fecha, tipo de actividad, descripción y detalle crítico. El sistema debe permitir visualizar el historial completo y filtrar por usuario, rango de fechas y tipo de actividad.

---

## b. Descripción detallada de cómo funciona

### Conceptos fundamentales

- **EntradaBitacora:** Entidad que representa una entrada individual en la bitácora. Contiene ID, usuario, fecha, tipo de actividad, descripción y detalle.

- **Bitácora automática:** 23 operaciones registradas desde 7 services distintos. Cada service recibe `BitacoraService` por inyección de dependencias y registra operaciones relevantes.

- **Inicialización temprana:** `BitacoraService.Inicializar()` se ejecuta en `Program.cs` startup para crear la tabla.

- **Búsqueda combinada:** El método `Buscar` acepta filtros opcionales de usuario, rango de fechas y tipo. Los filtros se combinan con AND en SQL.

### Operaciones registradas

| Service | Operaciones registradas |
|---------|------------------------|
| **UsuarioService** | Crear, modificar, eliminar usuario |
| **UsuarioPermisoService** | Asignar/desasignar permiso a usuario |
| **PermisoService** | Crear, modificar, eliminar permiso; componer |
| **IdiomaService** | Crear/eliminar idioma, guardar traducción |
| **ControlCambioService** | Restauración de cambio |
| **IntegridadService** | Recálculo de DV |
| **FrmPrincipal** | Cierre de sesión |

### Pantalla de Bitácora

- **FrmBitácora** con:
  - Header azul corporativo
  - Panel de búsqueda (usuario, fechas inicio/fin, tipo de actividad)
  - DataGridView de solo lectura con todas las entradas
  - Búsqueda por filtros combinados
  - Menú `Usuario → Bitácora` verifica permiso `BITACORA_VER`

### Seguridad

- Permiso `BITACORA_VER` asignado a la familia `Administrador`
- Solo se puede leer, no modificar ni eliminar entradas
- 37 traducciones registradas para interfaz y validaciones

---

## c. Diagrama de clases

![T08 Class Diagram](T08-ClassDiagram.puml)

```
Clases principales:
- EntradaBitacora: Entidad con datos de cada entrada
- BitacoraService: Orquestador de registro, listado y búsqueda
- BitacoraRepository: Persistencia en tabla Bitacora
```

---

## d. DER (Diagrama de Entidad-Relación)

![T08 DER](T08-DER.puml)

**Tablas:**

| Tabla | Descripción |
|-------|-------------|
| **Bitacora** | Almacena todas las entradas de la bitácora. |

---

## e. Diagramas de secuencia

Los siguientes diagramas de secuencia están disponibles en `diagrams/`:

| Archivo | Descripción |
|---------|-------------|
| `29-BitacoraFormLoad.puml` | Carga del formulario de bitácora |
| `30-BitacoraBuscar.puml` | Búsqueda por filtros combinados |

---

## f. Casos de Uso

### CU-05: Consultar Bitácora de Actividades (Diagrama 29)

| Campo | Descripción |
|-------|-------------|
| **DESCRIPCIÓN** | El usuario visualiza el listado completo de todas las actividades registradas en la bitácora del sistema, ordenadas por fecha descendente. |
| **ACTOR PRINCIPAL** | Usuario (con permiso `BITACORA_VER`) |
| **ACTORES SECUNDARIOS** | Sistema (BitacoraService, BitacoraRepository) |
| **PRECONDICIONES** | El usuario tiene el permiso `BITACORA_VER`. |
| **PUNTOS DE EXTENSIÓN** | Buscar entradas (CU-06) |
| **ESCENARIO PRINCIPAL** | 1. El usuario hace clic en "Bitácora". 2. El sistema verifica el permiso. 3. El sistema abre FrmBitacora modal. 4. El sistema llama a BitacoraService.Listar(). 5. El servicio delega en BitacoraRepository.Listar(). 6. El repositorio devuelve todas las entradas ordenadas por fecha DESC. 7. El servicio retorna la lista al formulario. 8. El formulario bindea el DataGridView en modo solo lectura. 9. El formulario se muestra en modo modal. |
| **FLUJOS ALTERNATIVOS** | *Sin entradas registradas (A1):* La grilla se muestra vacía. |
| **POSTCONDICIONES** | El formulario se muestra con las entradas. No hay modificaciones en BD. |

### CU-06: Buscar Entradas en Bitácora (Diagrama 30)

| Campo | Descripción |
|-------|-------------|
| **DESCRIPCIÓN** | El usuario filtra las entradas de bitácora mediante una combinación de criterios: usuario, rango de fechas (desde/hasta) y tipo de actividad. Los filtros se combinan con AND en SQL. |
| **ACTOR PRINCIPAL** | Usuario (con permiso `BITACORA_VER`) |
| **ACTORES SECUNDARIOS** | Sistema (BitacoraService, BitacoraRepository) |
| **PRECONDICIONES** | El usuario tiene permiso `BITACORA_VER`. El formulario de Bitácora está abierto (CU-05). |
| **PUNTOS DE EXTENSIÓN** | Ninguno |
| **ESCENARIO PRINCIPAL** | 1. El usuario completa los filtros deseados (usuario, fecha desde, fecha hasta, tipo actividad) y hace clic en "Buscar". 2. El sistema valida los filtros ingresados. 3. El sistema llama a BitacoraService.Buscar(usuario, desde, hasta, tipoActividad). 4. El servicio delega en BitacoraRepository.Buscar() que construye un WHERE dinámico combinando los filtros con AND. 5. El repositorio ejecuta la consulta y devuelve un DataTable con los resultados. 6. El servicio retorna el DataTable al formulario. 7. El formulario refresca el DataGridView con los resultados. |
| **FLUJOS ALTERNATIVOS** | *Sin filtros (A1):* Al no ingresar ningún filtro, se muestran todas las entradas (mismo resultado que CU-05). *Sin resultados (A2):* La grilla se muestra vacía. |
| **POSTCONDICIONES** | La grilla se actualiza con los resultados de la búsqueda. No hay modificaciones en BD. |
