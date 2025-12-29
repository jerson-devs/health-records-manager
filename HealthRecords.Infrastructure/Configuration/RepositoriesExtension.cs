using HealthRecords.Domain.Repositories;
using HealthRecords.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HealthRecords.Infrastructure.Configuration;

/// <summary>
/// Extensión para registrar los repositorios en el contenedor de DI
/// </summary>
public static class RepositoriesExtension
{
    /// <summary>
    /// Registra todos los repositorios con scope Scoped
    /// </summary>
    /// <param name="services">Colección de servicios</param>
    /// <returns>Colección de servicios para encadenamiento</returns>
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        // Los repositorios se crean a través de UnitOfWork, pero los registramos también para compatibilidad
        services.AddScoped<IPatientRepository, PatientRepository>();
        services.AddScoped<IMedicalRecordRepository, MedicalRecordRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        // Repositorio para stored procedures
        services.AddScoped<IStoredProcedureRepository, StoredProcedureRepository>();

        return services;
    }
}

