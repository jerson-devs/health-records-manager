using HealthRecords.Application.Requests.Patient;
using HealthRecords.Application.Responses.Patient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthRecords.Application.Interfaces;

/// <summary>
/// Interfaz del servicio de pacientes
/// </summary>
public interface IPatientService
{
    /// <summary>
    /// Obtiene todos los pacientes
    /// </summary>
    Task<IEnumerable<PatientResponse>> GetAllAsync();

    /// <summary>
    /// Obtiene un paciente por su ID
    /// </summary>
    Task<PatientResponse?> GetByIdAsync(int id);

    /// <summary>
    /// Obtiene un paciente con sus historiales médicos
    /// </summary>
    Task<PatientResponse?> GetByIdWithRecordsAsync(int id);

    /// <summary>
    /// Crea un nuevo paciente
    /// </summary>
    Task<PatientResponse> CreateAsync(CreatePatientRequest request);

    /// <summary>
    /// Actualiza un paciente existente
    /// </summary>
    Task<PatientResponse?> UpdateAsync(int id, UpdatePatientRequest request);

    /// <summary>
    /// Elimina un paciente
    /// </summary>
    Task<bool> DeleteAsync(int id);
}

