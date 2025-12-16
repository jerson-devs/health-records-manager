-- Script para crear un usuario inicial en la base de datos
-- La contraseña es "Admin123!" y está hasheada con BCrypt

-- Usuario administrador por defecto
-- Username: admin
-- Password: Admin123!
-- Email: admin@healthrecords.com

INSERT INTO USERS (USERNAME, EMAIL, PASSWORD_HASH, ROLE, CREATED_AT)
VALUES (
    'admin',
    'admin@healthrecords.com',
    '$2a$11$6h25Ry/IE.M59G7Ubb.OHOpFYlIXrxGMtA3QsFT9THPfknnhOreB.', -- Hash BCrypt de "Admin123!"
    'Admin',
    GETUTCDATE()
);

-- Si necesitas generar un nuevo hash de contraseña, puedes:
-- 1. Ejecutar el script: scripts/GenerateHash/GenerateHash.csproj
--    Comando: dotnet run --project scripts/GenerateHash/GenerateHash.csproj
-- 2. O usar un generador online de BCrypt
-- 3. O en C#: BCrypt.Net.BCrypt.HashPassword("tu_contraseña")

