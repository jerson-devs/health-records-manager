using System.ComponentModel.DataAnnotations;

namespace HealthRecords.Application.Requests.Auth;

/// <summary>
/// DTO para la solicitud de renovación de token
/// </summary>
public class RefreshTokenRequest
{
    /// <summary>
    /// Refresh token para renovar el access token
    /// </summary>
    [Required(ErrorMessage = "El refresh token es requerido")]
    public string RefreshToken { get; set; } = string.Empty;
}
