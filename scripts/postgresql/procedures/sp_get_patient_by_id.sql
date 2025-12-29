-- Stored Procedure: sp_get_patient_by_id
-- Descripción: Obtiene un paciente por su ID
-- Parámetros:
--   p_patient_id: ID del paciente
-- Retorna: Cursor con los datos del paciente
-- Autor: Health Records System
-- Fecha: 2025-01-16

CREATE OR REPLACE FUNCTION sp_get_patient_by_id(p_patient_id INTEGER)
RETURNS TABLE (
    patient_id INTEGER,
    nombre VARCHAR(200),
    email VARCHAR(100),
    fecha_nacimiento DATE,
    documento VARCHAR(50),
    created_at TIMESTAMP,
    updated_at TIMESTAMP
) AS $$
BEGIN
    RETURN QUERY
    SELECT 
        p.PATIENT_ID,
        p.NOMBRE,
        p.EMAIL,
        p.FECHA_NACIMIENTO,
        p.DOCUMENTO,
        p.CREATED_AT,
        p.UPDATED_AT
    FROM PATIENTS p
    WHERE p.PATIENT_ID = p_patient_id;
END;
$$ LANGUAGE plpgsql;

