-- Stored Procedure: sp_get_all_patients
-- Descripción: Obtiene todos los pacientes ordenados por nombre
-- Retorna: Cursor con todos los pacientes
-- Autor: Health Records System
-- Fecha: 2025-01-16

CREATE OR REPLACE FUNCTION sp_get_all_patients()
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
    ORDER BY p.NOMBRE;
END;
$$ LANGUAGE plpgsql;

