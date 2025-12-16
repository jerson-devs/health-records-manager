using System;

namespace HealthRecords.Application.Responses.Auth;

/// <summary>
/// DTO para la respuesta de login exitoso
/// </summary>
public class LoginResponse
{
    /// <summary>
    /// Access token JWT para autenticación (corto plazo)
    /// </summary>
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>
    /// Refresh token JWT para renovar el access token (largo plazo)
    /// </summary>
    public string RefreshToken { get; set; } = string.Empty;

    /// <summary>
    /// Fecha de expiración del access token
    /// </summary>
    public DateTime ExpiresAt { get; set; }

    /// <summary>
    /// Fecha de expiración del refresh token
    /// </summary>
    public DateTime RefreshTokenExpiresAt { get; set; }

    /// <summary>
    /// Información del usuario autenticado
    /// </summary>
    public UserInfo User { get; set; } = null!;
}

/// <summary>
/// Información básica del usuario
/// </summary>
public class UserInfo
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}

