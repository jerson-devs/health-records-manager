using HealthRecords.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthRecords.Domain.Repositories;

/// <summary>
/// Interfaz del repositorio para la entidad MedicalRecord.
/// Define los métodos de acceso a datos para historiales médicos.
/// </summary>
public interface IMedicalRecordRepository
{
    /// <summary>
    /// Obtiene todos los historiales médicos
    /// </summary>
    /// <returns>Lista de historiales médicos</returns>
    Task<IEnumerable<MedicalRecord>> GetAllAsync();

    /// <summary>
    /// Obtiene un historial médico por su ID
    /// </summary>
    /// <param name="id">ID del historial médico</param>
    /// <returns>Historial médico encontrado o null</returns>
    Task<MedicalRecord?> GetByIdAsync(int id);

    /// <summary>
    /// Obtiene todos los historiales médicos de un paciente
    /// </summary>
    /// <param name="patientId">ID del paciente</param>
    /// <returns>Lista de historiales médicos del paciente</returns>
    Task<IEnumerable<MedicalRecord>> GetByPatientIdAsync(int patientId);

    /// <summary>
    /// Crea un nuevo historial médico
    /// </summary>
    /// <param name="medicalRecord">Historial médico a crear</param>
    /// <returns>Historial médico creado</returns>
    Task<MedicalRecord> CreateAsync(MedicalRecord medicalRecord);

    /// <summary>
    /// Actualiza un historial médico existente
    /// </summary>
    /// <param name="medicalRecord">Historial médico a actualizar</param>
    /// <returns>Historial médico actualizado</returns>
    Task<MedicalRecord> UpdateAsync(MedicalRecord medicalRecord);

    /// <summary>
    /// Elimina un historial médico
    /// </summary>
    /// <param name="id">ID del historial médico a eliminar</param>
    /// <returns>True si se eliminó correctamente</returns>
    Task<bool> DeleteAsync(int id);
}

