using HealthRecords.Domain.Models;
using System;

namespace HealthRecords.Application.Interfaces;

/// <summary>
/// Interfaz para generar tokens JWT
/// </summary>
public interface IJwtTokenGenerator
{
    /// <summary>
    /// Genera un token JWT de acceso para el usuario
    /// </summary>
    /// <param name="user">Usuario para el cual generar el token</param>
    /// <returns>Token JWT como string</returns>
    string GenerateAccessToken(User user);

    /// <summary>
    /// Genera un refresh token para el usuario
    /// </summary>
    /// <param name="user">Usuario para el cual generar el refresh token</param>
    /// <returns>Refresh token como string</returns>
    string GenerateRefreshToken(User user);

    /// <summary>
    /// Obtiene la fecha de expiración del access token
    /// </summary>
    /// <returns>Fecha de expiración</returns>
    DateTime GetAccessTokenExpirationDate();

    /// <summary>
    /// Obtiene la fecha de expiración del refresh token
    /// </summary>
    /// <returns>Fecha de expiración</returns>
    DateTime GetRefreshTokenExpirationDate();

    /// <summary>
    /// Valida si un token está expirado
    /// </summary>
    /// <param name="token">Token a validar</param>
    /// <returns>True si el token es válido y no está expirado</returns>
    bool ValidateToken(string token);
}
