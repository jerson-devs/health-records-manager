-- Stored Procedure: sp_create_patient
-- Descripción: Crea un nuevo paciente
-- Parámetros:
--   p_nombre: Nombre del paciente
--   p_email: Email del paciente
--   p_fecha_nacimiento: Fecha de nacimiento
--   p_documento: Número de documento
-- Retorna: ID del paciente creado
-- Autor: Health Records System
-- Fecha: 2025-01-16

CREATE OR REPLACE FUNCTION sp_create_patient(
    p_nombre VARCHAR(200),
    p_email VARCHAR(100),
    p_fecha_nacimiento DATE,
    p_documento VARCHAR(50)
)
RETURNS INTEGER AS $$
DECLARE
    v_patient_id INTEGER;
    v_count INTEGER;
BEGIN
    -- Validar que el email no exista
    SELECT COUNT(*) INTO v_count
    FROM PATIENTS
    WHERE EMAIL = p_email;
    
    IF v_count > 0 THEN
        RAISE EXCEPTION 'Ya existe un paciente con el email: %', p_email;
    END IF;
    
    -- Validar que el documento no exista
    SELECT COUNT(*) INTO v_count
    FROM PATIENTS
    WHERE DOCUMENTO = p_documento;
    
    IF v_count > 0 THEN
        RAISE EXCEPTION 'Ya existe un paciente con el documento: %', p_documento;
    END IF;
    
    -- Insertar nuevo paciente
    INSERT INTO PATIENTS (NOMBRE, EMAIL, FECHA_NACIMIENTO, DOCUMENTO, CREATED_AT)
    VALUES (p_nombre, p_email, p_fecha_nacimiento, p_documento, CURRENT_TIMESTAMP)
    RETURNING PATIENT_ID INTO v_patient_id;
    
    RETURN v_patient_id;
EXCEPTION
    WHEN OTHERS THEN
        RAISE EXCEPTION 'Error al crear paciente: %', SQLERRM;
END;
$$ LANGUAGE plpgsql;

