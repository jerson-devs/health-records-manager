using HealthRecords.Domain.Repositories;
using HealthRecords.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace HealthRecords.Infrastructure.Repositories;

/// <summary>
/// Implementación del patrón Unit of Work
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IPatientRepository? _patients;
    private IMedicalRecordRepository? _medicalRecords;
    private IUserRepository? _users;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc/>
    public IPatientRepository Patients
    {
        get
        {
            return _patients ??= new PatientRepository(_context);
        }
    }

    /// <inheritdoc/>
    public IMedicalRecordRepository MedicalRecords
    {
        get
        {
            return _medicalRecords ??= new MedicalRecordRepository(_context);
        }
    }

    /// <inheritdoc/>
    public IUserRepository Users
    {
        get
        {
            return _users ??= new UserRepository(_context);
        }
    }

    /// <inheritdoc/>
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
