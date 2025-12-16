-- Script para crear un usuario inicial en PostgreSQL
-- La contraseña es "admin123" y está hasheada con BCrypt

-- Usuario administrador por defecto
-- Username: admin
-- Password: admin123
-- Email: admin@healthrecords.com

INSERT INTO "USERS" ("USERNAME", "EMAIL", "PASSWORD_HASH", "ROLE", "CREATED_AT")
VALUES (
    'admin',
    'admin@healthrecords.com',
    '$2a$11$.wpEJGn0P1sNO.ObYul4/utob2f1s1sxzFllxdjZbYrHOm7514PlK', -- Hash BCrypt de "admin123"
    'Admin',
    NOW()
);

-- Si necesitas generar un nuevo hash de contraseña, puedes:
-- 1. Ejecutar el script: scripts/GenerateHash/GenerateHash.csproj
--    Comando: dotnet run --project scripts/GenerateHash/GenerateHash.csproj
-- 2. O usar un generador online de BCrypt
-- 3. O en C#: BCrypt.Net.BCrypt.HashPassword("tu_contraseña")
