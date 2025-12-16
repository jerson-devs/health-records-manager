using HealthRecords.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthRecords.Domain.Repositories;

/// <summary>
/// Interfaz del repositorio para la entidad Patient.
/// Define los métodos de acceso a datos para pacientes.
/// </summary>
public interface IPatientRepository
{
    /// <summary>
    /// Obtiene todos los pacientes
    /// </summary>
    /// <returns>Lista de pacientes</returns>
    Task<IEnumerable<Patient>> GetAllAsync();

    /// <summary>
    /// Obtiene un paciente por su ID
    /// </summary>
    /// <param name="id">ID del paciente</param>
    /// <returns>Paciente encontrado o null</returns>
    Task<Patient?> GetByIdAsync(int id);

    /// <summary>
    /// Obtiene un paciente con sus historiales médicos
    /// </summary>
    /// <param name="id">ID del paciente</param>
    /// <returns>Paciente con historiales médicos o null</returns>
    Task<Patient?> GetByIdWithRecordsAsync(int id);

    /// <summary>
    /// Crea un nuevo paciente
    /// </summary>
    /// <param name="patient">Paciente a crear</param>
    /// <returns>Paciente creado</returns>
    Task<Patient> CreateAsync(Patient patient);

    /// <summary>
    /// Actualiza un paciente existente
    /// </summary>
    /// <param name="patient">Paciente a actualizar</param>
    /// <returns>Paciente actualizado</returns>
    Task<Patient> UpdateAsync(Patient patient);

    /// <summary>
    /// Elimina un paciente
    /// </summary>
    /// <param name="id">ID del paciente a eliminar</param>
    /// <returns>True si se eliminó correctamente</returns>
    Task<bool> DeleteAsync(int id);

    /// <summary>
    /// Verifica si existe un paciente con el email dado
    /// </summary>
    /// <param name="email">Email a verificar</param>
    /// <returns>True si existe</returns>
    Task<bool> ExistsByEmailAsync(string email);

    /// <summary>
    /// Verifica si existe un paciente con el documento dado
    /// </summary>
    /// <param name="documento">Documento a verificar</param>
    /// <returns>True si existe</returns>
    Task<bool> ExistsByDocumentoAsync(string documento);
}

