using System;
using System.Text.RegularExpressions;

namespace HealthRecords.Domain.ValueObjects;

/// <summary>
/// Value Object para representar un nombre de usuario con validación
/// </summary>
public class Username
{
    private static readonly Regex UsernameRegex = new Regex(
        @"^[a-zA-Z0-9_]{3,50}$",
        RegexOptions.Compiled);

    public string Value { get; }

    private Username(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El nombre de usuario no puede estar vacío", nameof(value));

        if (value.Length < 3 || value.Length > 50)
            throw new ArgumentException("El nombre de usuario debe tener entre 3 y 50 caracteres", nameof(value));

        if (!UsernameRegex.IsMatch(value))
            throw new ArgumentException("El nombre de usuario solo puede contener letras, números y guiones bajos", nameof(value));

        Value = value;
    }

    public static Username Create(string value)
    {
        return new Username(value);
    }

    public static implicit operator string(Username username) => username.Value;

    public static implicit operator Username(string value) => new Username(value);

    public override string ToString() => Value;

    public override bool Equals(object? obj)
    {
        if (obj is Username other)
            return Value.Equals(other.Value, StringComparison.Ordinal);
        return false;
    }

    public override int GetHashCode() => Value.GetHashCode();
}
