using HealthRecords.Domain.Models;
using HealthRecords.Domain.Repositories;
using HealthRecords.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthRecords.Infrastructure.Repositories;

/// <summary>
/// Implementación del repositorio para la entidad Patient.
/// Utiliza Entity Framework Core para acceso a datos.
/// </summary>
public class PatientRepository : IPatientRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Constructor que recibe el DbContext mediante inyección de dependencias
    /// </summary>
    /// <param name="context">Contexto de base de datos</param>
    public PatientRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Patient>> GetAllAsync()
    {
        return await _context.Patients
            .OrderBy(p => p.Nombre)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<Patient?> GetByIdAsync(int id)
    {
        return await _context.Patients
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    /// <inheritdoc/>
    public async Task<Patient?> GetByIdWithRecordsAsync(int id)
    {
        return await _context.Patients
            .Include(p => p.MedicalRecords.OrderByDescending(r => r.Fecha))
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    /// <inheritdoc/>
    public Task<Patient> CreateAsync(Patient patient)
    {
        patient.CreatedAt = DateTime.UtcNow;
        _context.Patients.Add(patient);
        return Task.FromResult(patient);
    }

    /// <inheritdoc/>
    public Task<Patient> UpdateAsync(Patient patient)
    {
        patient.UpdatedAt = DateTime.UtcNow;
        _context.Patients.Update(patient);
        return Task.FromResult(patient);
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteAsync(int id)
    {
        var patient = await _context.Patients.FindAsync(id);
        if (patient == null)
            return false;

        _context.Patients.Remove(patient);
        return true;
    }

    /// <inheritdoc/>
    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _context.Patients
            .AnyAsync(p => p.Email == email);
    }

    /// <inheritdoc/>
    public async Task<bool> ExistsByDocumentoAsync(string documento)
    {
        return await _context.Patients
            .AnyAsync(p => p.Documento == documento);
    }
}

