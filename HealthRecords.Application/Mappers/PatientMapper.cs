using HealthRecords.Application.Interfaces;
using HealthRecords.Application.Interfaces.Mappers;
using HealthRecords.Application.Requests.Patient;
using HealthRecords.Application.Responses.Patient;
using HealthRecords.Domain.Models;
using System.Linq;

namespace HealthRecords.Application.Mappers;

/// <summary>
/// Implementación del mapper para Patient
/// </summary>
public class PatientMapper : IPatientMapper
{
    private readonly IDateTimeConverter _dateTimeConverter;

    public PatientMapper(IDateTimeConverter dateTimeConverter)
    {
        _dateTimeConverter = dateTimeConverter;
    }

    /// <inheritdoc/>
    public PatientResponse MapToResponse(Patient patient)
    {
        return new PatientResponse
        {
            Id = patient.Id,
            Nombre = patient.Nombre,
            Email = patient.Email,
            FechaNacimiento = patient.FechaNacimiento,
            Documento = patient.Documento,
            CreatedAt = patient.CreatedAt,
            UpdatedAt = patient.UpdatedAt,
            MedicalRecords = patient.MedicalRecords?
                .Select(mr => new MedicalRecordSummary
                {
                    Id = mr.Id,
                    Fecha = mr.Fecha,
                    Diagnostico = mr.Diagnostico,
                    Tratamiento = mr.Tratamiento,
                    Medico = mr.Medico,
                    CreatedAt = mr.CreatedAt,
                    UpdatedAt = mr.UpdatedAt
                })
                .ToList()
        };
    }

    /// <inheritdoc/>
    public Patient MapToEntity(CreatePatientRequest request)
    {
        return new Patient
        {
            Nombre = request.Nombre,
            Email = request.Email,
            FechaNacimiento = _dateTimeConverter.ToUtc(request.FechaNacimiento),
            Documento = request.Documento
        };
    }

    /// <inheritdoc/>
    public void MapToEntity(UpdatePatientRequest request, Patient patient)
    {
        patient.Nombre = request.Nombre;
        patient.Email = request.Email;
        patient.FechaNacimiento = _dateTimeConverter.ToUtc(request.FechaNacimiento);
        patient.Documento = request.Documento;
    }
}
