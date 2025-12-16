using BCrypt.Net;

Console.WriteLine("Generador de Hash BCrypt para contraseñas");
Console.WriteLine("==========================================\n");

string password = "admin123";
string hash = BCrypt.Net.BCrypt.HashPassword(password, workFactor: 11);

Console.WriteLine($"Contraseña: {password}");
Console.WriteLine($"Hash BCrypt: {hash}");
Console.WriteLine("\nPuedes usar este hash en el script SQL para crear el usuario inicial.");



