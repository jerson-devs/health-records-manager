-- Script de Migración: Oracle 11g a Oracle Moderno (12c+)
-- Health Records Manager
-- Autor: Health Records System
-- Fecha: 2025-01-16
-- Descripción: Script para migrar desde Oracle 11g a Oracle 12c/18c/19c/21c/23c

-- ============================================
-- PREREQUISITOS
-- ============================================
-- 1. Tener acceso a ambas bases de datos (Oracle 11g y Oracle moderno)
-- 2. Tener permisos DBA o suficientes permisos para crear objetos
-- 3. Verificar compatibilidad de versiones
-- 4. Hacer backup completo antes de migrar

-- ============================================
-- 1. VERIFICAR VERSIONES
-- ============================================

-- En Oracle 11g (origen):
SELECT * FROM V$VERSION;

-- En Oracle moderno (destino):
SELECT * FROM V$VERSION;

-- ============================================
-- 2. EXPORTAR ESTRUCTURA Y DATOS
-- ============================================

-- 2.1. Exportar estructura (DDL)
-- Usar expdp o generar scripts DDL desde Oracle SQL Developer

-- 2.2. Exportar datos
-- Usar expdp para exportar datos

-- Ejemplo con expdp:
/*
expdp system/password@oracle11g \
  DIRECTORY=DATA_PUMP_DIR \
  DUMPFILE=healthrecords_export.dmp \
  SCHEMAS=HEALTHRECORDS \
  LOGFILE=export.log
*/

-- ============================================
-- 3. ACTUALIZAR SINTAXIS PL/SQL
-- ============================================

-- 3.1. Packages - Verificar compatibilidad
-- La mayoría de packages PL/SQL de 11g son compatibles con versiones modernas
-- Pero revisar:
-- - Funciones deprecadas
-- - Nuevas características disponibles
-- - Mejoras de performance

-- 3.2. Actualizar PACKAGE_PATIENTS si es necesario
CREATE OR REPLACE PACKAGE PACKAGE_PATIENTS AS
    -- Verificar que la sintaxis sea compatible
    -- Oracle 12c+ soporta mejoras como:
    -- - WITH clause mejorado
    -- - JSON support
    -- - In-Memory Column Store (12c+)
    
    TYPE T_PATIENT_CURSOR IS REF CURSOR;
    
    PROCEDURE GET_BY_ID(
        p_patient_id IN NUMBER,
        p_patient OUT T_PATIENT_CURSOR
    );
    
    PROCEDURE GET_ALL(
        p_patients OUT T_PATIENT_CURSOR
    );
    
    PROCEDURE CREATE_PATIENT(
        p_nombre IN VARCHAR2,
        p_email IN VARCHAR2,
        p_fecha_nacimiento IN DATE,
        p_documento IN VARCHAR2,
        p_patient_id OUT NUMBER
    );
    
    PROCEDURE UPDATE_PATIENT(
        p_patient_id IN NUMBER,
        p_nombre IN VARCHAR2,
        p_email IN VARCHAR2,
        p_fecha_nacimiento IN DATE,
        p_documento IN VARCHAR2,
        p_success OUT NUMBER
    );
    
    PROCEDURE DELETE_PATIENT(
        p_patient_id IN NUMBER,
        p_success OUT NUMBER
    );
    
    FUNCTION EXISTS_BY_EMAIL(p_email IN VARCHAR2) RETURN NUMBER;
    FUNCTION EXISTS_BY_DOCUMENTO(p_documento IN VARCHAR2) RETURN NUMBER;
END PACKAGE_PATIENTS;
/

-- 3.3. Actualizar PACKAGE_MEDICAL_RECORDS
CREATE OR REPLACE PACKAGE PACKAGE_MEDICAL_RECORDS AS
    TYPE T_MEDICAL_RECORD_CURSOR IS REF CURSOR;
    
    PROCEDURE GET_BY_ID(
        p_record_id IN NUMBER,
        p_record OUT T_MEDICAL_RECORD_CURSOR
    );
    
    PROCEDURE GET_BY_PATIENT(
        p_patient_id IN NUMBER,
        p_records OUT T_MEDICAL_RECORD_CURSOR
    );
    
    PROCEDURE GET_ALL(
        p_records OUT T_MEDICAL_RECORD_CURSOR
    );
    
    PROCEDURE CREATE_RECORD(
        p_patient_id IN NUMBER,
        p_fecha IN DATE,
        p_diagnostico IN VARCHAR2,
        p_tratamiento IN VARCHAR2,
        p_medico IN VARCHAR2,
        p_record_id OUT NUMBER
    );
    
    PROCEDURE UPDATE_RECORD(
        p_record_id IN NUMBER,
        p_patient_id IN NUMBER,
        p_fecha IN DATE,
        p_diagnostico IN VARCHAR2,
        p_tratamiento IN VARCHAR2,
        p_medico IN VARCHAR2,
        p_success OUT NUMBER
    );
    
    PROCEDURE DELETE_RECORD(
        p_record_id IN NUMBER,
        p_success OUT NUMBER
    );
END PACKAGE_MEDICAL_RECORDS;
/

-- ============================================
-- 4. IMPORTAR DATOS
-- ============================================

-- 4.1. Importar usando impdp
/*
impdp system/password@oracle-modern \
  DIRECTORY=DATA_PUMP_DIR \
  DUMPFILE=healthrecords_export.dmp \
  SCHEMAS=HEALTHRECORDS \
  REMAP_SCHEMA=HEALTHRECORDS:HEALTHRECORDS \
  LOGFILE=import.log \
  TABLE_EXISTS_ACTION=REPLACE
*/

-- 4.2. Verificar importación
SELECT COUNT(*) AS total_patients FROM PATIENTS;
SELECT COUNT(*) AS total_records FROM MEDICAL_RECORDS;
SELECT COUNT(*) AS total_users FROM USERS;

-- ============================================
-- 5. ACTUALIZAR SECUENCIAS
-- ============================================

-- Asegurar que las secuencias estén en el valor correcto
DECLARE
    v_max_patient_id NUMBER;
    v_max_record_id NUMBER;
    v_max_user_id NUMBER;
BEGIN
    -- Actualizar secuencia de PATIENTS
    SELECT NVL(MAX(PATIENT_ID), 0) INTO v_max_patient_id FROM PATIENTS;
    EXECUTE IMMEDIATE 'ALTER SEQUENCE SEQ_PATIENTS RESTART START WITH ' || (v_max_patient_id + 1);
    
    -- Actualizar secuencia de MEDICAL_RECORDS
    SELECT NVL(MAX(RECORD_ID), 0) INTO v_max_record_id FROM MEDICAL_RECORDS;
    EXECUTE IMMEDIATE 'ALTER SEQUENCE SEQ_MEDICAL_RECORDS RESTART START WITH ' || (v_max_record_id + 1);
    
    -- Actualizar secuencia de USERS
    SELECT NVL(MAX(USER_ID), 0) INTO v_max_user_id FROM USERS;
    EXECUTE IMMEDIATE 'ALTER SEQUENCE SEQ_USERS RESTART START WITH ' || (v_max_user_id + 1);
    
    DBMS_OUTPUT.PUT_LINE('Secuencias actualizadas correctamente');
END;
/

-- ============================================
-- 6. VERIFICAR INTEGRIDAD
-- ============================================

-- 6.1. Verificar conteo de registros
DECLARE
    v_patients_count NUMBER;
    v_records_count NUMBER;
    v_users_count NUMBER;
BEGIN
    SELECT COUNT(*) INTO v_patients_count FROM PATIENTS;
    SELECT COUNT(*) INTO v_records_count FROM MEDICAL_RECORDS;
    SELECT COUNT(*) INTO v_users_count FROM USERS;
    
    DBMS_OUTPUT.PUT_LINE('Pacientes migrados: ' || v_patients_count);
    DBMS_OUTPUT.PUT_LINE('Historiales médicos migrados: ' || v_records_count);
    DBMS_OUTPUT.PUT_LINE('Usuarios migrados: ' || v_users_count);
END;
/

-- 6.2. Verificar integridad referencial
DECLARE
    v_orphan_records NUMBER;
BEGIN
    SELECT COUNT(*) INTO v_orphan_records
    FROM MEDICAL_RECORDS mr
    WHERE NOT EXISTS (
        SELECT 1 FROM PATIENTS p WHERE p.PATIENT_ID = mr.PATIENT_ID
    );
    
    IF v_orphan_records > 0 THEN
        DBMS_OUTPUT.PUT_LINE('ADVERTENCIA: Se encontraron ' || v_orphan_records || ' historiales médicos huérfanos');
    ELSE
        DBMS_OUTPUT.PUT_LINE('Integridad referencial verificada: todos los historiales tienen paciente asociado');
    END IF;
END;
/

-- ============================================
-- 7. APLICAR MEJORAS DE ORACLE MODERNO
-- ============================================

-- 7.1. Oracle 12c+: Identity Columns (opcional)
-- Si quieres usar Identity Columns en lugar de secuencias:
/*
ALTER TABLE PATIENTS MODIFY PATIENT_ID GENERATED ALWAYS AS IDENTITY;
ALTER TABLE MEDICAL_RECORDS MODIFY RECORD_ID GENERATED ALWAYS AS IDENTITY;
ALTER TABLE USERS MODIFY USER_ID GENERATED ALWAYS AS IDENTITY;
*/

-- 7.2. Oracle 12c+: In-Memory Column Store (opcional, requiere licencia)
-- ALTER TABLE PATIENTS INMEMORY;
-- ALTER TABLE MEDICAL_RECORDS INMEMORY;

-- 7.3. Oracle 18c+: JSON Support (si necesitas almacenar JSON)
-- ALTER TABLE PATIENTS ADD (METADATA JSON);

-- ============================================
-- 8. ACTUALIZAR FUNCIONES
-- ============================================

-- Verificar que las funciones sean compatibles
-- Ver: scripts/oracle/functions/

-- ============================================
-- 9. ACTUALIZAR TRIGGERS
-- ============================================

-- Verificar que los triggers sean compatibles
-- Ver: scripts/oracle/triggers/

-- ============================================
-- 10. OPTIMIZACIÓN POST-MIGRACIÓN
-- ============================================

-- 10.1. Recolectar estadísticas
EXEC DBMS_STATS.GATHER_TABLE_STATS('HEALTHRECORDS', 'PATIENTS');
EXEC DBMS_STATS.GATHER_TABLE_STATS('HEALTHRECORDS', 'MEDICAL_RECORDS');
EXEC DBMS_STATS.GATHER_TABLE_STATS('HEALTHRECORDS', 'USERS');

-- 10.2. Rebuild índices (si es necesario)
-- ALTER INDEX IX_PATIENTS_EMAIL REBUILD;
-- ALTER INDEX IX_PATIENTS_DOCUMENTO REBUILD;
-- ALTER INDEX IX_MEDICAL_RECORDS_PATIENT_ID REBUILD;

-- ============================================
-- 11. VERIFICACIÓN FINAL
-- ============================================

-- 11.1. Probar packages
DECLARE
    v_cursor SYS_REFCURSOR;
    v_count NUMBER;
BEGIN
    -- Probar GET_ALL
    PACKAGE_PATIENTS.GET_ALL(v_cursor);
    -- Procesar cursor...
    DBMS_OUTPUT.PUT_LINE('Package PACKAGE_PATIENTS funciona correctamente');
END;
/

-- 11.2. Verificar funciones
SELECT FN_GET_PATIENT_AGE(1) FROM DUAL;
SELECT FN_VALIDATE_EMAIL('test@example.com') FROM DUAL;
SELECT FN_COUNT_RECORDS_BY_PATIENT(1) FROM DUAL;

-- ============================================
-- NOTAS FINALES
-- ============================================

-- 1. Verificar que todos los datos se migraron correctamente
-- 2. Comparar conteos entre Oracle 11g y Oracle moderno
-- 3. Probar todos los packages, procedures y functions
-- 4. Verificar rendimiento de consultas
-- 5. Validar integridad referencial
-- 6. Hacer backup de Oracle moderno antes de poner en producción
-- 7. Actualizar connection strings en la aplicación

PRINT 'Migración de Oracle 11g a Oracle moderno completada.';
PRINT 'Recuerda verificar todos los datos y funcionalidades antes de poner en producción.';

