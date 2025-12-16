namespace HealthRecords.Domain.Exceptions;

/// <summary>
/// Excepción lanzada cuando no se encuentra un paciente
/// </summary>
public class PatientNotFoundException : DomainException
{
    public PatientNotFoundException(int patientId) 
        : base($"No se encontró un paciente con ID: {patientId}")
    {
        PatientId = patientId;
    }

    public int PatientId { get; }
}
