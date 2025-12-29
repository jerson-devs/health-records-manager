using HealthRecords.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace HealthRecords.Infrastructure.Configuration;

/// <summary>
/// Extensión para configurar Oracle con Entity Framework Core.
/// </summary>
public static class OracleExtension
{
    /// <summary>
    /// Agrega y configura el DbContext de Oracle
    /// </summary>
    /// <param name="services">Colección de servicios</param>
    /// <param name="configuration">Configuración de la aplicación</param>
    /// <returns>Colección de servicios para encadenamiento</returns>
    public static IServiceCollection AddOracleContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("OracleConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "Oracle connection string 'OracleConnection' not found in configuration. " +
                "Please add it to appsettings.json or environment variables.");
        }

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseOracle(connectionString, oracleOptions =>
            {
                oracleOptions.MigrationsAssembly("HealthRecords.Infrastructure");
                // Configuraciones adicionales para Oracle
                oracleOptions.CommandTimeout(30);
            });

            // Habilitar logging de consultas SQL en desarrollo
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                options.EnableSensitiveDataLogging();
                options.LogTo(Console.WriteLine, LogLevel.Information);
            }
        });

        return services;
    }
}

