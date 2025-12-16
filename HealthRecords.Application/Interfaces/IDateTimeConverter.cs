using System;

namespace HealthRecords.Application.Interfaces;

/// <summary>
/// Interfaz para convertir fechas a UTC
/// </summary>
public interface IDateTimeConverter
{
    /// <summary>
    /// Convierte una fecha a UTC si no lo está
    /// </summary>
    /// <param name="dateTime">Fecha a convertir</param>
    /// <returns>Fecha en UTC</returns>
    DateTime ToUtc(DateTime dateTime);

    /// <summary>
    /// Convierte una fecha nullable a UTC si no lo está
    /// </summary>
    /// <param name="dateTime">Fecha nullable a convertir</param>
    /// <returns>Fecha en UTC o null</returns>
    DateTime? ToUtc(DateTime? dateTime);
}
