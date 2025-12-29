using HealthRecords.Domain.Models;
using HealthRecords.Domain.Repositories;
using HealthRecords.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthRecords.Infrastructure.Repositories;

/// <summary>
/// Implementación del repositorio para ejecutar stored procedures.
/// Soporta tanto Oracle (PL/SQL) como PostgreSQL (PL/pgSQL).
/// Nota: Esta implementación requiere que los stored procedures estén creados en la base de datos.
/// </summary>
public class StoredProcedureRepository : IStoredProcedureRepository
{
    private readonly ApplicationDbContext _context;
    private readonly DatabaseFactoryExtension.DatabaseProvider _provider;

    public StoredProcedureRepository(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _provider = DatabaseFactoryExtension.GetDatabaseProvider(configuration);
    }

    public async Task<Patient?> GetPatientByIdAsync(int patientId)
    {
        if (_provider == DatabaseFactoryExtension.DatabaseProvider.Oracle)
        {
            // Oracle: Usar package procedure con REF CURSOR
            // Nota: La ejecución de packages Oracle con REF CURSOR requiere configuración adicional
            // Por ahora, usamos consulta directa como alternativa
            return await _context.Patients
                .Where(p => p.Id == patientId)
                .FirstOrDefaultAsync();
        }
        else
        {
            // PostgreSQL: Usar función
            return await _context.Patients
                .FromSqlRaw("SELECT * FROM sp_get_patient_by_id({0})", patientId)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }
    }

    public async Task<IEnumerable<Patient>> GetAllPatientsAsync()
    {
        if (_provider == DatabaseFactoryExtension.DatabaseProvider.Oracle)
        {
            // Oracle: Usar package procedure
            // Nota: La ejecución de packages Oracle con REF CURSOR requiere configuración adicional
            // Por ahora, usamos consulta directa como alternativa
            return await _context.Patients
                .OrderBy(p => p.Nombre)
                .ToListAsync();
        }
        else
        {
            // PostgreSQL: Usar función
            return await _context.Patients
                .FromSqlRaw("SELECT * FROM sp_get_all_patients()")
                .AsNoTracking()
                .ToListAsync();
        }
    }

    public async Task<int> CreatePatientAsync(string nombre, string email, DateTime fechaNacimiento, string documento)
    {
        if (_provider == DatabaseFactoryExtension.DatabaseProvider.Oracle)
        {
            // Oracle: Usar package procedure con parámetro OUT
            // Nota: La ejecución de packages Oracle con parámetros OUT requiere configuración adicional
            // Por ahora, creamos el paciente directamente y retornamos el ID
            var patient = new Patient
            {
                Nombre = nombre,
                Email = email,
                FechaNacimiento = fechaNacimiento,
                Documento = documento,
                CreatedAt = DateTime.UtcNow
            };
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
            return patient.Id;
        }
        else
        {
            // PostgreSQL: Usar función
            var result = await _context.Database
                .SqlQueryRaw<int>("SELECT sp_create_patient({0}, {1}, {2}, {3})", nombre, email, fechaNacimiento, documento)
                .FirstOrDefaultAsync();
            return result;
        }
    }

    public async Task<bool> UpdatePatientAsync(int patientId, string nombre, string email, DateTime fechaNacimiento, string documento)
    {
        if (_provider == DatabaseFactoryExtension.DatabaseProvider.Oracle)
        {
            // Oracle: Usar package procedure
            // Nota: La ejecución de packages Oracle con parámetros OUT requiere configuración adicional
            // Por ahora, actualizamos directamente
            var patient = await _context.Patients.FindAsync(patientId);
            if (patient == null) return false;

            patient.Nombre = nombre;
            patient.Email = email;
            patient.FechaNacimiento = fechaNacimiento;
            patient.Documento = documento;
            patient.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
        else
        {
            // PostgreSQL: Usar función
            var result = await _context.Database
                .SqlQueryRaw<int>("SELECT sp_update_patient({0}, {1}, {2}, {3}, {4})", patientId, nombre, email, fechaNacimiento, documento)
                .FirstOrDefaultAsync();
            return result > 0;
        }
    }

    public async Task<bool> DeletePatientAsync(int patientId)
    {
        if (_provider == DatabaseFactoryExtension.DatabaseProvider.Oracle)
        {
            // Oracle: Usar package procedure
            // Nota: La ejecución de packages Oracle con parámetros OUT requiere configuración adicional
            // Por ahora, eliminamos directamente
            var patient = await _context.Patients.FindAsync(patientId);
            if (patient == null) return false;

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            return true;
        }
        else
        {
            // PostgreSQL: Usar función
            var result = await _context.Database
                .SqlQueryRaw<int>("SELECT sp_delete_patient({0})", patientId)
                .FirstOrDefaultAsync();
            return result > 0;
        }
    }

    public async Task<MedicalRecord?> GetMedicalRecordByIdAsync(int recordId)
    {
        if (_provider == DatabaseFactoryExtension.DatabaseProvider.Oracle)
        {
            // Oracle: Usar package procedure
            // Nota: La ejecución de packages Oracle con REF CURSOR requiere configuración adicional
            // Por ahora, usamos consulta directa como alternativa
            return await _context.MedicalRecords
                .Where(mr => mr.Id == recordId)
                .FirstOrDefaultAsync();
        }
        else
        {
            // PostgreSQL: Usar función
            return await _context.MedicalRecords
                .FromSqlRaw("SELECT * FROM sp_get_record_by_id({0})", recordId)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }
    }

    public async Task<IEnumerable<MedicalRecord>> GetMedicalRecordsByPatientAsync(int patientId)
    {
        if (_provider == DatabaseFactoryExtension.DatabaseProvider.Oracle)
        {
            // Oracle: Usar package procedure
            // Nota: La ejecución de packages Oracle con REF CURSOR requiere configuración adicional
            // Por ahora, usamos consulta directa como alternativa
            return await _context.MedicalRecords
                .Where(mr => mr.PatientId == patientId)
                .OrderByDescending(mr => mr.Fecha)
                .ToListAsync();
        }
        else
        {
            // PostgreSQL: Usar función
            return await _context.MedicalRecords
                .FromSqlRaw("SELECT * FROM sp_get_records_by_patient({0})", patientId)
                .AsNoTracking()
                .ToListAsync();
        }
    }

    public async Task<int> CreateMedicalRecordAsync(int patientId, DateTime fecha, string diagnostico, string tratamiento, string medico)
    {
        if (_provider == DatabaseFactoryExtension.DatabaseProvider.Oracle)
        {
            // Oracle: Usar package procedure con parámetro OUT
            // Nota: La ejecución de packages Oracle con parámetros OUT requiere configuración adicional
            // Por ahora, creamos el historial directamente y retornamos el ID
            var record = new MedicalRecord
            {
                PatientId = patientId,
                Fecha = fecha,
                Diagnostico = diagnostico,
                Tratamiento = tratamiento,
                Medico = medico,
                CreatedAt = DateTime.UtcNow
            };
            _context.MedicalRecords.Add(record);
            await _context.SaveChangesAsync();
            return record.Id;
        }
        else
        {
            // PostgreSQL: Usar función
            var result = await _context.Database
                .SqlQueryRaw<int>("SELECT sp_create_record({0}, {1}, {2}, {3}, {4})", patientId, fecha, diagnostico, tratamiento, medico)
                .FirstOrDefaultAsync();
            return result;
        }
    }

    public async Task<bool> UpdateMedicalRecordAsync(int recordId, int patientId, DateTime fecha, string diagnostico, string tratamiento, string medico)
    {
        if (_provider == DatabaseFactoryExtension.DatabaseProvider.Oracle)
        {
            // Oracle: Usar package procedure
            // Nota: La ejecución de packages Oracle con parámetros OUT requiere configuración adicional
            // Por ahora, actualizamos directamente
            var record = await _context.MedicalRecords.FindAsync(recordId);
            if (record == null) return false;

            record.PatientId = patientId;
            record.Fecha = fecha;
            record.Diagnostico = diagnostico;
            record.Tratamiento = tratamiento;
            record.Medico = medico;
            record.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
        else
        {
            // PostgreSQL: Usar función
            var result = await _context.Database
                .SqlQueryRaw<int>("SELECT sp_update_record({0}, {1}, {2}, {3}, {4}, {5})", recordId, patientId, fecha, diagnostico, tratamiento, medico)
                .FirstOrDefaultAsync();
            return result > 0;
        }
    }

    public async Task<bool> DeleteMedicalRecordAsync(int recordId)
    {
        if (_provider == DatabaseFactoryExtension.DatabaseProvider.Oracle)
        {
            // Oracle: Usar package procedure
            // Nota: La ejecución de packages Oracle con parámetros OUT requiere configuración adicional
            // Por ahora, eliminamos directamente
            var record = await _context.MedicalRecords.FindAsync(recordId);
            if (record == null) return false;

            _context.MedicalRecords.Remove(record);
            await _context.SaveChangesAsync();
            return true;
        }
        else
        {
            // PostgreSQL: Usar función
            var result = await _context.Database
                .SqlQueryRaw<int>("SELECT sp_delete_record({0})", recordId)
                .FirstOrDefaultAsync();
            return result > 0;
        }
    }

    public async Task<int?> GetPatientAgeAsync(int patientId)
    {
        if (_provider == DatabaseFactoryExtension.DatabaseProvider.Oracle)
        {
            // Oracle: Usar función
            var result = await _context.Database
                .SqlQueryRaw<int?>("SELECT FN_GET_PATIENT_AGE({0}) FROM DUAL", patientId)
                .FirstOrDefaultAsync();
            return result;
        }
        else
        {
            // PostgreSQL: Usar función
            var result = await _context.Database
                .SqlQueryRaw<int?>("SELECT fn_get_patient_age({0})", patientId)
                .FirstOrDefaultAsync();
            return result;
        }
    }

    public async Task<bool> ValidateEmailAsync(string email)
    {
        if (_provider == DatabaseFactoryExtension.DatabaseProvider.Oracle)
        {
            // Oracle: Usar función
            var result = await _context.Database
                .SqlQueryRaw<int>("SELECT FN_VALIDATE_EMAIL({0}) FROM DUAL", email)
                .FirstOrDefaultAsync();
            return result == 1;
        }
        else
        {
            // PostgreSQL: Usar función
            var result = await _context.Database
                .SqlQueryRaw<bool>("SELECT fn_validate_email({0})", email)
                .FirstOrDefaultAsync();
            return result;
        }
    }

    public async Task<int> CountRecordsByPatientAsync(int patientId)
    {
        if (_provider == DatabaseFactoryExtension.DatabaseProvider.Oracle)
        {
            // Oracle: Usar función
            var result = await _context.Database
                .SqlQueryRaw<int>("SELECT FN_COUNT_RECORDS_BY_PATIENT({0}) FROM DUAL", patientId)
                .FirstOrDefaultAsync();
            return result;
        }
        else
        {
            // PostgreSQL: Usar función
            var result = await _context.Database
                .SqlQueryRaw<int>("SELECT fn_count_records_by_patient({0})", patientId)
                .FirstOrDefaultAsync();
            return result;
        }
    }
}

