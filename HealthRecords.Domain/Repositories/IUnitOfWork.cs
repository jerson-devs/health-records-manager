using System.Threading;
using System.Threading.Tasks;

namespace HealthRecords.Domain.Repositories;

/// <summary>
/// Interfaz para el patrón Unit of Work que agrupa múltiples operaciones de repositorio en una sola transacción
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Repositorio de pacientes
    /// </summary>
    IPatientRepository Patients { get; }

    /// <summary>
    /// Repositorio de historiales médicos
    /// </summary>
    IMedicalRecordRepository MedicalRecords { get; }

    /// <summary>
    /// Repositorio de usuarios
    /// </summary>
    IUserRepository Users { get; }

    /// <summary>
    /// Guarda todos los cambios realizados en los repositorios
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>Número de entidades afectadas</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
