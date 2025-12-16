using HealthRecords.Application.Requests.MedicalRecord;
using HealthRecords.Application.Responses.MedicalRecord;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthRecords.Application.Interfaces;

/// <summary>
/// Interfaz del servicio de historiales médicos
/// </summary>
public interface IMedicalRecordService
{
    /// <summary>
    /// Obtiene todos los historiales médicos
    /// </summary>
    Task<IEnumerable<MedicalRecordResponse>> GetAllAsync();

    /// <summary>
    /// Obtiene un historial médico por su ID
    /// </summary>
    Task<MedicalRecordResponse?> GetByIdAsync(int id);

    /// <summary>
    /// Obtiene todos los historiales médicos de un paciente
    /// </summary>
    Task<IEnumerable<MedicalRecordResponse>> GetByPatientIdAsync(int patientId);

    /// <summary>
    /// Crea un nuevo historial médico
    /// </summary>
    Task<MedicalRecordResponse> CreateAsync(CreateMedicalRecordRequest request);

    /// <summary>
    /// Actualiza un historial médico existente
    /// </summary>
    Task<MedicalRecordResponse?> UpdateAsync(int id, UpdateMedicalRecordRequest request);

    /// <summary>
    /// Elimina un historial médico
    /// </summary>
    Task<bool> DeleteAsync(int id);
}

