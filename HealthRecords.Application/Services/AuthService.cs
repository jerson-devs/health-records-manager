using BCrypt.Net;
using HealthRecords.Application.Constants;
using HealthRecords.Application.Interfaces;
using HealthRecords.Application.Requests.Auth;
using HealthRecords.Application.Responses.Auth;
using HealthRecords.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace HealthRecords.Application.Services;

/// <summary>
/// Servicio de autenticación que maneja login, refresh tokens y logout
/// </summary>
public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        IUserRepository userRepository,
        IJwtTokenGenerator jwtTokenGenerator,
        ILogger<AuthService> logger)
    {
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        _logger.LogInformation("Event: {EventId} - Intento de login. Username: {Username}", LogEvents.LoginAttempt, request.Username);
        
        // Buscar usuario por username o email
        var user = await _userRepository.GetByUsernameAsync(request.Username) 
            ?? await _userRepository.GetByEmailAsync(request.Username);

        if (user == null)
        {
            _logger.LogWarning("Event: {EventId} - Intento de login con usuario inexistente. Username: {Username}", LogEvents.LoginFailedInvalidUser, request.Username);
            return null;
        }

        // Verificar contraseña
        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            _logger.LogWarning("Event: {EventId} - Intento de login con contraseña incorrecta. Username: {Username}, UserId: {UserId}", LogEvents.LoginFailedInvalidPassword, request.Username, user.Id);
            return null;
        }

        // Generar tokens JWT
        var accessToken = _jwtTokenGenerator.GenerateAccessToken(user);
        var refreshToken = _jwtTokenGenerator.GenerateRefreshToken(user);
        var accessTokenExpiration = _jwtTokenGenerator.GetAccessTokenExpirationDate();
        var refreshTokenExpiration = _jwtTokenGenerator.GetRefreshTokenExpirationDate();

        var response = new LoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = accessTokenExpiration,
            RefreshTokenExpiresAt = refreshTokenExpiration,
            User = new UserInfo
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role
            }
        };

        _logger.LogInformation("Event: {EventId} - Login exitoso. Username: {Username}, UserId: {UserId}, Role: {Role}", 
            LogEvents.LoginSuccess, user.Username, user.Id, user.Role);
        return response;
    }

    /// <inheritdoc/>
    public async Task<LoginResponse?> RefreshTokenAsync(string refreshToken)
    {
        _logger.LogInformation("Event: {EventId} - Intento de renovación de token", LogEvents.RefreshTokenAttempt);

        // Validar el refresh token
        if (!_jwtTokenGenerator.ValidateToken(refreshToken))
        {
            _logger.LogWarning("Event: {EventId} - Refresh token inválido o expirado", LogEvents.RefreshTokenFailed);
            return null;
        }

        // Decodificar el token para obtener el user ID
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(refreshToken);

        // Verificar que sea un refresh token
        var tokenType = jwtToken.Claims.FirstOrDefault(c => c.Type == "token_type")?.Value;
        if (tokenType != "refresh")
        {
            _logger.LogWarning("Event: {EventId} - Token proporcionado no es un refresh token", LogEvents.RefreshTokenFailed);
            return null;
        }

        // Obtener el user ID del token
        var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
        {
            _logger.LogWarning("Event: {EventId} - No se pudo obtener el ID de usuario del refresh token", LogEvents.RefreshTokenFailed);
            return null;
        }

        // Buscar el usuario
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            _logger.LogWarning("Event: {EventId} - Usuario no encontrado para refresh token. UserId: {UserId}", LogEvents.RefreshTokenFailed, userId);
            return null;
        }

        // Generar nuevos tokens
        var accessToken = _jwtTokenGenerator.GenerateAccessToken(user);
        var newRefreshToken = _jwtTokenGenerator.GenerateRefreshToken(user);
        var accessTokenExpiration = _jwtTokenGenerator.GetAccessTokenExpirationDate();
        var refreshTokenExpiration = _jwtTokenGenerator.GetRefreshTokenExpirationDate();

        var response = new LoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken,
            ExpiresAt = accessTokenExpiration,
            RefreshTokenExpiresAt = refreshTokenExpiration,
            User = new UserInfo
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role
            }
        };

        _logger.LogInformation("Event: {EventId} - Token renovado exitosamente. UserId: {UserId}", LogEvents.RefreshTokenSuccess, userId);
        return response;
    }

    /// <inheritdoc/>
    public async Task<bool> LogoutAsync(string refreshToken)
    {
        _logger.LogInformation("Event: {EventId} - Intento de logout", LogEvents.LogoutAttempt);

        // Validar el refresh token
        if (!_jwtTokenGenerator.ValidateToken(refreshToken))
        {
            _logger.LogWarning("Event: {EventId} - Refresh token inválido para logout", LogEvents.LogoutFailed);
            return false;
        }

        // En un sistema stateless con JWT, el logout se maneja principalmente en el cliente
        // El token se invalida simplemente no renovándolo
        // En un sistema más robusto, podrías mantener una blacklist de tokens invalidados
        
        _logger.LogInformation("Event: {EventId} - Logout exitoso", LogEvents.LogoutSuccess);
        return true;
    }
}

