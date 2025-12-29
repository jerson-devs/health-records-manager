-- Stored Procedure: sp_update_record
-- Descripción: Actualiza un historial médico existente
-- Parámetros:
--   p_record_id: ID del historial médico
--   p_patient_id: ID del paciente
--   p_fecha: Fecha del historial
--   p_diagnostico: Diagnóstico
--   p_tratamiento: Tratamiento
--   p_medico: Nombre del médico
-- Retorna: Número de filas actualizadas (1 si éxito, 0 si no se encontró)
-- Autor: Health Records System
-- Fecha: 2025-01-16

CREATE OR REPLACE FUNCTION sp_update_record(
    p_record_id INTEGER,
    p_patient_id INTEGER,
    p_fecha DATE,
    p_diagnostico VARCHAR(500),
    p_tratamiento VARCHAR(1000),
    p_medico VARCHAR(200)
)
RETURNS INTEGER AS $$
DECLARE
    v_count INTEGER;
    v_rows_affected INTEGER;
BEGIN
    -- Validar que el historial médico exista
    SELECT COUNT(*) INTO v_count
    FROM MEDICAL_RECORDS
    WHERE RECORD_ID = p_record_id;
    
    IF v_count = 0 THEN
        RAISE EXCEPTION 'Historial médico no encontrado con ID: %', p_record_id;
    END IF;
    
    -- Validar que el paciente exista
    SELECT COUNT(*) INTO v_count
    FROM PATIENTS
    WHERE PATIENT_ID = p_patient_id;
    
    IF v_count = 0 THEN
        RAISE EXCEPTION 'Paciente no encontrado con ID: %', p_patient_id;
    END IF;
    
    -- Validar que la fecha no sea futura
    IF p_fecha > CURRENT_DATE THEN
        RAISE EXCEPTION 'La fecha del historial médico no puede ser futura';
    END IF;
    
    -- Actualizar historial médico
    UPDATE MEDICAL_RECORDS
    SET PATIENT_ID = p_patient_id,
        FECHA = p_fecha,
        DIAGNOSTICO = p_diagnostico,
        TRATAMIENTO = p_tratamiento,
        MEDICO = p_medico,
        UPDATED_AT = CURRENT_TIMESTAMP
    WHERE RECORD_ID = p_record_id;
    
    GET DIAGNOSTICS v_rows_affected = ROW_COUNT;
    RETURN v_rows_affected;
EXCEPTION
    WHEN OTHERS THEN
        RAISE EXCEPTION 'Error al actualizar historial médico: %', SQLERRM;
END;
$$ LANGUAGE plpgsql;

