namespace HealthRecords.Domain.Exceptions;

/// <summary>
/// Excepción lanzada cuando no se encuentra un historial médico
/// </summary>
public class MedicalRecordNotFoundException : DomainException
{
    public MedicalRecordNotFoundException(int recordId) 
        : base($"No se encontró un historial médico con ID: {recordId}")
    {
        RecordId = recordId;
    }

    public int RecordId { get; }
}
