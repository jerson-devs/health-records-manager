-- Script para agregar la columna UPDATED_AT a la tabla USERS
-- Ejecutar este script en PostgreSQL si la columna no existe

-- Verificar si la columna existe antes de agregarla
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 
        FROM information_schema.columns 
        WHERE table_name = 'USERS' 
        AND column_name = 'UPDATED_AT'
    ) THEN
        ALTER TABLE "USERS" 
        ADD COLUMN "UPDATED_AT" TIMESTAMP NULL;
        
        RAISE NOTICE 'Columna UPDATED_AT agregada exitosamente a la tabla USERS';
    ELSE
        RAISE NOTICE 'La columna UPDATED_AT ya existe en la tabla USERS';
    END IF;
END $$;
