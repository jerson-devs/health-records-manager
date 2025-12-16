using HealthRecords.Application.Interfaces;
using System;

namespace HealthRecords.Application.Services;

/// <summary>
/// Servicio para convertir fechas a UTC
/// </summary>
public class DateTimeConverter : IDateTimeConverter
{
    /// <inheritdoc/>
    public DateTime ToUtc(DateTime dateTime)
    {
        return dateTime.Kind == DateTimeKind.Utc
            ? dateTime
            : DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
    }

    /// <inheritdoc/>
    public DateTime? ToUtc(DateTime? dateTime)
    {
        if (!dateTime.HasValue)
            return null;

        return dateTime.Value.Kind == DateTimeKind.Utc
            ? dateTime
            : DateTime.SpecifyKind(dateTime.Value, DateTimeKind.Utc);
    }
}
