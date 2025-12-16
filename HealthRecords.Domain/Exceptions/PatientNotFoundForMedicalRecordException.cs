namespace HealthRecords.Domain.Exceptions;

/// <summary>
/// Excepción lanzada cuando se intenta crear un historial médico para un paciente que no existe
/// </summary>
public class PatientNotFoundForMedicalRecordException : DomainException
{
    public PatientNotFoundForMedicalRecordException(int patientId) 
        : base($"No se encontró un paciente con ID: {patientId}")
    {
        PatientId = patientId;
    }

    public int PatientId { get; }
}
