-- Script de configuración para Azure SQL Database
-- Health Records Manager
-- Autor: Health Records System
-- Fecha: 2025-01-16

-- ============================================
-- CONFIGURACIÓN INICIAL
-- ============================================

-- Nota: Este script debe ejecutarse después de crear la base de datos en Azure SQL
-- Usar Azure Portal o Azure CLI para crear la base de datos primero

-- ============================================
-- 1. CONFIGURAR FIREWALL RULES
-- ============================================
-- Nota: Las reglas de firewall se configuran desde Azure Portal o Azure CLI
-- Ejemplo con Azure CLI:
-- az sql server firewall-rule create \
--   --resource-group health-records-rg \
--   --server health-records-server \
--   --name AllowAzureServices \
--   --start-ip-address 0.0.0.0 \
--   --end-ip-address 0.0.0.0

-- ============================================
-- 2. CREAR USUARIOS Y PERMISOS
-- ============================================

-- Crear usuario para la aplicación (si no existe)
-- Nota: En Azure SQL, los usuarios se crean desde el servidor principal
-- Este script asume que ya tienes un usuario creado

-- Crear esquema para la aplicación
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'healthrecords')
BEGIN
    EXEC('CREATE SCHEMA healthrecords')
END
GO

-- ============================================
-- 3. CONFIGURAR PERMISOS
-- ============================================

-- Otorgar permisos al esquema
-- Ejecutar como administrador o usuario con permisos suficientes

-- ============================================
-- 4. VERIFICAR CONFIGURACIÓN
-- ============================================

-- Verificar que el esquema existe
SELECT name, schema_id 
FROM sys.schemas 
WHERE name = 'healthrecords'
GO

-- Verificar usuarios de la base de datos
SELECT name, type_desc, authentication_type_desc
FROM sys.database_principals
WHERE type IN ('S', 'U', 'G')
GO

-- ============================================
-- 5. CONFIGURAR OPCIONES DE BASE DE DATOS
-- ============================================

-- Configurar collation (si es necesario)
-- ALTER DATABASE [HealthRecordsDB] COLLATE SQL_Latin1_General_CP1_CI_AS
-- GO

-- Configurar opciones de compatibilidad
ALTER DATABASE [HealthRecordsDB] SET COMPATIBILITY_LEVEL = 160 -- SQL Server 2022
GO

-- Habilitar opciones recomendadas
ALTER DATABASE [HealthRecordsDB] SET ANSI_NULLS ON
GO

ALTER DATABASE [HealthRecordsDB] SET ANSI_PADDING ON
GO

ALTER DATABASE [HealthRecordsDB] SET ANSI_WARNINGS ON
GO

ALTER DATABASE [HealthRecordsDB] SET ARITHABORT ON
GO

ALTER DATABASE [HealthRecordsDB] SET CONCAT_NULL_YIELDS_NULL ON
GO

ALTER DATABASE [HealthRecordsDB] SET QUOTED_IDENTIFIER ON
GO

-- ============================================
-- 6. CONFIGURAR BACKUP
-- ============================================
-- Nota: En Azure SQL, los backups automáticos están habilitados por defecto
-- Configurar retention policy desde Azure Portal:
-- - Short-term retention: 7-35 días
-- - Long-term retention: Configurar según necesidades

-- ============================================
-- 7. CONFIGURAR MONITOREO
-- ============================================
-- Nota: Azure SQL proporciona métricas automáticas
-- Configurar alertas desde Azure Portal para:
-- - DTU/CPU usage
-- - Storage usage
-- - Connection count
-- - Deadlocks
-- - Slow queries

-- ============================================
-- 8. CONFIGURAR SECURITY
-- ============================================

-- Habilitar Advanced Threat Protection (desde Azure Portal)
-- Habilitar Transparent Data Encryption (TDE) - Habilitado por defecto
-- Configurar Azure AD authentication (opcional pero recomendado)

-- ============================================
-- 9. VERIFICAR CONECTIVIDAD
-- ============================================

-- Probar conexión desde la aplicación
-- Connection String ejemplo:
-- Server=tcp:health-records-server.database.windows.net,1433;
-- Initial Catalog=HealthRecordsDB;
-- Persist Security Info=False;
-- User ID=your-admin-user@health-records-server;
-- Password=your-password;
-- MultipleActiveResultSets=False;
-- Encrypt=True;
-- TrustServerCertificate=False;
-- Connection Timeout=30;

-- ============================================
-- 10. NOTAS IMPORTANTES
-- ============================================

-- 1. Firewall Rules:
--    - Configurar reglas para permitir acceso desde App Service
--    - Permitir acceso desde tu IP para desarrollo
--    - Considerar usar Private Endpoints para mayor seguridad

-- 2. Connection Strings:
--    - Usar Azure Key Vault para almacenar connection strings
--    - No hardcodear credenciales en código
--    - Usar Managed Identity cuando sea posible

-- 3. Performance:
--    - Monitorear DTU/CPU usage
--    - Considerar usar Read Replicas para lectura
--    - Optimizar queries y usar índices apropiados

-- 4. Security:
--    - Usar Azure AD authentication cuando sea posible
--    - Habilitar Advanced Threat Protection
--    - Revisar regularmente los audit logs

-- 5. Backup & Recovery:
--    - Configurar retention policy apropiada
--    - Probar restore procedures regularmente
--    - Documentar recovery procedures

-- ============================================
-- FIN DEL SCRIPT
-- ============================================

PRINT 'Script de configuración de Azure SQL Database completado.'
PRINT 'Recuerda:'
PRINT '1. Configurar firewall rules desde Azure Portal'
PRINT '2. Crear usuarios y asignar permisos'
PRINT '3. Configurar connection strings en App Service'
PRINT '4. Aplicar migraciones de Entity Framework'
PRINT '5. Configurar backups y monitoring'

