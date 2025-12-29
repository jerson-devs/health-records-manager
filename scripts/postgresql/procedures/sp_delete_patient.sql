-- Stored Procedure: sp_delete_patient
-- Descripción: Elimina un paciente (solo si no tiene historiales médicos)
-- Parámetros:
--   p_patient_id: ID del paciente a eliminar
-- Retorna: Número de filas eliminadas (1 si éxito, 0 si no se encontró)
-- Autor: Health Records System
-- Fecha: 2025-01-16

CREATE OR REPLACE FUNCTION sp_delete_patient(p_patient_id INTEGER)
RETURNS INTEGER AS $$
DECLARE
    v_count INTEGER;
    v_rows_affected INTEGER;
BEGIN
    -- Verificar si el paciente tiene historiales médicos
    SELECT COUNT(*) INTO v_count
    FROM MEDICAL_RECORDS
    WHERE PATIENT_ID = p_patient_id;
    
    IF v_count > 0 THEN
        RAISE EXCEPTION 'No se puede eliminar el paciente porque tiene historiales médicos asociados';
    END IF;
    
    -- Eliminar paciente
    DELETE FROM PATIENTS
    WHERE PATIENT_ID = p_patient_id;
    
    GET DIAGNOSTICS v_rows_affected = ROW_COUNT;
    RETURN v_rows_affected;
EXCEPTION
    WHEN OTHERS THEN
        RAISE EXCEPTION 'Error al eliminar paciente: %', SQLERRM;
END;
$$ LANGUAGE plpgsql;

