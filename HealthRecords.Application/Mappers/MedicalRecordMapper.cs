using HealthRecords.Application.Interfaces;
using HealthRecords.Application.Interfaces.Mappers;
using HealthRecords.Application.Requests.MedicalRecord;
using HealthRecords.Application.Responses.MedicalRecord;
using HealthRecords.Domain.Models;

namespace HealthRecords.Application.Mappers;

/// <summary>
/// Implementación del mapper para MedicalRecord
/// </summary>
public class MedicalRecordMapper : IMedicalRecordMapper
{
    private readonly IDateTimeConverter _dateTimeConverter;

    public MedicalRecordMapper(IDateTimeConverter dateTimeConverter)
    {
        _dateTimeConverter = dateTimeConverter;
    }

    /// <inheritdoc/>
    public MedicalRecordResponse MapToResponse(MedicalRecord record)
    {
        return new MedicalRecordResponse
        {
            Id = record.Id,
            PatientId = record.PatientId,
            PatientName = record.Patient?.Nombre ?? string.Empty,
            Fecha = record.Fecha,
            Diagnostico = record.Diagnostico,
            Tratamiento = record.Tratamiento,
            Medico = record.Medico,
            CreatedAt = record.CreatedAt,
            UpdatedAt = record.UpdatedAt
        };
    }

    /// <inheritdoc/>
    public MedicalRecord MapToEntity(CreateMedicalRecordRequest request)
    {
        return new MedicalRecord
        {
            PatientId = request.PatientId,
            Fecha = _dateTimeConverter.ToUtc(request.Fecha),
            Diagnostico = request.Diagnostico,
            Tratamiento = request.Tratamiento,
            Medico = request.Medico
        };
    }

    /// <inheritdoc/>
    public void MapToEntity(UpdateMedicalRecordRequest request, MedicalRecord medicalRecord)
    {
        medicalRecord.Fecha = _dateTimeConverter.ToUtc(request.Fecha);
        medicalRecord.Diagnostico = request.Diagnostico;
        medicalRecord.Tratamiento = request.Tratamiento;
        medicalRecord.Medico = request.Medico;
    }
}
