using HealthRecords.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace HealthRecords.Infrastructure.Configuration;

/// <summary>
/// Extensión para configurar PostgreSQL con Entity Framework Core.
/// Simula estructura compatible con Oracle (nombres en mayúsculas).
/// </summary>
public static class PostgreSqlExtension
{
    /// <summary>
    /// Agrega y configura el DbContext de PostgreSQL
    /// </summary>
    /// <param name="services">Colección de servicios</param>
    /// <param name="configuration">Configuración de la aplicación</param>
    /// <returns>Colección de servicios para encadenamiento</returns>
    public static IServiceCollection AddPostgreSqlContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.MigrationsAssembly("HealthRecords.Infrastructure");
                // Configuraciones adicionales para simular comportamiento Oracle
                npgsqlOptions.CommandTimeout(30);
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
