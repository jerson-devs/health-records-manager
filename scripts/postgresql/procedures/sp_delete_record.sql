-- Stored Procedure: sp_delete_record
-- Descripción: Elimina un historial médico
-- Parámetros:
--   p_record_id: ID del historial médico a eliminar
-- Retorna: Número de filas eliminadas (1 si éxito, 0 si no se encontró)
-- Autor: Health Records System
-- Fecha: 2025-01-16

CREATE OR REPLACE FUNCTION sp_delete_record(p_record_id INTEGER)
RETURNS INTEGER AS $$
DECLARE
    v_rows_affected INTEGER;
BEGIN
    DELETE FROM MEDICAL_RECORDS
    WHERE RECORD_ID = p_record_id;
    
    GET DIAGNOSTICS v_rows_affected = ROW_COUNT;
    RETURN v_rows_affected;
EXCEPTION
    WHEN OTHERS THEN
        RAISE EXCEPTION 'Error al eliminar historial médico: %', SQLERRM;
END;
$$ LANGUAGE plpgsql;

