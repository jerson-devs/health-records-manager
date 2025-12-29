-- Trigger: TRG_PATIENTS_BEFORE_UPDATE
-- Descripción: Trigger que se ejecuta antes de actualizar un paciente
-- Funcionalidad:
--   - Actualiza UPDATED_AT con la fecha/hora actual
--   - Valida el formato del email si se modifica
-- Autor: Health Records System
-- Fecha: 2025-01-16

CREATE OR REPLACE TRIGGER TRG_PATIENTS_BEFORE_UPDATE
BEFORE UPDATE ON PATIENTS
FOR EACH ROW
BEGIN
    -- Actualizar UPDATED_AT
    :NEW.UPDATED_AT := SYSTIMESTAMP;
    
    -- Validar email si se modificó
    IF :NEW.EMAIL != :OLD.EMAIL THEN
        IF FN_VALIDATE_EMAIL(:NEW.EMAIL) = 0 THEN
            RAISE_APPLICATION_ERROR(-20302, 'Formato de email inválido: ' || :NEW.EMAIL);
        END IF;
    END IF;
END;
/

