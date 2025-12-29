-- Función: fn_validate_email
-- Descripción: Valida el formato de un email usando expresión regular
-- Parámetros:
--   p_email: Email a validar
-- Retorna: TRUE si es válido, FALSE si no es válido (BOOLEAN)
-- Autor: Health Records System
-- Fecha: 2025-01-16

CREATE OR REPLACE FUNCTION fn_validate_email(p_email VARCHAR(100))
RETURNS BOOLEAN AS $$
BEGIN
    -- Validar que el email no sea nulo o vacío
    IF p_email IS NULL OR TRIM(p_email) = '' THEN
        RETURN FALSE;
    END IF;
    
    -- Validar formato usando expresión regular
    -- Patrón: al menos un carácter antes de @, al menos un carácter después de @, y al menos 2 caracteres después del punto
    IF p_email ~* '^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$' THEN
        RETURN TRUE;
    ELSE
        RETURN FALSE;
    END IF;
EXCEPTION
    WHEN OTHERS THEN
        RETURN FALSE;
END;
$$ LANGUAGE plpgsql;

