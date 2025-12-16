using HealthRecords.Domain.Models;
using HealthRecords.Infrastructure.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace HealthRecords.Infrastructure;

/// <summary>
/// Contexto de base de datos para Entity Framework Core.
/// Configurado para SQL Server simulando estructura Oracle.
/// </summary>
public class ApplicationDbContext : DbContext
{
    /// <summary>
    /// Constructor sin parámetros (para migraciones)
    /// </summary>
    public ApplicationDbContext()
    {
    }

    /// <summary>
    /// Constructor con opciones de DbContext
    /// </summary>
    /// <param name="options">Opciones de configuración del DbContext</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// DbSet para la entidad Patient
    /// </summary>
    public DbSet<Patient> Patients { get; set; } = null!;

    /// <summary>
    /// DbSet para la entidad MedicalRecord
    /// </summary>
    public DbSet<MedicalRecord> MedicalRecords { get; set; } = null!;

    /// <summary>
    /// DbSet para la entidad User
    /// </summary>
    public DbSet<User> Users { get; set; } = null!;

    /// <summary>
    /// Override de SaveChangesAsync para convertir automáticamente todas las fechas a UTC
    /// </summary>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Convertir todas las fechas DateTime a UTC antes de guardar
        var entries = ChangeTracker.Entries()
            .Where(e => e.State == Microsoft.EntityFrameworkCore.EntityState.Added || 
                       e.State == Microsoft.EntityFrameworkCore.EntityState.Modified);

        foreach (var entry in entries)
        {
            foreach (var property in entry.Properties)
            {
                if (property.Metadata.ClrType == typeof(DateTime) && property.CurrentValue != null)
                {
                    var dateTime = (DateTime)property.CurrentValue;
                    if (dateTime.Kind != DateTimeKind.Utc)
                    {
                        property.CurrentValue = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
                    }
                }
                else if (property.Metadata.ClrType == typeof(DateTime?) && property.CurrentValue != null)
                {
                    var dateTime = (DateTime?)property.CurrentValue;
                    if (dateTime.HasValue && dateTime.Value.Kind != DateTimeKind.Utc)
                    {
                        property.CurrentValue = DateTime.SpecifyKind(dateTime.Value, DateTimeKind.Utc);
                    }
                }
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Configuración del modelo usando Fluent API
    /// </summary>
    /// <param name="modelBuilder">Builder para configurar el modelo</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aplicar todas las configuraciones de entidades desde el assembly
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}

