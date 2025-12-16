using HealthRecords.Application.Interfaces;
using HealthRecords.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace HealthRecords.Application.Services;

/// <summary>
/// Servicio para generar tokens JWT
/// </summary>
public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly IConfiguration _configuration;
    private readonly JwtSecurityTokenHandler _tokenHandler;

    public JwtTokenGenerator(IConfiguration configuration)
    {
        _configuration = configuration;
        _tokenHandler = new JwtSecurityTokenHandler();
    }

    /// <inheritdoc/>
    public string GenerateAccessToken(User user)
    {
        var jwtSettings = _configuration.GetSection("JWT");
        var signingKey = jwtSettings["SigningKey"] ?? throw new InvalidOperationException("JWT SigningKey no configurado");
        var issuer = jwtSettings["Issuer"] ?? throw new InvalidOperationException("JWT Issuer no configurado");
        var audience = jwtSettings["Audience"] ?? throw new InvalidOperationException("JWT Audience no configurado");
        var expirationMinutes = int.Parse(jwtSettings["AccessTokenExpirationMinutes"] ?? "15");

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: GetAccessTokenExpirationDate(),
            signingCredentials: credentials
        );

        return _tokenHandler.WriteToken(token);
    }

    /// <inheritdoc/>
    public string GenerateRefreshToken(User user)
    {
        var jwtSettings = _configuration.GetSection("JWT");
        var signingKey = jwtSettings["SigningKey"] ?? throw new InvalidOperationException("JWT SigningKey no configurado");
        var issuer = jwtSettings["Issuer"] ?? throw new InvalidOperationException("JWT Issuer no configurado");
        var audience = jwtSettings["Audience"] ?? throw new InvalidOperationException("JWT Audience no configurado");
        var expirationDays = int.Parse(jwtSettings["RefreshTokenExpirationDays"] ?? "7");

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("token_type", "refresh"),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: GetRefreshTokenExpirationDate(),
            signingCredentials: credentials
        );

        return _tokenHandler.WriteToken(token);
    }

    /// <inheritdoc/>
    public DateTime GetAccessTokenExpirationDate()
    {
        var jwtSettings = _configuration.GetSection("JWT");
        var expirationMinutes = int.Parse(jwtSettings["AccessTokenExpirationMinutes"] ?? "15");
        return DateTime.UtcNow.AddMinutes(expirationMinutes);
    }

    /// <inheritdoc/>
    public DateTime GetRefreshTokenExpirationDate()
    {
        var jwtSettings = _configuration.GetSection("JWT");
        var expirationDays = int.Parse(jwtSettings["RefreshTokenExpirationDays"] ?? "7");
        return DateTime.UtcNow.AddDays(expirationDays);
    }

    /// <inheritdoc/>
    public bool ValidateToken(string token)
    {
        try
        {
            var jwtSettings = _configuration.GetSection("JWT");
            var signingKey = jwtSettings["SigningKey"] ?? throw new InvalidOperationException("JWT SigningKey no configurado");
            var issuer = jwtSettings["Issuer"] ?? throw new InvalidOperationException("JWT Issuer no configurado");
            var audience = jwtSettings["Audience"] ?? throw new InvalidOperationException("JWT Audience no configurado");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = true,
                ValidAudience = audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            _tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
