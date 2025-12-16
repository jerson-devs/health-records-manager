using System;

namespace HealthRecords.Domain.Models;

/// <summary>
/// Clase base para todas las entidades del dominio
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Identificador único de la entidad (Primary Key)
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Fecha de creación del registro
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Fecha de última actualización del registro
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}
