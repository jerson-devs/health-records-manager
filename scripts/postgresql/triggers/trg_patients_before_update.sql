-- Trigger: trg_patients_before_update
-- Descripción: Trigger que se ejecuta antes de actualizar un paciente
-- Funcionalidad:
--   - Actualiza UPDATED_AT con la fecha/hora actual
--   - Valida el formato del email si se modifica
-- Autor: Health Records System
-- Fecha: 2025-01-16

CREATE OR REPLACE FUNCTION trg_patients_before_update_func()
RETURNS TRIGGER AS $$
BEGIN
    -- Actualizar UPDATED_AT
    NEW.UPDATED_AT := CURRENT_TIMESTAMP;
    
    -- Validar email si se modificó
    IF NEW.EMAIL != OLD.EMAIL THEN
        IF NOT fn_validate_email(NEW.EMAIL) THEN
            RAISE EXCEPTION 'Formato de email inválido: %', NEW.EMAIL;
        END IF;
    END IF;
    
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_patients_before_update
BEFORE UPDATE ON PATIENTS
FOR EACH ROW
EXECUTE FUNCTION trg_patients_before_update_func();

