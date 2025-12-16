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
/// Implementación del repositorio para la entidad MedicalRecord.
/// Utiliza Entity Framework Core para acceso a datos.
/// </summary>
public class MedicalRecordRepository : IMedicalRecordRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Constructor que recibe el DbContext mediante inyección de dependencias
    /// </summary>
    /// <param name="context">Contexto de base de datos</param>
    public MedicalRecordRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<MedicalRecord>> GetAllAsync()
    {
        return await _context.MedicalRecords
            .Include(mr => mr.Patient)
            .OrderByDescending(mr => mr.Fecha)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<MedicalRecord?> GetByIdAsync(int id)
    {
        return await _context.MedicalRecords
            .Include(mr => mr.Patient)
            .FirstOrDefaultAsync(mr => mr.Id == id);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<MedicalRecord>> GetByPatientIdAsync(int patientId)
    {
        return await _context.MedicalRecords
            .Where(mr => mr.PatientId == patientId)
            .OrderByDescending(mr => mr.Fecha)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public Task<MedicalRecord> CreateAsync(MedicalRecord medicalRecord)
    {
        medicalRecord.CreatedAt = DateTime.UtcNow;
        _context.MedicalRecords.Add(medicalRecord);
        return Task.FromResult(medicalRecord);
    }

    /// <inheritdoc/>
    public Task<MedicalRecord> UpdateAsync(MedicalRecord medicalRecord)
    {
        medicalRecord.UpdatedAt = DateTime.UtcNow;
        _context.MedicalRecords.Update(medicalRecord);
        return Task.FromResult(medicalRecord);
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteAsync(int id)
    {
        var medicalRecord = await _context.MedicalRecords.FindAsync(id);
        if (medicalRecord == null)
            return false;

        _context.MedicalRecords.Remove(medicalRecord);
        return true;
    }
}

