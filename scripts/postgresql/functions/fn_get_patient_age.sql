-- Función: fn_get_patient_age
-- Descripción: Calcula la edad de un paciente en años basándose en su fecha de nacimiento
-- Parámetros:
--   p_patient_id: ID del paciente
-- Retorna: Edad en años (INTEGER)
-- Autor: Health Records System
-- Fecha: 2025-01-16

CREATE OR REPLACE FUNCTION fn_get_patient_age(p_patient_id INTEGER)
RETURNS INTEGER AS $$
DECLARE
    v_fecha_nacimiento DATE;
    v_edad INTEGER;
BEGIN
    -- Obtener fecha de nacimiento del paciente
    SELECT FECHA_NACIMIENTO INTO v_fecha_nacimiento
    FROM PATIENTS
    WHERE PATIENT_ID = p_patient_id;
    
    -- Si no se encuentra el paciente, retornar NULL
    IF v_fecha_nacimiento IS NULL THEN
        RETURN NULL;
    END IF;
    
    -- Calcular edad en años
    v_edad := EXTRACT(YEAR FROM AGE(CURRENT_DATE, v_fecha_nacimiento));
    
    RETURN v_edad;
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        RETURN NULL;
    WHEN OTHERS THEN
        RAISE EXCEPTION 'Error al calcular edad del paciente: %', SQLERRM;
END;
$$ LANGUAGE plpgsql;

