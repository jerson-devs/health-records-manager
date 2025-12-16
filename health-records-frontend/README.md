# Health Records Manager - Frontend

Frontend del sistema Health Records Manager desarrollado con Angular 17, NgRx (Redux), y Angular Material 17.

## 🚀 Tecnologías

- **Angular 17** - Framework frontend
- **NgRx (Redux)** - Gestión de estado
- **Angular Material 17** - UI Components
- **RxJS** - Programación reactiva
- **TypeScript** - Tipado estático
- **Reactive Forms** - Formularios reactivos

## 📋 Requisitos

- Node.js 18+ 
- npm o pnpm
- Angular CLI 17+

## 🔧 Instalación

1. Instalar dependencias:
```bash
npm install
# o
pnpm install
```

2. Configurar la URL de la API en `src/environments/environment.ts`:
```typescript
export const environment = {
  production: false,
  apiUrl: 'http://localhost:5252/api/v1'
};
```

3. Ejecutar aplicación en desarrollo:
```bash
npm start
# o
pnpm start
```

La aplicación estará disponible en `http://localhost:4200`

## 🏗️ Estructura del Proyecto

```
src/app/
├── core/                    # Módulo core (singleton)
│   ├── guards/              # Guards de ruta
│   ├── interceptors/        # HTTP interceptors
│   ├── models/              # Interfaces y tipos TypeScript
│   │   ├── api-response.models.ts
│   │   ├── auth.models.ts
│   │   ├── medical-record.models.ts
│   │   ├── patient.models.ts
│   │   └── state.models.ts  # ⭐ Estados centralizados
│   ├── services/            # Servicios HTTP
│   │   ├── api.service.ts
│   │   ├── auth.service.ts
│   │   ├── patient.service.ts
│   │   └── medical-record.service.ts
│   └── store/               # NgRx Store
│       ├── auth/
│       ├── patients/
│       ├── medical-records/
│       └── index.ts
├── features/                # Módulos de features
│   ├── auth/
│   │   └── login/
│   ├── patients/
│   │   ├── patients-list/
│   │   ├── patient-detail/
│   │   └── patient-form/
│   └── medical-records/
│       ├── record-detail/
│       └── record-form/
└── shared/                  # Componentes compartidos
    └── components/
        └── navbar/
```

## 📊 Gestión de Estado (NgRx)

El proyecto utiliza NgRx para gestión de estado siguiendo el patrón Redux:

### Estados Centralizados

Todos los estados están definidos en `core/models/state.models.ts`:

```typescript
export interface AppState {
  auth: AuthState;
  patients: PatientsState;
  medicalRecords: MedicalRecordsState;
}
```

### Flujo de Datos

```
Component → Action → Effect → Service (HTTP) → Action → Reducer → Store → Component
```

### Características

- ✅ Estados centralizados en un solo archivo
- ✅ EntityAdapter para operaciones CRUD optimizadas
- ✅ Effects para manejar side effects (HTTP, localStorage)
- ✅ Selectores memoizados para mejor rendimiento
- ✅ Type safety completo con TypeScript

Ver [documentación completa de Redux](docs/REDUX_ARCHITECTURE.md)

## 🎨 Angular Material

El proyecto usa Angular Material 17 con:

- **Tema personalizado**: Colores médicos (azul primario, verde accent, rojo warn)
- **Tipografía**: Roboto (configurada globalmente)
- **Componentes**: Cards, Tables, Forms, Buttons, Icons, etc.
- **Responsive**: Diseño adaptable a diferentes tamaños de pantalla

### Configuración del Tema

El tema está configurado en `src/styles.scss`:

```scss
$health-records-primary: mat.define-palette(mat.$blue-palette, 600, 300, 800);
$health-records-accent: mat.define-palette(mat.$green-palette, 500, 200, 700);
$health-records-warn: mat.define-palette(mat.$red-palette);
```

## 🔐 Autenticación

El sistema utiliza JWT Bearer tokens:

1. El usuario inicia sesión en `/login`
2. El token se almacena en localStorage
3. El interceptor JWT agrega el token a todas las peticiones HTTP
4. El guard de autenticación protege las rutas privadas

## 📱 Características

### Pacientes
- ✅ Lista de pacientes con tabla Material
- ✅ Detalle de paciente con información completa
- ✅ Formulario de creación/edición
- ✅ Eliminación con confirmación

### Historiales Médicos
- ✅ Lista de historiales por paciente
- ✅ Detalle de historial médico
- ✅ Formulario de creación/edición
- ✅ Eliminación con confirmación

### Autenticación
- ✅ Login con validación
- ✅ Manejo de errores
- ✅ Logout
- ✅ Protección de rutas

## 🛠️ Scripts Disponibles

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

## 📝 Convenciones de Código

### Componentes
- Usar `OnPush` change detection cuando sea posible
- Usar `async` pipe para suscripciones
- Usar variables de template para evitar múltiples evaluaciones del async pipe

### Servicios
- Solo hacer llamadas HTTP
- NO conocer el Store
- Retornar Observables

### Effects
- Manejar todos los side effects
- Validar y mapear tipos antes de llamar a servicios
- Dispatchar acciones de éxito/error

### Reducers
- Ser funciones puras
- Importar tipos de estado desde `state.models.ts`
- No hacer side effects

## 🐛 Solución de Problemas

### Error: "Property 'loading' does not exist"
**Solución**: Usar `loading$ | async` en lugar de `loading` en templates.

### Error: "Type 'Partial<X>' is not assignable"
**Solución**: Mapear tipos en effects antes de llamar a servicios.

### Error: "Type 'X[] | null' is not assignable to CdkTableDataSourceInput"
**Solución**: Usar variables de template: `*ngIf="(data$ | async) as data"` y `[dataSource]="data || []"`

### Error: "No argument named $headline"
**Solución**: Angular Material v17 cambió la API de tipografía. Usar `mat.define-typography-config()` sin parámetros.

## 📚 Documentación Adicional

- [Arquitectura Redux](docs/REDUX_ARCHITECTURE.md) - Documentación completa de NgRx
- [Backend README](../docs/README.md) - Documentación del backend
- [API Documentation](../docs/API.md) - Documentación de endpoints

## 🧪 Testing

```bash
# Ejecutar tests
npm test

# Tests con cobertura
npm run test:coverage
```

## 📦 Build para Producción

```bash
# Compilar
npm run build

# Los archivos compilados estarán en dist/
```

## 🚀 Deployment

1. Compilar para producción:
```bash
npm run build
```

2. Los archivos estáticos estarán en `dist/health-records-frontend/`

3. Servir con cualquier servidor web estático (nginx, Apache, etc.)

## 📝 Licencia

Este proyecto es de demostración técnica.
