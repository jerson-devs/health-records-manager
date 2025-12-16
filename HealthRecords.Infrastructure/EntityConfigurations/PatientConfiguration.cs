using HealthRecords.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthRecords.Infrastructure.EntityConfigurations;

/// <summary>
/// Configuración de la entidad Patient usando Fluent API
/// </summary>
public class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> entity)
    {
        entity.ToTable("PATIENTS"); // Nombre de tabla en mayúsculas (estilo Oracle)

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .HasColumnName("PATIENT_ID")
            .ValueGeneratedOnAdd();

        entity.Property(e => e.Nombre)
            .HasColumnName("NOMBRE")
            .HasMaxLength(200)
            .IsRequired();

        entity.Property(e => e.Email)
            .HasColumnName("EMAIL")
            .HasMaxLength(100)
            .IsRequired();

        entity.Property(e => e.FechaNacimiento)
            .HasColumnName("FECHA_NACIMIENTO")
            .IsRequired();

        entity.Property(e => e.Documento)
            .HasColumnName("DOCUMENTO")
            .HasMaxLength(50)
            .IsRequired();

        entity.Property(e => e.CreatedAt)
            .HasColumnName("CREATED_AT")
            .IsRequired();

        entity.Property(e => e.UpdatedAt)
            .HasColumnName("UPDATED_AT");

        // Índice único en Email
        entity.HasIndex(e => e.Email)
            .IsUnique()
            .HasDatabaseName("IX_PATIENTS_EMAIL");

        // Índice único en Documento
        entity.HasIndex(e => e.Documento)
            .IsUnique()
            .HasDatabaseName("IX_PATIENTS_DOCUMENTO");
    }
}
