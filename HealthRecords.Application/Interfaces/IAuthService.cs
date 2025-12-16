using HealthRecords.Application.Requests.Auth;
using HealthRecords.Application.Responses.Auth;
using System.Threading.Tasks;

namespace HealthRecords.Application.Interfaces;

/// <summary>
/// Interfaz del servicio de autenticación
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Autentica un usuario y genera tokens JWT
    /// </summary>
    /// <param name="request">Datos de login</param>
    /// <returns>Respuesta con tokens JWT y datos del usuario</returns>
    Task<LoginResponse?> LoginAsync(LoginRequest request);

    /// <summary>
    /// Renueva el access token usando un refresh token válido
    /// </summary>
    /// <param name="refreshToken">Refresh token para renovar</param>
    /// <returns>Nuevos tokens JWT</returns>
    Task<LoginResponse?> RefreshTokenAsync(string refreshToken);

    /// <summary>
    /// Cierra la sesión del usuario (invalida el refresh token)
    /// </summary>
    /// <param name="refreshToken">Refresh token a invalidar</param>
    /// <returns>True si se cerró la sesión exitosamente</returns>
    Task<bool> LogoutAsync(string refreshToken);
}

