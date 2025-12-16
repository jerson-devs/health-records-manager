using HealthRecords.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthRecords.Infrastructure.EntityConfigurations;

/// <summary>
/// Configuración de la entidad User usando Fluent API
/// </summary>
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> entity)
    {
        entity.ToTable("USERS"); // Nombre de tabla en mayúsculas (estilo Oracle)

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .HasColumnName("USER_ID")
            .ValueGeneratedOnAdd();

        entity.Property(e => e.Username)
            .HasColumnName("USERNAME")
            .HasMaxLength(50)
            .IsRequired();

        entity.Property(e => e.Email)
            .HasColumnName("EMAIL")
            .HasMaxLength(100)
            .IsRequired();

        entity.Property(e => e.PasswordHash)
            .HasColumnName("PASSWORD_HASH")
            .HasMaxLength(500)
            .IsRequired();

        entity.Property(e => e.Role)
            .HasColumnName("ROLE")
            .HasMaxLength(50)
            .IsRequired()
            .HasDefaultValue("User");

        entity.Property(e => e.CreatedAt)
            .HasColumnName("CREATED_AT")
            .IsRequired();

        entity.Property(e => e.UpdatedAt)
            .HasColumnName("UPDATED_AT")
            .IsRequired(false);

        // Índice único en Username
        entity.HasIndex(e => e.Username)
            .IsUnique()
            .HasDatabaseName("IX_USERS_USERNAME");

        // Índice único en Email
        entity.HasIndex(e => e.Email)
            .IsUnique()
            .HasDatabaseName("IX_USERS_EMAIL");
    }
}
