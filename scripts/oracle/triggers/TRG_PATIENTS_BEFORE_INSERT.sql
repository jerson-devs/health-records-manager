-- Trigger: TRG_PATIENTS_BEFORE_INSERT
-- Descripción: Trigger que se ejecuta antes de insertar un paciente
-- Funcionalidad:
--   - Auto-genera el ID si no se proporciona (usando secuencia)
--   - Establece CREATED_AT con la fecha/hora actual si no se proporciona
-- Autor: Health Records System
-- Fecha: 2025-01-16

CREATE OR REPLACE TRIGGER TRG_PATIENTS_BEFORE_INSERT
BEFORE INSERT ON PATIENTS
FOR EACH ROW
BEGIN
    -- Auto-generar ID si es NULL (usando secuencia)
    IF :NEW.PATIENT_ID IS NULL THEN
        SELECT SEQ_PATIENTS.NEXTVAL INTO :NEW.PATIENT_ID FROM DUAL;
    END IF;
    
    -- Establecer CREATED_AT si es NULL
    IF :NEW.CREATED_AT IS NULL THEN
        :NEW.CREATED_AT := SYSTIMESTAMP;
    END IF;
    
    -- Validar email usando función
    IF FN_VALIDATE_EMAIL(:NEW.EMAIL) = 0 THEN
        RAISE_APPLICATION_ERROR(-20301, 'Formato de email inválido: ' || :NEW.EMAIL);
    END IF;
END;
/

