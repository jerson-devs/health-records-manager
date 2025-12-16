using System;
using System.Collections.Generic;

namespace HealthRecords.Application.Responses.Patient;

/// <summary>
/// DTO para la respuesta de un paciente
/// </summary>
public class PatientResponse
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime FechaNacimiento { get; set; }
    public string Documento { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<MedicalRecordSummary>? MedicalRecords { get; set; }
}

/// <summary>
/// Resumen de un historial médico para incluir en la respuesta del paciente
/// </summary>
public class MedicalRecordSummary
{
    public int Id { get; set; }
    public DateTime Fecha { get; set; }
    public string Diagnostico { get; set; } = string.Empty;
    public string Tratamiento { get; set; } = string.Empty;
    public string Medico { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

