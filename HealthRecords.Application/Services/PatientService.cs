using HealthRecords.Application.Constants;
using HealthRecords.Application.Interfaces;
using HealthRecords.Application.Interfaces.Mappers;
using HealthRecords.Application.Requests.Patient;
using HealthRecords.Application.Responses.Patient;
using HealthRecords.Domain.Exceptions;
using HealthRecords.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthRecords.Application.Services;

/// <summary>
/// Servicio que maneja la lógica de negocio para pacientes
/// </summary>
public class PatientService : IPatientService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<PatientService> _logger;
    private readonly IPatientMapper _patientMapper;

    public PatientService(
        IUnitOfWork unitOfWork,
        ILogger<PatientService> logger,
        IPatientMapper patientMapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _patientMapper = patientMapper;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<PatientResponse>> GetAllAsync()
    {
        _logger.LogInformation("Event: {EventId} - Obteniendo todos los pacientes", LogEvents.PatientGetAll);
        var patients = await _unitOfWork.Patients.GetAllAsync();
        var count = patients.Count();
        _logger.LogInformation("Event: {EventId} - Se obtuvieron {PatientCount} pacientes exitosamente", LogEvents.PatientGetAll, count);
        return patients.Select(_patientMapper.MapToResponse);
    }

    /// <inheritdoc/>
    public async Task<PatientResponse?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Event: {EventId} - Obteniendo paciente con ID: {PatientId}", LogEvents.PatientGetById, id);
        var patient = await _unitOfWork.Patients.GetByIdAsync(id);
        if (patient == null)
        {
            _logger.LogWarning("Event: {EventId} - No se encontró paciente con ID: {PatientId}", LogEvents.PatientGetById, id);
            return null;
        }
        _logger.LogInformation("Event: {EventId} - Paciente obtenido exitosamente. ID: {PatientId}, Nombre: {PatientName}", LogEvents.PatientGetById, id, patient.Nombre);
        return _patientMapper.MapToResponse(patient);
    }

    /// <inheritdoc/>
    public async Task<PatientResponse?> GetByIdWithRecordsAsync(int id)
    {
        _logger.LogInformation("Event: {EventId} - Obteniendo paciente con historiales médicos. ID: {PatientId}", LogEvents.PatientGetByIdWithRecords, id);
        var patient = await _unitOfWork.Patients.GetByIdWithRecordsAsync(id);
        if (patient == null)
        {
            _logger.LogWarning("Event: {EventId} - No se encontró paciente con ID: {PatientId}", LogEvents.PatientGetByIdWithRecords, id);
            return null;
        }
        var recordsCount = patient.MedicalRecords?.Count() ?? 0;
        _logger.LogInformation("Event: {EventId} - Paciente obtenido exitosamente. ID: {PatientId}, Historiales: {RecordsCount}", LogEvents.PatientGetByIdWithRecords, id, recordsCount);
        return _patientMapper.MapToResponse(patient);
    }

    /// <inheritdoc/>
    public async Task<PatientResponse> CreateAsync(CreatePatientRequest request)
    {
        _logger.LogInformation("Event: {EventId} - Iniciando creación de paciente. Email: {Email}, Documento: {Documento}", LogEvents.PatientCreate, request.Email, request.Documento);
        
        // Validar que no exista un paciente con el mismo email
        if (await _unitOfWork.Patients.ExistsByEmailAsync(request.Email))
        {
            _logger.LogWarning("Event: {EventId} - Intento de crear paciente con email duplicado: {Email}", LogEvents.PatientDuplicateEmail, request.Email);
            throw new DuplicateEmailException(request.Email);
        }

        // Validar que no exista un paciente con el mismo documento
        if (await _unitOfWork.Patients.ExistsByDocumentoAsync(request.Documento))
        {
            _logger.LogWarning("Event: {EventId} - Intento de crear paciente con documento duplicado: {Documento}", LogEvents.PatientDuplicateDocument, request.Documento);
            throw new DuplicateDocumentException(request.Documento);
        }

        var patient = _patientMapper.MapToEntity(request);
        var createdPatient = await _unitOfWork.Patients.CreateAsync(patient);
        await _unitOfWork.SaveChangesAsync();
        _logger.LogInformation("Event: {EventId} - Paciente creado exitosamente. ID: {PatientId}, Nombre: {PatientName}, Email: {Email}", 
            LogEvents.PatientCreate, createdPatient.Id, createdPatient.Nombre, createdPatient.Email);
        return _patientMapper.MapToResponse(createdPatient);
    }

    /// <inheritdoc/>
    public async Task<PatientResponse?> UpdateAsync(int id, UpdatePatientRequest request)
    {
        _logger.LogInformation("Event: {EventId} - Iniciando actualización de paciente. ID: {PatientId}, Email: {Email}", LogEvents.PatientUpdate, id, request.Email);
        
        var patient = await _unitOfWork.Patients.GetByIdAsync(id);
        if (patient == null)
        {
            _logger.LogWarning("Event: {EventId} - No se encontró paciente para actualizar. ID: {PatientId}", LogEvents.PatientUpdate, id);
            return null;
        }

        // Validar que el email no esté en uso por otro paciente
        if (patient.Email != request.Email)
        {
            if (await _unitOfWork.Patients.ExistsByEmailAsync(request.Email))
            {
                _logger.LogWarning("Event: {EventId} - Intento de actualizar paciente con email duplicado. ID: {PatientId}, Email: {Email}", LogEvents.PatientDuplicateEmail, id, request.Email);
                throw new DuplicateEmailException(request.Email);
            }
        }

        _patientMapper.MapToEntity(request, patient);
        var updatedPatient = await _unitOfWork.Patients.UpdateAsync(patient);
        await _unitOfWork.SaveChangesAsync();
        _logger.LogInformation("Event: {EventId} - Paciente actualizado exitosamente. ID: {PatientId}, Nombre: {PatientName}", LogEvents.PatientUpdate, id, updatedPatient.Nombre);
        return _patientMapper.MapToResponse(updatedPatient);
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteAsync(int id)
    {
        _logger.LogInformation("Event: {EventId} - Iniciando eliminación de paciente. ID: {PatientId}", LogEvents.PatientDelete, id);
        
        var result = await _unitOfWork.Patients.DeleteAsync(id);
        if (result)
        {
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Event: {EventId} - Paciente eliminado exitosamente. ID: {PatientId}", LogEvents.PatientDelete, id);
        }
        else
        {
            _logger.LogWarning("Event: {EventId} - No se encontró paciente para eliminar. ID: {PatientId}", LogEvents.PatientDelete, id);
        }
        return result;
    }
}

