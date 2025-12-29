using HealthRecords.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace HealthRecords.Infrastructure.Configuration;

/// <summary>
/// Factory para seleccionar el proveedor de base de datos según configuración.
/// Soporta PostgreSQL y Oracle sin modificar el código existente.
/// </summary>
public static class DatabaseFactoryExtension
{
    /// <summary>
    /// Tipos de proveedores de base de datos soportados
    /// </summary>
    public enum DatabaseProvider
    {
        PostgreSQL,
        Oracle
    }

    /// <summary>
    /// Agrega y configura el DbContext según el proveedor especificado en configuración.
    /// Por defecto usa PostgreSQL si no se especifica.
    /// </summary>
    /// <param name="services">Colección de servicios</param>
    /// <param name="configuration">Configuración de la aplicación</param>
    /// <returns>Colección de servicios para encadenamiento</returns>
    public static IServiceCollection AddDatabaseContext(this IServiceCollection services, IConfiguration configuration)
    {
        // Obtener el proveedor de base de datos desde configuración
        var providerString = configuration["DatabaseProvider"] ?? "PostgreSQL";
        
        if (!Enum.TryParse<DatabaseProvider>(providerString, ignoreCase: true, out var provider))
        {
            // Si no se puede parsear, usar PostgreSQL por defecto
            provider = DatabaseProvider.PostgreSQL;
        }

        // Configurar según el proveedor seleccionado
        switch (provider)
        {
            case DatabaseProvider.Oracle:
                return services.AddOracleContext(configuration);
            
            case DatabaseProvider.PostgreSQL:
            default:
                return services.AddPostgreSqlContext(configuration);
        }
    }

    /// <summary>
    /// Obtiene el proveedor de base de datos configurado
    /// </summary>
    /// <param name="configuration">Configuración de la aplicación</param>
    /// <returns>Proveedor de base de datos configurado</returns>
    public static DatabaseProvider GetDatabaseProvider(IConfiguration configuration)
    {
        var providerString = configuration["DatabaseProvider"] ?? "PostgreSQL";
        
        if (Enum.TryParse<DatabaseProvider>(providerString, ignoreCase: true, out var provider))
        {
            return provider;
        }

        return DatabaseProvider.PostgreSQL;
    }
}

