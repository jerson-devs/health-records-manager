-- Trigger: trg_patients_before_insert
-- Descripción: Trigger que se ejecuta antes de insertar un paciente
-- Funcionalidad:
--   - Establece CREATED_AT con la fecha/hora actual si no se proporciona
--   - Valida el formato del email usando función
-- Autor: Health Records System
-- Fecha: 2025-01-16

CREATE OR REPLACE FUNCTION trg_patients_before_insert_func()
RETURNS TRIGGER AS $$
BEGIN
    -- Establecer CREATED_AT si es NULL
    IF NEW.CREATED_AT IS NULL THEN
        NEW.CREATED_AT := CURRENT_TIMESTAMP;
    END IF;
    
    -- Validar email usando función
    IF NOT fn_validate_email(NEW.EMAIL) THEN
        RAISE EXCEPTION 'Formato de email inválido: %', NEW.EMAIL;
    END IF;
    
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_patients_before_insert
BEFORE INSERT ON PATIENTS
FOR EACH ROW
EXECUTE FUNCTION trg_patients_before_insert_func();

