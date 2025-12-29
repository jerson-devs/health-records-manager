# Health Records Manager

Sistema completo de Gestión de Historiales Médicos desarrollado con .NET Core 8 y Angular 17, implementando Clean Architecture, NgRx (Redux), y autenticación JWT.

## 🏗️ Arquitectura

El proyecto sigue **Clean Architecture** con separación clara de responsabilidades:

```
health-records-manager/
├── HealthRecords.API/              # Capa de presentación (Controllers, Middleware)
├── HealthRecords.Application/      # Lógica de negocio (Services, DTOs, Mappers)
├── HealthRecords.Domain/           # Entidades y contratos (Models, Interfaces)
├── HealthRecords.Infrastructure/   # Implementaciones (Repositories, DbContext, Config)
│   └── Migrations/                 # Migraciones de Entity Framework
├── HealthRecords.Tests.Unit/       # Tests unitarios
├── health-records-frontend/        # Frontend Angular 17 con NgRx
└── scripts/                        # Scripts SQL y utilidades
```

## 🚀 Tecnologías

### Backend
- **.NET Core 8** - Framework principal
- **Entity Framework Core 8** - ORM con PostgreSQL
- **PostgreSQL** - Base de datos
- **JWT Bearer** - Autenticación
- **Swagger/OpenAPI** - Documentación de API
- **FluentValidation** - Validaciones
- **Clean Architecture** - Separación de capas

### Frontend
- **Angular 17** - Framework frontend
- **NgRx (Redux)** - Gestión de estado
- **Angular Material 17** - UI Components
- **Reactive Forms** - Formularios reactivos
- **RxJS** - Programación reactiva
- **TypeScript** - Tipado estático

## 📋 Requisitos

### Backend
- .NET 8 SDK
- PostgreSQL 12+
- Visual Studio 2022 o VS Code
- dotnet-ef CLI (para migraciones)

### Frontend
- Node.js 18+
- npm o pnpm
- Angular CLI 17+

## 🔧 Instalación Rápida

### 1. Clonar el Repositorio

```bash
git clone <url-del-repositorio>
cd health-records-manager
```

### 2. Configurar Base de Datos

1. Crear la base de datos PostgreSQL:
```sql
CREATE DATABASE "HealthRecordsDB";
```

2. Configurar cadena de conexión en `HealthRecords.API/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=HealthRecordsDB;Username=postgres;Password=tu_password"
  }
}
```

3. Instalar dotnet-ef (si no está instalado):
```bash
dotnet tool install --global dotnet-ef
```

4. Aplicar migraciones:
```bash
dotnet ef database update --project HealthRecords.Infrastructure --startup-project HealthRecords.API
```

5. Crear usuario inicial (ver `LEVANTAR_APP.md` para el SQL completo):
```sql
INSERT INTO "USERS" ("USERNAME", "EMAIL", "PASSWORD_HASH", "ROLE", "CREATED_AT")
VALUES ('admin', 'admin@healthrecords.com', '$2a$11$6h25Ry/IE.M59G7Ubb.OHOpFYlIXrxGMtA3QsFT9THPfknnhOreB.', 'Admin', NOW())
ON CONFLICT ("USERNAME") DO NOTHING;
```

**Credenciales por defecto:**
- Username: `admin`
- Password: `Admin123!`

### 3. Levantar Backend

```bash
dotnet run --project HealthRecords.API
```

La API estará disponible en:
- **HTTP:** http://localhost:5252
- **HTTPS:** https://localhost:7053
- **Swagger:** http://localhost:5252/swagger

### 4. Levantar Frontend

```bash
cd health-records-frontend
npm install  # o pnpm install
npm start    # o pnpm start
```

La aplicación estará disponible en:
- http://localhost:4200

> 📖 **Para instrucciones detalladas, consulta [LEVANTAR_APP.md](LEVANTAR_APP.md)**

## 📊 Endpoints Principales

### Autenticación
- `POST /api/v1/auth/login` - Iniciar sesión
- `POST /api/v1/auth/refresh` - Refrescar token

### Pacientes
- `GET /api/v1/patients` - Listar pacientes
- `GET /api/v1/patients/{id}` - Obtener paciente
- `GET /api/v1/patients/{id}/records` - Obtener paciente con historiales
- `POST /api/v1/patients` - Crear paciente
- `PUT /api/v1/patients/{id}` - Actualizar paciente
- `DELETE /api/v1/patients/{id}` - Eliminar paciente

### Historiales Médicos
- `GET /api/v1/medicalrecords` - Listar historiales
- `GET /api/v1/medicalrecords/{id}` - Obtener historial
- `POST /api/v1/medicalrecords` - Crear historial
- `PUT /api/v1/medicalrecords/{id}` - Actualizar historial
- `DELETE /api/v1/medicalrecords/{id}` - Eliminar historial

## 🗄️ Base de Datos

### Estructura de Tablas

- **PATIENTS**: Pacientes del sistema
- **MEDICAL_RECORDS**: Historiales médicos
- **USERS**: Usuarios del sistema (autenticación)

Todas las tablas incluyen:
- Campos de auditoría (`CREATED_AT`, `UPDATED_AT`)
- Índices para optimización
- Constraints de integridad referencial

### Migraciones

```bash
# Aplicar migraciones
dotnet ef database update --project HealthRecords.Infrastructure --startup-project HealthRecords.API

# Crear nueva migración
dotnet ef migrations add NombreMigracion --project HealthRecords.Infrastructure --startup-project HealthRecords.API
```

## 🎨 Frontend - Arquitectura

### Gestión de Estado (NgRx)

El frontend utiliza NgRx para gestión de estado siguiendo el patrón Redux:

```
Component → Action → Effect → Service (HTTP) → Action → Reducer → Store → Component
```

**Características:**
- ✅ Estados centralizados en `core/models/state.models.ts`
- ✅ EntityAdapter para operaciones CRUD optimizadas
- ✅ Effects para manejar side effects (HTTP, localStorage)
- ✅ Selectores memoizados para mejor rendimiento
- ✅ Type safety completo con TypeScript

### Estructura del Frontend

```
src/app/
├── core/                    # Módulo core (singleton)
│   ├── guards/              # Guards de ruta
│   ├── interceptors/        # HTTP interceptors
│   ├── models/              # Interfaces y tipos TypeScript
│   ├── services/            # Servicios HTTP
│   └── store/               # NgRx Store
│       ├── auth/
│       ├── patients/
│       └── medical-records/
├── features/                # Módulos de features
│   ├── auth/
│   ├── patients/
│   └── medical-records/
└── shared/                  # Componentes compartidos
```

### Angular Material

El proyecto usa Angular Material 17 con:
- **Tema personalizado**: Colores médicos (azul primario, verde accent, rojo warn)
- **Tipografía**: Roboto (configurada globalmente)
- **Componentes**: Cards, Tables, Forms, Buttons, Icons, etc.
- **Responsive**: Diseño adaptable a diferentes tamaños de pantalla

## 🔐 Autenticación

El sistema utiliza JWT Bearer tokens:

1. El usuario inicia sesión en `/login`
2. El token se almacena en localStorage
3. El interceptor JWT agrega el token a todas las peticiones HTTP
4. El guard de autenticación protege las rutas privadas

### Configuración JWT

En `HealthRecords.API/appsettings.json`:

```json
{
  "JWT": {
    "Issuer": "http://localhost:5252",
    "Audience": "http://localhost:5252",
    "SigningKey": "tu-clave-secreta-super-segura",
    "AccessTokenExpirationMinutes": 15,
    "RefreshTokenExpirationDays": 7
  }
}
```

## 📱 Características

### Pacientes
- ✅ Lista de pacientes con tabla Material
- ✅ Detalle de paciente con información completa
- ✅ Formulario de creación/edición
- ✅ Eliminación con confirmación
- ✅ Búsqueda y filtrado

### Historiales Médicos
- ✅ Lista de historiales por paciente
- ✅ Detalle de historial médico
- ✅ Formulario de creación/edición
- ✅ Eliminación con confirmación
- ✅ Relación con pacientes

### Autenticación
- ✅ Login con validación
- ✅ Manejo de errores
- ✅ Logout
- ✅ Protección de rutas
- ✅ Refresh token

## 🛠️ Scripts Disponibles

### Backend

```bash
# Desarrollo
dotnet run --project HealthRecords.API
dotnet watch run --project HealthRecords.API

# Migraciones
dotnet ef database update --project HealthRecords.Infrastructure --startup-project HealthRecords.API
dotnet ef migrations add NombreMigracion --project HealthRecords.Infrastructure --startup-project HealthRecords.API

# Testing
dotnet test

# Restaurar paquetes
dotnet restore
```

### Frontend

```bash
# Desarrollo
npm start              # Inicia servidor de desarrollo
npm run build          # Compila para producción
npm run watch          # Compila en modo watch

# Testing
npm test               # Ejecuta tests unitarios
npm run test:watch     # Ejecuta tests en modo watch

# Linting
npm run lint           # Ejecuta el linter
```

## 🧪 Testing

### Backend

```bash
dotnet test
```

### Frontend

```bash
npm test
npm run test:coverage
```

## 📝 Convenciones de Código

### Backend
- Clean Architecture con separación de capas
- DTOs para transferencia de datos
- Mappers para conversión entre entidades y DTOs
- Repositorios para acceso a datos
- Servicios para lógica de negocio
- Validaciones con FluentValidation

### Frontend
- Componentes con `OnPush` change detection cuando sea posible
- Uso de `async` pipe para suscripciones
- Variables de template para evitar múltiples evaluaciones
- Servicios solo para llamadas HTTP
- Effects para manejar side effects
- Reducers como funciones puras

## 🐛 Solución de Problemas

### Backend

**Error de conexión a PostgreSQL:**
- Verifica que PostgreSQL esté ejecutándose
- Verifica las credenciales en `appsettings.json`
- Asegúrate de que la base de datos exista

**Error de migraciones:**
- Asegúrate de tener dotnet-ef instalado
- Verifica la cadena de conexión
- Intenta eliminar y recrear las migraciones

### Frontend

**Error de conexión a la API:**
- Verifica que el backend esté corriendo
- Verifica la URL en `src/environments/environment.ts`
- Revisa la consola del navegador

**Error de compilación:**
- Verifica que todas las dependencias estén instaladas
- Intenta limpiar el proyecto: `npm run build -- --delete-output-path`

## 📦 Build para Producción

### Backend

```bash
dotnet publish -c Release -o ./publish
```

### Frontend

```bash
cd health-records-frontend
npm run build
```

Los archivos compilados estarán en `dist/health-records-frontend/`

## 🚀 Deployment

### Backend
1. Compilar: `dotnet publish -c Release`
2. Configurar variables de entorno
3. Configurar base de datos de producción
4. Desplegar en servidor (IIS, Azure, AWS, etc.)

### Frontend
1. Compilar: `npm run build`
2. Servir archivos estáticos con nginx, Apache, etc.
3. Configurar proxy para API si es necesario

## 📚 Documentación Adicional

- [LEVANTAR_APP.md](LEVANTAR_APP.md) - Guía detallada para levantar la aplicación
- Swagger UI - Documentación interactiva de la API (disponible cuando el backend está corriendo)

## 🎯 Próximos Pasos

1. ✅ Sistema de autenticación JWT
2. ✅ CRUD de pacientes
3. ✅ CRUD de historiales médicos
4. ✅ Frontend con Angular Material
5. ✅ Gestión de estado con NgRx
6. 🔄 Tests unitarios y de integración
7. 🔄 Documentación de API completa
8. 🔄 CI/CD pipeline

## 📝 Licencia

Este proyecto es de demostración técnica.

## 👥 Autor

Desarrollado como proyecto de demostración de expertise en .NET Core 8, Angular 17, Clean Architecture, y arquitectura escalable.

---

**¿Necesitas ayuda?** Consulta [LEVANTAR_APP.md](LEVANTAR_APP.md) para instrucciones detalladas de instalación y solución de problemas.



