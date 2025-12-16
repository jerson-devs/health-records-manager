using HealthRecords.Application.Interfaces;
using HealthRecords.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HealthRecords.Infrastructure.Configuration;

/// <summary>
/// Extensión para registrar los servicios en el contenedor de DI
/// </summary>
public static class ServicesExtension
{
    /// <summary>
    /// Registra todos los servicios con scope Scoped
    /// </summary>
    /// <param name="services">Colección de servicios</param>
    /// <returns>Colección de servicios para encadenamiento</returns>
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IPatientService, PatientService>();
        services.AddScoped<IMedicalRecordService, MedicalRecordService>();
        services.AddScoped<Application.Interfaces.IDateTimeConverter, Application.Services.DateTimeConverter>();
        services.AddScoped<Application.Interfaces.Mappers.IPatientMapper, Application.Mappers.PatientMapper>();
        services.AddScoped<Application.Interfaces.Mappers.IMedicalRecordMapper, Application.Mappers.MedicalRecordMapper>();
        services.AddScoped<Application.Interfaces.IJwtTokenGenerator, Application.Services.JwtTokenGenerator>();

        return services;
    }
}

