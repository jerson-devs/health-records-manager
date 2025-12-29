-- Stored Procedure: sp_get_records_by_patient
-- Descripción: Obtiene todos los historiales médicos de un paciente
-- Parámetros:
--   p_patient_id: ID del paciente
-- Retorna: Cursor con los historiales médicos del paciente
-- Autor: Health Records System
-- Fecha: 2025-01-16

CREATE OR REPLACE FUNCTION sp_get_records_by_patient(p_patient_id INTEGER)
RETURNS TABLE (
    record_id INTEGER,
    patient_id INTEGER,
    fecha DATE,
    diagnostico VARCHAR(500),
    tratamiento VARCHAR(1000),
    medico VARCHAR(200),
    created_at TIMESTAMP,
    updated_at TIMESTAMP
) AS $$
BEGIN
    RETURN QUERY
    SELECT 
        mr.RECORD_ID,
        mr.PATIENT_ID,
        mr.FECHA,
        mr.DIAGNOSTICO,
        mr.TRATAMIENTO,
        mr.MEDICO,
        mr.CREATED_AT,
        mr.UPDATED_AT
    FROM MEDICAL_RECORDS mr
    WHERE mr.PATIENT_ID = p_patient_id
    ORDER BY mr.FECHA DESC;
END;
$$ LANGUAGE plpgsql;

