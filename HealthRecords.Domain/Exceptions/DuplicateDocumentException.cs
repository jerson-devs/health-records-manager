namespace HealthRecords.Domain.Exceptions;

/// <summary>
/// Excepción lanzada cuando se intenta crear un paciente con un documento que ya existe
/// </summary>
public class DuplicateDocumentException : DomainException
{
    public DuplicateDocumentException(string documento) 
        : base($"Ya existe un paciente con el documento: {documento}")
    {
        Documento = documento;
    }

    public string Documento { get; }
}
