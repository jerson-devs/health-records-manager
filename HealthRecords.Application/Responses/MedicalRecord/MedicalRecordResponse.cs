using System;

namespace HealthRecords.Application.Responses.MedicalRecord;

/// <summary>
/// DTO para la respuesta de un historial médico
/// </summary>
public class MedicalRecordResponse
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
    public string Diagnostico { get; set; } = string.Empty;
    public string Tratamiento { get; set; } = string.Empty;
    public string Medico { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

