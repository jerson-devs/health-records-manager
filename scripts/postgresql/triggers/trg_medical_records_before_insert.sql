-- Trigger: trg_medical_records_before_insert
-- Descripción: Trigger que se ejecuta antes de insertar un historial médico
-- Funcionalidad:
--   - Establece CREATED_AT con la fecha/hora actual si no se proporciona
--   - Valida que la fecha no sea futura
--   - Valida que el paciente exista
-- Autor: Health Records System
-- Fecha: 2025-01-16

CREATE OR REPLACE FUNCTION trg_medical_records_before_insert_func()
RETURNS TRIGGER AS $$
DECLARE
    v_patient_exists INTEGER;
BEGIN
    -- Establecer CREATED_AT si es NULL
    IF NEW.CREATED_AT IS NULL THEN
        NEW.CREATED_AT := CURRENT_TIMESTAMP;
    END IF;
    
    -- Validar que la fecha no sea futura
    IF NEW.FECHA > CURRENT_DATE THEN
        RAISE EXCEPTION 'La fecha del historial médico no puede ser futura';
    END IF;
    
    -- Validar que el paciente exista
    SELECT COUNT(*) INTO v_patient_exists
    FROM PATIENTS
    WHERE PATIENT_ID = NEW.PATIENT_ID;
    
    IF v_patient_exists = 0 THEN
        RAISE EXCEPTION 'El paciente con ID % no existe', NEW.PATIENT_ID;
    END IF;
    
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_medical_records_before_insert
BEFORE INSERT ON MEDICAL_RECORDS
FOR EACH ROW
EXECUTE FUNCTION trg_medical_records_before_insert_func();

