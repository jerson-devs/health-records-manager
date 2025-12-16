using System;

namespace HealthRecords.Domain.Exceptions;

/// <summary>
/// Clase base para todas las excepciones de dominio
/// </summary>
public abstract class DomainException : Exception
{
    protected DomainException(string message) : base(message)
    {
    }

    protected DomainException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
