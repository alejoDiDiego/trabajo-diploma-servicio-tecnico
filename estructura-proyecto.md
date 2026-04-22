# Estructura del Proyecto - TP Integrador

## Arquitectura DDD/Capas

```
TP_INTEGRADOR/
├── DOMAIN/
│   ├── Features/Usuarios/
│   └── Exceptions/
├── APPLICATION/
│   ├── Features/Usuarios/
│   │   ├── DTOs/
│   │   ├── Interfaces/
│   │   └── exceptions/
├── INFRASTRUCTURE/
│   └── Features/Usuarios/
├── PRESENTATION/
│   └── Forms/Auth/
└── CROSSCUTTING/
    ├── Auth/
    ├── Configuration/
    └── Security/
```

## Descripción de Capas

| Capa | Propósito |
|------|----------|
| **DOMAIN** | Entidades, reglas de negocio, excepciones del dominio |
| **APPLICATION** | Casos de uso, DTOs, interfaces de servicios |
| **INFRASTRUCTURE** | Implementación de acceso a datos (SQL) |
| **PRESENTATION** | Formularios WinForms (UI) |
| **CROSSCUTTING** | Auth, seguridad, configuración global |

## Tecnologías

- **.NET Framework 4.7.2**
- **C#**
- **WinForms**
- **SQL Server**