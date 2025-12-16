using System;

namespace HealthRecords.Domain.Models;

/// <summary>
/// Entidad que representa un usuario del sistema para autenticación.
/// Simula estructura compatible con Oracle.
/// </summary>
public class User : BaseEntity
{
    /// <summary>
    /// Nombre de usuario para login
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Correo electrónico del usuario
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Hash de la contraseña (nunca almacenar contraseña en texto plano)
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// Rol del usuario (Admin, Doctor, Nurse, etc.)
    /// </summary>
    public string Role { get; set; } = "User";
}

