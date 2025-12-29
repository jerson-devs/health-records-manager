-- Trigger: TRG_MEDICAL_RECORDS_BEFORE_UPDATE
-- Descripción: Trigger que se ejecuta antes de actualizar un historial médico
-- Funcionalidad:
--   - Actualiza UPDATED_AT con la fecha/hora actual
--   - Valida que la fecha no sea futura si se modifica
--   - Valida que el paciente exista si se modifica
-- Autor: Health Records System
-- Fecha: 2025-01-16

CREATE OR REPLACE TRIGGER TRG_MEDICAL_RECORDS_BEFORE_UPDATE
BEFORE UPDATE ON MEDICAL_RECORDS
FOR EACH ROW
DECLARE
    v_patient_exists NUMBER;
BEGIN
    -- Actualizar UPDATED_AT
    :NEW.UPDATED_AT := SYSTIMESTAMP;
    
    -- Validar fecha si se modificó
    IF :NEW.FECHA != :OLD.FECHA THEN
        IF :NEW.FECHA > SYSDATE THEN
            RAISE_APPLICATION_ERROR(-20305, 'La fecha del historial médico no puede ser futura');
        END IF;
    END IF;
    
    -- Validar paciente si se modificó
    IF :NEW.PATIENT_ID != :OLD.PATIENT_ID THEN
        SELECT COUNT(*) INTO v_patient_exists
        FROM PATIENTS
        WHERE PATIENT_ID = :NEW.PATIENT_ID;
        
        IF v_patient_exists = 0 THEN
            RAISE_APPLICATION_ERROR(-20306, 'El paciente con ID ' || :NEW.PATIENT_ID || ' no existe');
        END IF;
    END IF;
END;
/

