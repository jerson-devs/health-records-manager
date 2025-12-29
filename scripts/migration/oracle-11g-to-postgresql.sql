-- Script de Migración: Oracle 11g a PostgreSQL
-- Health Records Manager
-- Autor: Health Records System
-- Fecha: 2025-01-16
-- Descripción: Script completo para migrar datos desde Oracle 11g a PostgreSQL

-- ============================================
-- PREREQUISITOS
-- ============================================
-- 1. Las tablas deben estar creadas en PostgreSQL (usar migraciones EF Core)
-- 2. Tener acceso a ambas bases de datos (Oracle y PostgreSQL)
-- 3. Tener permisos de lectura en Oracle y escritura en PostgreSQL
-- 4. Ejecutar este script en PostgreSQL después de exportar datos de Oracle

-- ============================================
-- 1. EXPORTAR DATOS DE ORACLE
-- ============================================
-- Ejecutar en Oracle para exportar datos a CSV o usar herramientas como:
-- - Oracle SQL Developer: Export Data
-- - expdp (Data Pump)
-- - SQL*Plus con spool

-- Ejemplo de exportación desde Oracle:
/*
SET PAGESIZE 0
SET FEEDBACK OFF
SET HEADING OFF
SET ECHO OFF
SPOOL patients_export.csv
SELECT PATIENT_ID || ',' || 
       NOMBRE || ',' || 
       EMAIL || ',' || 
       TO_CHAR(FECHA_NACIMIENTO, 'YYYY-MM-DD') || ',' ||
       DOCUMENTO || ',' ||
       TO_CHAR(CREATED_AT, 'YYYY-MM-DD HH24:MI:SS') || ',' ||
       TO_CHAR(NVL(UPDATED_AT, CREATED_AT), 'YYYY-MM-DD HH24:MI:SS')
FROM PATIENTS;
SPOOL OFF
*/

-- ============================================
-- 2. TRANSFORMACIÓN DE TIPOS DE DATOS
-- ============================================

-- Oracle → PostgreSQL
-- NUMBER → INTEGER o BIGINT
-- VARCHAR2(n) → VARCHAR(n)
-- DATE → DATE o TIMESTAMP
-- TIMESTAMP → TIMESTAMP
-- CLOB → TEXT
-- BLOB → BYTEA
-- NUMBER(p,s) → DECIMAL(p,s) o NUMERIC(p,s)

-- ============================================
-- 3. IMPORTAR DATOS A POSTGRESQL
-- ============================================

-- Nota: Ajustar los valores según tus datos exportados
-- Este script asume que los datos ya están en formato compatible

BEGIN;

-- ============================================
-- 3.1. IMPORTAR PATIENTS
-- ============================================

-- Opción A: Desde CSV usando COPY (más rápido)
-- COPY PATIENTS (PATIENT_ID, NOMBRE, EMAIL, FECHA_NACIMIENTO, DOCUMENTO, CREATED_AT, UPDATED_AT)
-- FROM '/path/to/patients_export.csv'
-- WITH (FORMAT csv, HEADER false, DELIMITER ',');

-- Opción B: Insert directo (para datos pequeños)
-- INSERT INTO PATIENTS (PATIENT_ID, NOMBRE, EMAIL, FECHA_NACIMIENTO, DOCUMENTO, CREATED_AT, UPDATED_AT)
-- VALUES
--   (1, 'Juan Pérez', 'juan@example.com', '1980-01-15', '12345678', '2024-01-01 10:00:00', NULL),
--   (2, 'María García', 'maria@example.com', '1985-05-20', '87654321', '2024-01-01 10:00:00', NULL);
--   -- ... más registros

-- Opción C: Usar función para transformar fechas
CREATE OR REPLACE FUNCTION import_patients_from_oracle()
RETURNS void AS $$
DECLARE
    v_patient RECORD;
BEGIN
    -- Aquí iría la lógica para leer desde Oracle y insertar en PostgreSQL
    -- Esto requiere una extensión como oracle_fdw o usar una herramienta externa
    RAISE NOTICE 'Esta función requiere configuración adicional para conectarse a Oracle';
END;
$$ LANGUAGE plpgsql;

-- ============================================
-- 3.2. IMPORTAR MEDICAL_RECORDS
-- ============================================

-- COPY MEDICAL_RECORDS (RECORD_ID, PATIENT_ID, FECHA, DIAGNOSTICO, TRATAMIENTO, MEDICO, CREATED_AT, UPDATED_AT)
-- FROM '/path/to/medical_records_export.csv'
-- WITH (FORMAT csv, HEADER false, DELIMITER ',');

-- ============================================
-- 3.3. IMPORTAR USERS
-- ============================================

-- COPY USERS (USER_ID, USERNAME, EMAIL, PASSWORD_HASH, ROLE, CREATED_AT, UPDATED_AT)
-- FROM '/path/to/users_export.csv'
-- WITH (FORMAT csv, HEADER false, DELIMITER ',');

-- ============================================
-- 4. ACTUALIZAR SECUENCIAS
-- ============================================

-- Asegurar que las secuencias estén en el valor correcto después de importar
SELECT setval('patients_patient_id_seq', (SELECT MAX(PATIENT_ID) FROM PATIENTS));
SELECT setval('medical_records_record_id_seq', (SELECT MAX(RECORD_ID) FROM MEDICAL_RECORDS));
SELECT setval('users_user_id_seq', (SELECT MAX(USER_ID) FROM USERS));

-- ============================================
-- 5. VERIFICACIÓN DE INTEGRIDAD
-- ============================================

-- 5.1. Verificar conteo de registros
DO $$
DECLARE
    v_patients_count INTEGER;
    v_records_count INTEGER;
    v_users_count INTEGER;
BEGIN
    SELECT COUNT(*) INTO v_patients_count FROM PATIENTS;
    SELECT COUNT(*) INTO v_records_count FROM MEDICAL_RECORDS;
    SELECT COUNT(*) INTO v_users_count FROM USERS;
    
    RAISE NOTICE 'Pacientes migrados: %', v_patients_count;
    RAISE NOTICE 'Historiales médicos migrados: %', v_records_count;
    RAISE NOTICE 'Usuarios migrados: %', v_users_count;
END;
$$;

-- 5.2. Verificar integridad referencial
DO $$
DECLARE
    v_orphan_records INTEGER;
BEGIN
    SELECT COUNT(*) INTO v_orphan_records
    FROM MEDICAL_RECORDS mr
    WHERE NOT EXISTS (
        SELECT 1 FROM PATIENTS p WHERE p.PATIENT_ID = mr.PATIENT_ID
    );
    
    IF v_orphan_records > 0 THEN
        RAISE WARNING 'Se encontraron % historiales médicos huérfanos (sin paciente asociado)', v_orphan_records;
    ELSE
        RAISE NOTICE 'Integridad referencial verificada: todos los historiales tienen paciente asociado';
    END IF;
END;
$$;

-- 5.3. Verificar valores nulos críticos
DO $$
DECLARE
    v_null_emails INTEGER;
    v_null_documents INTEGER;
BEGIN
    SELECT COUNT(*) INTO v_null_emails FROM PATIENTS WHERE EMAIL IS NULL;
    SELECT COUNT(*) INTO v_null_documents FROM PATIENTS WHERE DOCUMENTO IS NULL;
    
    IF v_null_emails > 0 THEN
        RAISE WARNING 'Se encontraron % pacientes sin email', v_null_emails;
    END IF;
    
    IF v_null_documents > 0 THEN
        RAISE WARNING 'Se encontraron % pacientes sin documento', v_null_documents;
    END IF;
END;
$$;

-- 5.4. Verificar duplicados
DO $$
DECLARE
    v_duplicate_emails INTEGER;
    v_duplicate_documents INTEGER;
BEGIN
    SELECT COUNT(*) INTO v_duplicate_emails
    FROM (
        SELECT EMAIL, COUNT(*) as cnt
        FROM PATIENTS
        GROUP BY EMAIL
        HAVING COUNT(*) > 1
    ) duplicates;
    
    SELECT COUNT(*) INTO v_duplicate_documents
    FROM (
        SELECT DOCUMENTO, COUNT(*) as cnt
        FROM PATIENTS
        GROUP BY DOCUMENTO
        HAVING COUNT(*) > 1
    ) duplicates;
    
    IF v_duplicate_emails > 0 THEN
        RAISE WARNING 'Se encontraron % emails duplicados', v_duplicate_emails;
    END IF;
    
    IF v_duplicate_documents > 0 THEN
        RAISE WARNING 'Se encontraron % documentos duplicados', v_duplicate_documents;
    END IF;
END;
$$;

-- ============================================
-- 6. CREAR ÍNDICES (si no existen)
-- ============================================

-- Los índices deberían estar creados por las migraciones EF Core
-- Pero verificamos que existan

DO $$
BEGIN
    -- Verificar índices de PATIENTS
    IF NOT EXISTS (SELECT 1 FROM pg_indexes WHERE indexname = 'IX_PATIENTS_EMAIL') THEN
        CREATE UNIQUE INDEX IX_PATIENTS_EMAIL ON PATIENTS(EMAIL);
        RAISE NOTICE 'Índice IX_PATIENTS_EMAIL creado';
    END IF;
    
    IF NOT EXISTS (SELECT 1 FROM pg_indexes WHERE indexname = 'IX_PATIENTS_DOCUMENTO') THEN
        CREATE UNIQUE INDEX IX_PATIENTS_DOCUMENTO ON PATIENTS(DOCUMENTO);
        RAISE NOTICE 'Índice IX_PATIENTS_DOCUMENTO creado';
    END IF;
    
    -- Verificar índices de MEDICAL_RECORDS
    IF NOT EXISTS (SELECT 1 FROM pg_indexes WHERE indexname = 'IX_MEDICAL_RECORDS_PATIENT_ID') THEN
        CREATE INDEX IX_MEDICAL_RECORDS_PATIENT_ID ON MEDICAL_RECORDS(PATIENT_ID);
        RAISE NOTICE 'Índice IX_MEDICAL_RECORDS_PATIENT_ID creado';
    END IF;
END;
$$;

-- ============================================
-- 7. APLICAR STORED PROCEDURES Y FUNCIONES
-- ============================================

-- Ejecutar los scripts de stored procedures de PostgreSQL
-- Ver: scripts/postgresql/procedures/
-- Ver: scripts/postgresql/functions/

-- ============================================
-- 8. APLICAR TRIGGERS
-- ============================================

-- Ejecutar los scripts de triggers de PostgreSQL
-- Ver: scripts/postgresql/triggers/

-- ============================================
-- 9. VACUUM Y ANALYZE
-- ============================================

-- Optimizar la base de datos después de la migración
VACUUM ANALYZE PATIENTS;
VACUUM ANALYZE MEDICAL_RECORDS;
VACUUM ANALYZE USERS;

-- ============================================
-- 10. FINALIZAR TRANSACCIÓN
-- ============================================

COMMIT;

-- ============================================
-- NOTAS FINALES
-- ============================================

-- 1. Verificar que todos los datos se migraron correctamente
-- 2. Comparar conteos entre Oracle y PostgreSQL
-- 3. Probar stored procedures y funciones
-- 4. Verificar rendimiento de consultas
-- 5. Validar integridad referencial
-- 6. Hacer backup de PostgreSQL antes de poner en producción

PRINT 'Migración de Oracle 11g a PostgreSQL completada.';
PRINT 'Recuerda verificar todos los datos antes de poner en producción.';

