-- Stored Procedures para Health Records Manager
-- Simulan packages PL/SQL de Oracle

-- ============================================
-- Stored Procedure: Obtener Paciente por ID
-- Equivalente a: PACKAGE_PATIENTS.GET_BY_ID
-- ============================================
CREATE OR REPLACE PROCEDURE sp_GetPatientById
    @PatientId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        PATIENT_ID AS Id,
        NOMBRE AS Nombre,
        EMAIL AS Email,
        FECHA_NACIMIENTO AS FechaNacimiento,
        DOCUMENTO AS Documento,
        CREATED_AT AS CreatedAt,
        UPDATED_AT AS UpdatedAt
    FROM PATIENTS
    WHERE PATIENT_ID = @PatientId;
END;
GO

-- ============================================
-- Stored Procedure: Obtener Historiales por Paciente
-- Equivalente a: PACKAGE_RECORDS.GET_BY_PATIENT
-- ============================================
CREATE OR REPLACE PROCEDURE sp_GetRecordsByPatientId
    @PatientId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        r.RECORD_ID AS Id,
        r.PATIENT_ID AS PatientId,
        r.FECHA AS Fecha,
        r.DIAGNOSTICO AS Diagnostico,
        r.TRATAMIENTO AS Tratamiento,
        r.MEDICO AS Medico,
        r.CREATED_AT AS CreatedAt,
        r.UPDATED_AT AS UpdatedAt,
        p.NOMBRE AS PatientName
    FROM MEDICAL_RECORDS r
    INNER JOIN PATIENTS p ON r.PATIENT_ID = p.PATIENT_ID
    WHERE r.PATIENT_ID = @PatientId
    ORDER BY r.FECHA DESC;
END;
GO

-- ============================================
-- Stored Procedure: Crear Historial Médico
-- Equivalente a: PACKAGE_RECORDS.CREATE
-- ============================================
CREATE OR REPLACE PROCEDURE sp_CreateMedicalRecord
    @PatientId INT,
    @Fecha DATETIME,
    @Diagnostico NVARCHAR(500),
    @Tratamiento NVARCHAR(1000),
    @Medico NVARCHAR(200),
    @RecordId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO MEDICAL_RECORDS (
        PATIENT_ID,
        FECHA,
        DIAGNOSTICO,
        TRATAMIENTO,
        MEDICO,
        CREATED_AT
    )
    VALUES (
        @PatientId,
        @Fecha,
        @Diagnostico,
        @Tratamiento,
        @Medico,
        GETUTCDATE()
    );
    
    SET @RecordId = SCOPE_IDENTITY();
END;
GO

-- ============================================
-- Stored Procedure: Actualizar Historial Médico
-- Equivalente a: PACKAGE_RECORDS.UPDATE
-- ============================================
CREATE OR REPLACE PROCEDURE sp_UpdateMedicalRecord
    @RecordId INT,
    @Fecha DATETIME,
    @Diagnostico NVARCHAR(500),
    @Tratamiento NVARCHAR(1000),
    @Medico NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE MEDICAL_RECORDS
    SET 
        FECHA = @Fecha,
        DIAGNOSTICO = @Diagnostico,
        TRATAMIENTO = @Tratamiento,
        MEDICO = @Medico,
        UPDATED_AT = GETUTCDATE()
    WHERE RECORD_ID = @RecordId;
END;
GO

-- ============================================
-- Función: Verificar si existe paciente por email
-- Equivalente a: PACKAGE_PATIENTS.EXISTS_BY_EMAIL
-- ============================================
CREATE OR REPLACE FUNCTION fn_PatientExistsByEmail
    @Email NVARCHAR(100)
RETURNS BIT
AS
BEGIN
    DECLARE @Exists BIT = 0;
    
    IF EXISTS (SELECT 1 FROM PATIENTS WHERE EMAIL = @Email)
        SET @Exists = 1;
    
    RETURN @Exists;
END;
GO

-- ============================================
-- Función: Verificar si existe paciente por documento
-- Equivalente a: PACKAGE_PATIENTS.EXISTS_BY_DOCUMENTO
-- ============================================
CREATE OR REPLACE FUNCTION fn_PatientExistsByDocumento
    @Documento NVARCHAR(50)
RETURNS BIT
AS
BEGIN
    DECLARE @Exists BIT = 0;
    
    IF EXISTS (SELECT 1 FROM PATIENTS WHERE DOCUMENTO = @Documento)
        SET @Exists = 1;
    
    RETURN @Exists;
END;
GO

-- Nota: Para PostgreSQL, usar la sintaxis equivalente:
-- CREATE OR REPLACE FUNCTION sp_GetPatientById(patient_id INTEGER)
-- RETURNS TABLE(...) AS $$
-- BEGIN
--     RETURN QUERY SELECT ... FROM PATIENTS WHERE PATIENT_ID = patient_id;
-- END;
-- $$ LANGUAGE plpgsql;




