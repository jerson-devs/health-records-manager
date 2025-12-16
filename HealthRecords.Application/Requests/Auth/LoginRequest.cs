using System.ComponentModel.DataAnnotations;

namespace HealthRecords.Application.Requests.Auth;

/// <summary>
/// DTO para la solicitud de login
/// </summary>
public class LoginRequest
{
    /// <summary>
    /// Nombre de usuario o email
    /// </summary>
    [Required(ErrorMessage = "El nombre de usuario o email es requerido")]
    [StringLength(100, ErrorMessage = "El nombre de usuario no puede exceder 100 caracteres")]
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Contraseña del usuario
    /// </summary>
    [Required(ErrorMessage = "La contraseña es requerida")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 100 caracteres")]
    public string Password { get; set; } = string.Empty;
}

