# T06 - Integridad de Datos (DVH/DVV)

## a. Objetivo

Garantizar la integridad de los datos almacenados en la tabla `Usuarios` mediante dígitos verificadores horizontales (DVH) por fila y un dígito verificador vertical (DVV) general de la tabla. El sistema debe verificar la integridad en cada inicio de sesión y permitir el recálculo manual de todos los dígitos verificadores.

---

## b. Descripción detallada de cómo funciona

### Conceptos fundamentales

- **DVH (Dígito Verificador Horizontal):** Valor SHA256 calculado sobre la concatenación de todos los atributos de un usuario, separados por su posición ordinal (`username|1|password|2|id|3`). Se almacena en la columna `dvh` de la tabla `Usuarios`.

- **DVV (Dígito Verificador Vertical):** Valor SHA256 calculado sobre la concatenación de todos los DVH ordenados por ID (`dvh1|id1|dvh2|id2|...`). Se almacena en la tabla `DigitosVerticales` con el nombre de la tabla como clave.

### Funcionamiento general

1. **Inicialización:** Al arrancar el sistema, `IntegridadService.Inicializar()` crea la tabla `DigitosVerticales` y agrega la columna `dvh` a `Usuarios` si no existe. Luego, `UsuarioService.Inicializar()` llama a `RecalcularDVVUsuarios()` para precalcular el DVV.

2. **Cálculo de DVH:** Al crear o modificar un usuario, se calcula el DVH inmediatamente:
   - Se concatenan los valores de cada propiedad con su posición: `username|1|password|2|id_rol|3|id|4`
   - Se aplica SHA256 sobre esa cadena
   - Se persiste en la columna `dvh`

3. **Cálculo de DVV:** Después de cada operación que afecte a la tabla de usuarios (crear, modificar, eliminar), se recalcula el DVV completo:
   - Se obtienen todos los pares `(dvh, id)` ordenados por ID
   - Se concatenan: `dvh1|id1|dvh2|id2|...`
   - Se aplica SHA256 y se guarda en `DigitosVerticales`

4. **Verificación en login:** Cada vez que un usuario inicia sesión:
   - `IntegridadService.VerificarIntegridadUsuarios()` recorre todos los usuarios
   - Recalcula el DVH de cada uno y lo compara con el almacenado
   - Recalcula el DVV total y lo compara con el almacenado
   - Si alguna comparación falla, `SessionManager.IntegridadComprometida = true`
   - `FrmLogin` muestra una advertencia al usuario pero permite continuar

5. **Recálculo manual:** Desde `FrmPrincipal` menú `Usuario → Recalcular DV`, con permiso `INTEGRIDAD_RECALCULAR`:
   - `IntegridadService.RecalcularTodosDV()` recalcula DVH de todos los usuarios
   - Luego recalcula el DVV desde cero
   - Se registra la operación en la Bitácora

6. **Excepciones:** Ante cualquier error en la verificación, se captura la excepción y se retorna `false` (integridad comprometida), evitando que un error técnico deje al usuario sin poder acceder.

7. **Seguridad:** El permiso `INTEGRIDAD_RECALCULAR` está asignado a la familia `Administrador`. Solo los usuarios con ese permiso pueden ver y ejecutar el recálculo.

---

## c. Diagrama de clases

![T06 Class Diagram](T06-ClassDiagram.puml)

```
Clases principales:
- DigitoVerificadorHelper: Métodos estáticos para calcular DVH y DVV usando SHA256
- IntegridadService: Orquestador de verificación y recálculo
- IntegridadRepository: Persistencia de DVV en DigitosVerticales
- UserDVH: DTO con DVH e ID para cálculo vertical
- Usuario (existente): Entidad con DVH como propiedad adicional
```

---

## d. DER (Diagrama de Entidad-Relación)

![T06 DER](T06-DER.puml)

**Tablas:**

| Tabla | Descripción |
|-------|-------------|
| **Usuarios** | Tabla existente con columna adicional `dvh` para el DVH de cada fila. |
| **DigitosVerticales** | Almacena el DVV por tabla. `nombre_tabla` como clave única. |

---

## e. Diagramas de secuencia

Los siguientes diagramas de secuencia están disponibles en `diagrams/`:

| Archivo | Descripción |
|---------|-------------|
| `25-LoginVerificaIntegridad.puml` | Inicio de sesión con verificación de integridad de datos |
| `26-RecalcularDV.puml` | Recálculo manual de todos los dígitos verificadores |

---

## f. Casos de Uso

### CU-01: Iniciar Sesión con Verificación de Integridad (Diagrama 25)

| Campo | Descripción |
|-------|-------------|
| **DESCRIPCIÓN** | El sistema verifica la integridad de los datos de la tabla Usuarios durante el inicio de sesión calculando DVH por fila y DVV general. Si se detecta una manipulación no autorizada, se informa al usuario pero se permite el acceso. |
| **ACTOR PRINCIPAL** | Usuario |
| **ACTORES SECUNDARIOS** | Sistema (PasswordHasher, UsuarioService, IntegridadService, DigitoVerificadorHelper, BitacoraService, SessionManager) |
| **PRECONDICIONES** | El usuario existe con credenciales válidas. Las tablas Usuarios (columna dvh) y DigitosVerticales (registro "Usuarios") existen. |
| **PUNTOS DE EXTENSIÓN** | Ninguno |
| **ESCENARIO PRINCIPAL** | 1. El usuario ingresa usuario y password y presiona Iniciar Sesión. 2. El sistema obtiene el usuario desde el repositorio. 3. El sistema verifica el hash del password. 4. El sistema inicia sesión en SessionManager. 5. El sistema verifica la integridad: recalcula DVH de cada usuario y lo compara con el almacenado, recalcula DVV y lo compara con el almacenado. 6. La integridad es correcta. 7. El sistema registra "Inicio de sesión" en bitácora. 8. El sistema cierra el formulario con DialogResult.OK. |
| **FLUJOS ALTERNATIVOS** | *Password incorrecto (A1):* El sistema registra "Inicio de sesión fallido" en bitácora y muestra mensaje de error. *Integridad comprometida (A2):* El sistema muestra una advertencia al usuario pero permite continuar. SessionManager.IntegridadComprometida = true. |
| **POSTCONDICIONES** | Sesión iniciada. SessionManager.IntegridadComprometida refleja el estado. Se registró entrada en bitácora. |

### CU-02: Recalcular Dígitos Verificadores (Diagrama 26)

| Campo | Descripción |
|-------|-------------|
| **DESCRIPCIÓN** | El administrador recalcula manualmente todos los DVH y el DVV de la tabla Usuarios para restaurar la integridad después de modificaciones legítimas o verificar consistencia. |
| **ACTOR PRINCIPAL** | Administrador (con permiso `INTEGRIDAD_RECALCULAR`) |
| **ACTORES SECUNDARIOS** | Sistema (IntegridadService, DigitoVerificadorHelper, UsuarioRepository, IntegridadRepository, BitacoraService) |
| **PRECONDICIONES** | El usuario tiene el permiso `INTEGRIDAD_RECALCULAR`. Existen usuarios registrados. |
| **PUNTOS DE EXTENSIÓN** | Ninguno |
| **ESCENARIO PRINCIPAL** | 1. El usuario hace clic en "Recalcular DV". 2. El sistema verifica el permiso. 3. El sistema confirma la operación mediante MessageBox. 4. El sistema itera sobre todos los usuarios: calcula DVH con DigitoVerificadorHelper y actualiza la columna dvh en BD. 5. El sistema obtiene todos los pares (dvh, id) y calcula el DVV. 6. El sistema guarda el DVV en DigitosVerticales. 7. El sistema registra "Recálculo de DV" en bitácora. 8. El sistema actualiza IntegridadComprometida = false. 9. El sistema muestra mensaje de éxito. |
| **FLUJOS ALTERNATIVOS** | *Cancelación (A1):* El usuario cancela el mensaje de confirmación. No se ejecuta el recálculo. |
| **POSTCONDICIONES** | Todos los DVH y el DVV están recalculados y persistidos. IntegridadComprometida = false. Se registró en bitácora. |
