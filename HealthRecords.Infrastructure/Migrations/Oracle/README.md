# Migraciones Oracle

Esta carpeta contiene las migraciones específicas para Oracle Database.

## Crear una nueva migración para Oracle

Para crear una migración específica para Oracle, primero asegúrate de que `DatabaseProvider` en `appsettings.json` esté configurado como `"Oracle"`, luego ejecuta:

```bash
dotnet ef migrations add NombreMigracion --project ../HealthRecords.Infrastructure --startup-project ../../HealthRecords.API --context ApplicationDbContext
```

## Notas importantes

- Las migraciones de Oracle deben manejar diferencias de sintaxis respecto a PostgreSQL
- Oracle usa secuencias (SEQUENCE) en lugar de IDENTITY para auto-incremento
- Los nombres de tablas y columnas en Oracle se convierten automáticamente a mayúsculas
- Oracle tiene límites de longitud de nombres (30 caracteres en versiones antiguas, 128 en versiones recientes)

## Diferencias clave

1. **Secuencias**: Oracle requiere secuencias explícitas para auto-incremento
2. **Tipos de datos**: 
   - `VARCHAR2` en lugar de `VARCHAR`
   - `NUMBER` en lugar de `INTEGER` o `BIGINT`
   - `DATE` o `TIMESTAMP` para fechas
3. **Nombres**: Oracle convierte automáticamente a mayúsculas (a menos que uses comillas)

