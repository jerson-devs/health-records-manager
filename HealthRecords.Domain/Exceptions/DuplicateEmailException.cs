namespace HealthRecords.Domain.Exceptions;

/// <summary>
/// Excepción lanzada cuando se intenta crear o actualizar un paciente con un email que ya existe
/// </summary>
public class DuplicateEmailException : DomainException
{
    public DuplicateEmailException(string email) 
        : base($"Ya existe un paciente con el email: {email}")
    {
        Email = email;
    }

    public string Email { get; }
}
