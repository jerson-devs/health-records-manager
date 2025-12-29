-- Función: FN_GET_PATIENT_AGE
-- Descripción: Calcula la edad de un paciente en años basándose en su fecha de nacimiento
-- Parámetros:
--   p_patient_id: ID del paciente
-- Retorna: Edad en años (NUMBER)
-- Autor: Health Records System
-- Fecha: 2025-01-16

CREATE OR REPLACE FUNCTION FN_GET_PATIENT_AGE(p_patient_id IN NUMBER)
RETURN NUMBER AS
    v_fecha_nacimiento DATE;
    v_edad NUMBER;
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
    v_edad := TRUNC(MONTHS_BETWEEN(SYSDATE, v_fecha_nacimiento) / 12);
    
    RETURN v_edad;
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        RETURN NULL;
    WHEN OTHERS THEN
        RAISE_APPLICATION_ERROR(-20201, 'Error al calcular edad del paciente: ' || SQLERRM);
END FN_GET_PATIENT_AGE;
/

