using System;

namespace HealthRecords.Domain.Models;

/// <summary>
/// Entidad que representa un historial médico.
/// Simula estructura compatible con Oracle.
/// </summary>
public class MedicalRecord : BaseEntity
{
    /// <summary>
    /// Identificador del paciente (Foreign Key)
    /// </summary>
    public int PatientId { get; set; }

    /// <summary>
    /// Fecha del registro médico
    /// </summary>
    public DateTime Fecha { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Diagnóstico del paciente
    /// </summary>
    public string Diagnostico { get; set; } = string.Empty;

    /// <summary>
    /// Tratamiento prescrito
    /// </summary>
    public string Tratamiento { get; set; } = string.Empty;

    /// <summary>
    /// Nombre del médico que atendió
    /// </summary>
    public string Medico { get; set; } = string.Empty;

    /// <summary>
    /// Navegación: Paciente asociado a este historial
    /// </summary>
    public virtual Patient Patient { get; set; } = null!;
}

