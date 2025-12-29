using HealthRecords.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthRecords.Domain.Repositories;

/// <summary>
/// Interfaz del repositorio para ejecutar stored procedures.
/// Permite ejecutar procedimientos almacenados tanto en Oracle como en PostgreSQL.
/// </summary>
public interface IStoredProcedureRepository
{
    /// <summary>
    /// Obtiene un paciente por ID usando stored procedure
    /// </summary>
    /// <param name="patientId">ID del paciente</param>
    /// <returns>Paciente encontrado o null</returns>
    Task<Patient?> GetPatientByIdAsync(int patientId);

    /// <summary>
    /// Obtiene todos los pacientes usando stored procedure
    /// </summary>
    /// <returns>Lista de pacientes</returns>
    Task<IEnumerable<Patient>> GetAllPatientsAsync();

    /// <summary>
    /// Crea un nuevo paciente usando stored procedure
    /// </summary>
    /// <param name="nombre">Nombre del paciente</param>
    /// <param name="email">Email del paciente</param>
    /// <param name="fechaNacimiento">Fecha de nacimiento</param>
    /// <param name="documento">Número de documento</param>
    /// <returns>ID del paciente creado</returns>
    Task<int> CreatePatientAsync(string nombre, string email, DateTime fechaNacimiento, string documento);

    /// <summary>
    /// Actualiza un paciente usando stored procedure
    /// </summary>
    /// <param name="patientId">ID del paciente</param>
    /// <param name="nombre">Nombre del paciente</param>
    /// <param name="email">Email del paciente</param>
    /// <param name="fechaNacimiento">Fecha de nacimiento</param>
    /// <param name="documento">Número de documento</param>
    /// <returns>True si se actualizó correctamente</returns>
    Task<bool> UpdatePatientAsync(int patientId, string nombre, string email, DateTime fechaNacimiento, string documento);

    /// <summary>
    /// Elimina un paciente usando stored procedure
    /// </summary>
    /// <param name="patientId">ID del paciente</param>
    /// <returns>True si se eliminó correctamente</returns>
    Task<bool> DeletePatientAsync(int patientId);

    /// <summary>
    /// Obtiene un historial médico por ID usando stored procedure
    /// </summary>
    /// <param name="recordId">ID del historial médico</param>
    /// <returns>Historial médico encontrado o null</returns>
    Task<MedicalRecord?> GetMedicalRecordByIdAsync(int recordId);

    /// <summary>
    /// Obtiene todos los historiales médicos de un paciente usando stored procedure
    /// </summary>
    /// <param name="patientId">ID del paciente</param>
    /// <returns>Lista de historiales médicos</returns>
    Task<IEnumerable<MedicalRecord>> GetMedicalRecordsByPatientAsync(int patientId);

    /// <summary>
    /// Crea un nuevo historial médico usando stored procedure
    /// </summary>
    /// <param name="patientId">ID del paciente</param>
    /// <param name="fecha">Fecha del historial</param>
    /// <param name="diagnostico">Diagnóstico</param>
    /// <param name="tratamiento">Tratamiento</param>
    /// <param name="medico">Nombre del médico</param>
    /// <returns>ID del historial médico creado</returns>
    Task<int> CreateMedicalRecordAsync(int patientId, DateTime fecha, string diagnostico, string tratamiento, string medico);

    /// <summary>
    /// Actualiza un historial médico usando stored procedure
    /// </summary>
    /// <param name="recordId">ID del historial médico</param>
    /// <param name="patientId">ID del paciente</param>
    /// <param name="fecha">Fecha del historial</param>
    /// <param name="diagnostico">Diagnóstico</param>
    /// <param name="tratamiento">Tratamiento</param>
    /// <param name="medico">Nombre del médico</param>
    /// <returns>True si se actualizó correctamente</returns>
    Task<bool> UpdateMedicalRecordAsync(int recordId, int patientId, DateTime fecha, string diagnostico, string tratamiento, string medico);

    /// <summary>
    /// Elimina un historial médico usando stored procedure
    /// </summary>
    /// <param name="recordId">ID del historial médico</param>
    /// <returns>True si se eliminó correctamente</returns>
    Task<bool> DeleteMedicalRecordAsync(int recordId);

    /// <summary>
    /// Calcula la edad de un paciente usando función
    /// </summary>
    /// <param name="patientId">ID del paciente</param>
    /// <returns>Edad en años o null si no se encuentra</returns>
    Task<int?> GetPatientAgeAsync(int patientId);

    /// <summary>
    /// Valida el formato de un email usando función
    /// </summary>
    /// <param name="email">Email a validar</param>
    /// <returns>True si es válido</returns>
    Task<bool> ValidateEmailAsync(string email);

    /// <summary>
    /// Cuenta el número de historiales médicos de un paciente usando función
    /// </summary>
    /// <param name="patientId">ID del paciente</param>
    /// <returns>Número de historiales médicos</returns>
    Task<int> CountRecordsByPatientAsync(int patientId);
}

