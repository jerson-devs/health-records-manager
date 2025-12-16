using System;

namespace HealthRecords.Domain.ValueObjects;

/// <summary>
/// Value Object para representar un documento de identidad con validación
/// </summary>
public class Documento
{
    public string Value { get; }

    private Documento(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El documento no puede estar vacío", nameof(value));

        if (value.Length > 50)
            throw new ArgumentException("El documento no puede exceder 50 caracteres", nameof(value));

        Value = value;
    }

    public static Documento Create(string value)
    {
        return new Documento(value);
    }

    public static implicit operator string(Documento documento) => documento.Value;

    public static implicit operator Documento(string value) => new Documento(value);

    public override string ToString() => Value;

    public override bool Equals(object? obj)
    {
        if (obj is Documento other)
            return Value.Equals(other.Value, StringComparison.Ordinal);
        return false;
    }

    public override int GetHashCode() => Value.GetHashCode();
}
