using HealthRecords.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthRecords.Infrastructure.EntityConfigurations;

/// <summary>
/// Configuración de la entidad MedicalRecord usando Fluent API
/// </summary>
public class MedicalRecordConfiguration : IEntityTypeConfiguration<MedicalRecord>
{
    public void Configure(EntityTypeBuilder<MedicalRecord> entity)
    {
        entity.ToTable("MEDICAL_RECORDS"); // Nombre de tabla en mayúsculas (estilo Oracle)

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .HasColumnName("RECORD_ID")
            .ValueGeneratedOnAdd();

        entity.Property(e => e.PatientId)
            .HasColumnName("PATIENT_ID")
            .IsRequired();

        entity.Property(e => e.Fecha)
            .HasColumnName("FECHA")
            .IsRequired();

        entity.Property(e => e.Diagnostico)
            .HasColumnName("DIAGNOSTICO")
            .HasMaxLength(500)
            .IsRequired();

        entity.Property(e => e.Tratamiento)
            .HasColumnName("TRATAMIENTO")
            .HasMaxLength(1000)
            .IsRequired();

        entity.Property(e => e.Medico)
            .HasColumnName("MEDICO")
            .HasMaxLength(200)
            .IsRequired();

        entity.Property(e => e.CreatedAt)
            .HasColumnName("CREATED_AT")
            .IsRequired();

        entity.Property(e => e.UpdatedAt)
            .HasColumnName("UPDATED_AT");

        // Relación con Patient
        entity.HasOne(e => e.Patient)
            .WithMany(p => p.MedicalRecords)
            .HasForeignKey(e => e.PatientId)
            .OnDelete(DeleteBehavior.Restrict); // Evitar eliminación en cascada

        // Índice en PatientId para mejorar rendimiento
        entity.HasIndex(e => e.PatientId)
            .HasDatabaseName("IX_MEDICAL_RECORDS_PATIENT_ID");
    }
}
