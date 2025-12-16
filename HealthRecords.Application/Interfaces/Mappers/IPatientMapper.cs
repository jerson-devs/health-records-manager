using HealthRecords.Application.Requests.Patient;
using HealthRecords.Application.Responses.Patient;
using HealthRecords.Domain.Models;

namespace HealthRecords.Application.Interfaces.Mappers;

/// <summary>
/// Interfaz para mapear entre entidades Patient y DTOs
/// </summary>
public interface IPatientMapper
{
    /// <summary>
    /// Mapea una entidad Patient a un PatientResponse
    /// </summary>
    PatientResponse MapToResponse(Patient patient);

    /// <summary>
    /// Mapea un CreatePatientRequest a una entidad Patient
    /// </summary>
    Patient MapToEntity(CreatePatientRequest request);

    /// <summary>
    /// Mapea un UpdatePatientRequest a una entidad Patient existente
    /// </summary>
    void MapToEntity(UpdatePatientRequest request, Patient patient);
}
