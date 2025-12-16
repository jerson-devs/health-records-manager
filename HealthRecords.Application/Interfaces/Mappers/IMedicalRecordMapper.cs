using HealthRecords.Application.Requests.MedicalRecord;
using HealthRecords.Application.Responses.MedicalRecord;
using HealthRecords.Domain.Models;

namespace HealthRecords.Application.Interfaces.Mappers;

/// <summary>
/// Interfaz para mapear entre entidades MedicalRecord y DTOs
/// </summary>
public interface IMedicalRecordMapper
{
    /// <summary>
    /// Mapea una entidad MedicalRecord a un MedicalRecordResponse
    /// </summary>
    MedicalRecordResponse MapToResponse(MedicalRecord medicalRecord);

    /// <summary>
    /// Mapea un CreateMedicalRecordRequest a una entidad MedicalRecord
    /// </summary>
    MedicalRecord MapToEntity(CreateMedicalRecordRequest request);

    /// <summary>
    /// Mapea un UpdateMedicalRecordRequest a una entidad MedicalRecord existente
    /// </summary>
    void MapToEntity(UpdateMedicalRecordRequest request, MedicalRecord medicalRecord);
}
