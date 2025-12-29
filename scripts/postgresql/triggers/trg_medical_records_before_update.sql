-- Trigger: trg_medical_records_before_update
-- Descripción: Trigger que se ejecuta antes de actualizar un historial médico
-- Funcionalidad:
--   - Actualiza UPDATED_AT con la fecha/hora actual
--   - Valida que la fecha no sea futura si se modifica
--   - Valida que el paciente exista si se modifica
-- Autor: Health Records System
-- Fecha: 2025-01-16

CREATE OR REPLACE FUNCTION trg_medical_records_before_update_func()
RETURNS TRIGGER AS $$
DECLARE
    v_patient_exists INTEGER;
BEGIN
    -- Actualizar UPDATED_AT
    NEW.UPDATED_AT := CURRENT_TIMESTAMP;
    
    -- Validar fecha si se modificó
    IF NEW.FECHA != OLD.FECHA THEN
        IF NEW.FECHA > CURRENT_DATE THEN
            RAISE EXCEPTION 'La fecha del historial médico no puede ser futura';
        END IF;
    END IF;
    
    -- Validar paciente si se modificó
    IF NEW.PATIENT_ID != OLD.PATIENT_ID THEN
        SELECT COUNT(*) INTO v_patient_exists
        FROM PATIENTS
        WHERE PATIENT_ID = NEW.PATIENT_ID;
        
        IF v_patient_exists = 0 THEN
            RAISE EXCEPTION 'El paciente con ID % no existe', NEW.PATIENT_ID;
        END IF;
    END IF;
    
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_medical_records_before_update
BEFORE UPDATE ON MEDICAL_RECORDS
FOR EACH ROW
EXECUTE FUNCTION trg_medical_records_before_update_func();

