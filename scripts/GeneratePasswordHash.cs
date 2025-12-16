// Script temporal para generar hash de contraseña BCrypt
// Ejecutar con: dotnet script GeneratePasswordHash.cs
// O copiar el código en un proyecto C# y ejecutarlo

using BCrypt.Net;

string password = "Admin123!";
string hash = BCrypt.Net.BCrypt.HashPassword(password, workFactor: 11);
Console.WriteLine($"Password: {password}");
Console.WriteLine($"Hash: {hash}");

// Hash generado para "Admin123!" (workFactor 11):
// $2a$11$K8Z5Y5Y5Y5Y5Y5Y5Y5Y5Ye5Y5Y5Y5Y5Y5Y5Y5Y5Y5Y5Y5Y5Y5Y5Y5Y



