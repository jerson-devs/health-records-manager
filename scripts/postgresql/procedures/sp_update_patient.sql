-- Stored Procedure: sp_update_patient
-- Descripción: Actualiza un paciente existente
-- Parámetros:
--   p_patient_id: ID del paciente
--   p_nombre: Nombre del paciente
--   p_email: Email del paciente
--   p_fecha_nacimiento: Fecha de nacimiento
--   p_documento: Número de documento
-- Retorna: Número de filas actualizadas (1 si éxito, 0 si no se encontró)
-- Autor: Health Records System
-- Fecha: 2025-01-16

CREATE OR REPLACE FUNCTION sp_update_patient(
    p_patient_id INTEGER,
    p_nombre VARCHAR(200),
    p_email VARCHAR(100),
    p_fecha_nacimiento DATE,
    p_documento VARCHAR(50)
)
RETURNS INTEGER AS $$
DECLARE
    v_count INTEGER;
    v_rows_affected INTEGER;
BEGIN
    -- Validar que el paciente exista
    SELECT COUNT(*) INTO v_count
    FROM PATIENTS
    WHERE PATIENT_ID = p_patient_id;
    
    IF v_count = 0 THEN
        RAISE EXCEPTION 'Paciente no encontrado con ID: %', p_patient_id;
    END IF;
    
    -- Validar que el email no esté en uso por otro paciente
    SELECT COUNT(*) INTO v_count
    FROM PATIENTS
    WHERE EMAIL = p_email AND PATIENT_ID != p_patient_id;
    
    IF v_count > 0 THEN
        RAISE EXCEPTION 'El email ya está en uso por otro paciente';
    END IF;
    
    -- Validar que el documento no esté en uso por otro paciente
    SELECT COUNT(*) INTO v_count
    FROM PATIENTS
    WHERE DOCUMENTO = p_documento AND PATIENT_ID != p_patient_id;
    
    IF v_count > 0 THEN
        RAISE EXCEPTION 'El documento ya está en uso por otro paciente';
    END IF;
    
    -- Actualizar paciente
    UPDATE PATIENTS
    SET NOMBRE = p_nombre,
        EMAIL = p_email,
        FECHA_NACIMIENTO = p_fecha_nacimiento,
        DOCUMENTO = p_documento,
        UPDATED_AT = CURRENT_TIMESTAMP
    WHERE PATIENT_ID = p_patient_id;
    
    GET DIAGNOSTICS v_rows_affected = ROW_COUNT;
    RETURN v_rows_affected;
EXCEPTION
    WHEN OTHERS THEN
        RAISE EXCEPTION 'Error al actualizar paciente: %', SQLERRM;
END;
$$ LANGUAGE plpgsql;

