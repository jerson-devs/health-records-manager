-- Stored Procedure: sp_get_record_by_id
-- Descripción: Obtiene un historial médico por su ID
-- Parámetros:
--   p_record_id: ID del historial médico
-- Retorna: Cursor con los datos del historial médico
-- Autor: Health Records System
-- Fecha: 2025-01-16

CREATE OR REPLACE FUNCTION sp_get_record_by_id(p_record_id INTEGER)
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
    WHERE mr.RECORD_ID = p_record_id;
END;
$$ LANGUAGE plpgsql;

