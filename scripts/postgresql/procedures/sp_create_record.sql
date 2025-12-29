-- Stored Procedure: sp_create_record
-- Descripción: Crea un nuevo historial médico
-- Parámetros:
--   p_patient_id: ID del paciente
--   p_fecha: Fecha del historial
--   p_diagnostico: Diagnóstico
--   p_tratamiento: Tratamiento
--   p_medico: Nombre del médico
-- Retorna: ID del historial médico creado
-- Autor: Health Records System
-- Fecha: 2025-01-16

CREATE OR REPLACE FUNCTION sp_create_record(
    p_patient_id INTEGER,
    p_fecha DATE,
    p_diagnostico VARCHAR(500),
    p_tratamiento VARCHAR(1000),
    p_medico VARCHAR(200)
)
RETURNS INTEGER AS $$
DECLARE
    v_record_id INTEGER;
    v_count INTEGER;
BEGIN
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
    
    -- Insertar nuevo historial médico
    INSERT INTO MEDICAL_RECORDS (PATIENT_ID, FECHA, DIAGNOSTICO, TRATAMIENTO, MEDICO, CREATED_AT)
    VALUES (p_patient_id, p_fecha, p_diagnostico, p_tratamiento, p_medico, CURRENT_TIMESTAMP)
    RETURNING RECORD_ID INTO v_record_id;
    
    RETURN v_record_id;
EXCEPTION
    WHEN OTHERS THEN
        RAISE EXCEPTION 'Error al crear historial médico: %', SQLERRM;
END;
$$ LANGUAGE plpgsql;

