using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Threading.Tasks;

namespace HealthRecords.Infrastructure.Configuration;

/// <summary>
/// Extensión para configurar autenticación JWT
/// </summary>
public static class JwtAuthExtension
{
    /// <summary>
    /// Agrega y configura la autenticación JWT
    /// </summary>
    /// <param name="services">Colección de servicios</param>
    /// <param name="configuration">Configuración de la aplicación</param>
    /// <returns>Colección de servicios para encadenamiento</returns>
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JWT");
        var signingKey = jwtSettings["SigningKey"] ?? throw new InvalidOperationException("JWT SigningKey no configurado");
        var issuer = jwtSettings["Issuer"] ?? throw new InvalidOperationException("JWT Issuer no configurado");
        var audience = jwtSettings["Audience"] ?? throw new InvalidOperationException("JWT Audience no configurado");

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = true,
                ValidAudience = audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(5) // Tolerancia de 5 minutos para diferencias de reloj
            };

            // Configurar eventos para debugging y manejo del token
            options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    // Extraer el token del header Authorization
                    var token = context.Request.Headers["Authorization"].ToString();
                    
                    // Si el token comienza con "Bearer ", removerlo
                    if (!string.IsNullOrEmpty(token) && token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                    {
                        token = token.Substring("Bearer ".Length).Trim();
                    }
                    
                    // Si el token comienza con "bearer " (minúsculas), removerlo también
                    if (!string.IsNullOrEmpty(token) && token.StartsWith("bearer ", StringComparison.OrdinalIgnoreCase))
                    {
                        token = token.Substring("bearer ".Length).Trim();
                    }
                    
                    context.Token = token;
                    return Task.CompletedTask;
                },
                OnAuthenticationFailed = context =>
                {
                    var loggerFactory = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>();
                    var logger = loggerFactory.CreateLogger("JwtAuth");
                    logger.LogError(context.Exception, "Error de autenticación JWT: {Error}", context.Exception.Message);
                    return Task.CompletedTask;
                },
                OnTokenValidated = context =>
                {
                    var loggerFactory = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>();
                    var logger = loggerFactory.CreateLogger("JwtAuth");
                    logger.LogInformation("Token JWT validado exitosamente");
                    return Task.CompletedTask;
                },
                OnChallenge = context =>
                {
                    var loggerFactory = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>();
                    var logger = loggerFactory.CreateLogger("JwtAuth");
                    logger.LogWarning("Challenge JWT: {Error}, {ErrorDescription}", context.Error, context.ErrorDescription);
                    return Task.CompletedTask;
                }
            };
        });

        return services;
    }
}

