-- Función: FN_COUNT_RECORDS_BY_PATIENT
-- Descripción: Cuenta el número de historiales médicos de un paciente
-- Parámetros:
--   p_patient_id: ID del paciente
-- Retorna: Número de historiales médicos (NUMBER)
-- Autor: Health Records System
-- Fecha: 2025-01-16

CREATE OR REPLACE FUNCTION FN_COUNT_RECORDS_BY_PATIENT(p_patient_id IN NUMBER)
RETURN NUMBER AS
    v_count NUMBER;
BEGIN
    SELECT COUNT(*) INTO v_count
    FROM MEDICAL_RECORDS
    WHERE PATIENT_ID = p_patient_id;
    
    RETURN NVL(v_count, 0);
EXCEPTION
    WHEN OTHERS THEN
        RETURN 0;
END FN_COUNT_RECORDS_BY_PATIENT;
/

