using System;
using System.Text.RegularExpressions;

namespace HealthRecords.Domain.ValueObjects;

/// <summary>
/// Value Object para representar un email con validación
/// </summary>
public class Email
{
    private static readonly Regex EmailRegex = new Regex(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public string Value { get; }

    private Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El email no puede estar vacío", nameof(value));

        if (!EmailRegex.IsMatch(value))
            throw new ArgumentException("El formato del email no es válido", nameof(value));

        if (value.Length > 100)
            throw new ArgumentException("El email no puede exceder 100 caracteres", nameof(value));

        Value = value;
    }

    public static Email Create(string value)
    {
        return new Email(value);
    }

    public static implicit operator string(Email email) => email.Value;

    public static implicit operator Email(string value) => new Email(value);

    public override string ToString() => Value;

    public override bool Equals(object? obj)
    {
        if (obj is Email other)
            return Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase);
        return false;
    }

    public override int GetHashCode() => Value.ToLowerInvariant().GetHashCode();
}
