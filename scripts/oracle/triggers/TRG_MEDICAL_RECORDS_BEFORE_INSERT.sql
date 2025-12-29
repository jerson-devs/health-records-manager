-- Trigger: TRG_MEDICAL_RECORDS_BEFORE_INSERT
-- Descripción: Trigger que se ejecuta antes de insertar un historial médico
-- Funcionalidad:
--   - Auto-genera el ID si no se proporciona (usando secuencia)
--   - Establece CREATED_AT con la fecha/hora actual si no se proporciona
--   - Valida que la fecha no sea futura
--   - Valida que el paciente exista
-- Autor: Health Records System
-- Fecha: 2025-01-16

CREATE OR REPLACE TRIGGER TRG_MEDICAL_RECORDS_BEFORE_INSERT
BEFORE INSERT ON MEDICAL_RECORDS
FOR EACH ROW
DECLARE
    v_patient_exists NUMBER;
BEGIN
    -- Auto-generar ID si es NULL (usando secuencia)
    IF :NEW.RECORD_ID IS NULL THEN
        SELECT SEQ_MEDICAL_RECORDS.NEXTVAL INTO :NEW.RECORD_ID FROM DUAL;
    END IF;
    
    -- Establecer CREATED_AT si es NULL
    IF :NEW.CREATED_AT IS NULL THEN
        :NEW.CREATED_AT := SYSTIMESTAMP;
    END IF;
    
    -- Validar que la fecha no sea futura
    IF :NEW.FECHA > SYSDATE THEN
        RAISE_APPLICATION_ERROR(-20303, 'La fecha del historial médico no puede ser futura');
    END IF;
    
    -- Validar que el paciente exista
    SELECT COUNT(*) INTO v_patient_exists
    FROM PATIENTS
    WHERE PATIENT_ID = :NEW.PATIENT_ID;
    
    IF v_patient_exists = 0 THEN
        RAISE_APPLICATION_ERROR(-20304, 'El paciente con ID ' || :NEW.PATIENT_ID || ' no existe');
    END IF;
END;
/

