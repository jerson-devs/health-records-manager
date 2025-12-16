using System;
using System.Collections.Generic;

namespace HealthRecords.Domain.Models;

/// <summary>
/// Entidad que representa un paciente en el sistema.
/// Simula estructura compatible con Oracle.
/// </summary>
public class Patient : BaseEntity
{
    /// <summary>
    /// Nombre completo del paciente
    /// </summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Correo electrónico del paciente
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Fecha de nacimiento del paciente
    /// </summary>
    public DateTime FechaNacimiento { get; set; }

    /// <summary>
    /// Número de documento de identidad (DNI, Pasaporte, etc.)
    /// </summary>
    public string Documento { get; set; } = string.Empty;

    /// <summary>
    /// Navegación: Lista de historiales médicos del paciente
    /// </summary>
    public virtual ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
}

