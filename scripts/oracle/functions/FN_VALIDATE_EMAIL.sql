-- Función: FN_VALIDATE_EMAIL
-- Descripción: Valida el formato de un email usando expresión regular
-- Parámetros:
--   p_email: Email a validar
-- Retorna: 1 si es válido, 0 si no es válido (NUMBER)
-- Autor: Health Records System
-- Fecha: 2025-01-16

CREATE OR REPLACE FUNCTION FN_VALIDATE_EMAIL(p_email IN VARCHAR2)
RETURN NUMBER AS
    v_pattern VARCHAR2(200) := '^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$';
BEGIN
    -- Validar que el email no sea nulo o vacío
    IF p_email IS NULL OR LENGTH(TRIM(p_email)) = 0 THEN
        RETURN 0;
    END IF;
    
    -- Validar formato básico usando REGEXP_LIKE (Oracle 10g+)
    IF REGEXP_LIKE(p_email, v_pattern) THEN
        RETURN 1;
    ELSE
        RETURN 0;
    END IF;
EXCEPTION
    WHEN OTHERS THEN
        RETURN 0;
END FN_VALIDATE_EMAIL;
/

