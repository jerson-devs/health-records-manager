-- Función: fn_count_records_by_patient
-- Descripción: Cuenta el número de historiales médicos de un paciente
-- Parámetros:
--   p_patient_id: ID del paciente
-- Retorna: Número de historiales médicos (INTEGER)
-- Autor: Health Records System
-- Fecha: 2025-01-16

CREATE OR REPLACE FUNCTION fn_count_records_by_patient(p_patient_id INTEGER)
RETURNS INTEGER AS $$
DECLARE
    v_count INTEGER;
BEGIN
    SELECT COUNT(*) INTO v_count
    FROM MEDICAL_RECORDS
    WHERE PATIENT_ID = p_patient_id;
    
    RETURN COALESCE(v_count, 0);
EXCEPTION
    WHEN OTHERS THEN
        RETURN 0;
END;
$$ LANGUAGE plpgsql;

