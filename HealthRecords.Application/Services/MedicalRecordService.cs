using HealthRecords.Application.Constants;
using HealthRecords.Application.Interfaces;
using HealthRecords.Application.Interfaces.Mappers;
using HealthRecords.Application.Requests.MedicalRecord;
using HealthRecords.Application.Responses.MedicalRecord;
using HealthRecords.Domain.Exceptions;
using HealthRecords.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthRecords.Application.Services;

/// <summary>
/// Servicio que maneja la lógica de negocio para historiales médicos
/// </summary>
public class MedicalRecordService : IMedicalRecordService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<MedicalRecordService> _logger;
    private readonly IMedicalRecordMapper _medicalRecordMapper;

    public MedicalRecordService(
        IUnitOfWork unitOfWork,
        ILogger<MedicalRecordService> logger,
        IMedicalRecordMapper medicalRecordMapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _medicalRecordMapper = medicalRecordMapper;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<MedicalRecordResponse>> GetAllAsync()
    {
        _logger.LogInformation("Event: {EventId} - Obteniendo todos los historiales médicos", LogEvents.MedicalRecordGetAll);
        var records = await _unitOfWork.MedicalRecords.GetAllAsync();
        var count = records.Count();
        _logger.LogInformation("Event: {EventId} - Se obtuvieron {RecordCount} historiales médicos exitosamente", LogEvents.MedicalRecordGetAll, count);
        return records.Select(_medicalRecordMapper.MapToResponse);
    }

    /// <inheritdoc/>
    public async Task<MedicalRecordResponse?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Event: {EventId} - Obteniendo historial médico con ID: {RecordId}", LogEvents.MedicalRecordGetById, id);
        var record = await _unitOfWork.MedicalRecords.GetByIdAsync(id);
        if (record == null)
        {
            _logger.LogWarning("Event: {EventId} - No se encontró historial médico con ID: {RecordId}", LogEvents.MedicalRecordGetById, id);
            return null;
        }
        _logger.LogInformation("Event: {EventId} - Historial médico obtenido exitosamente. ID: {RecordId}, Paciente: {PatientId}", LogEvents.MedicalRecordGetById, id, record.PatientId);
        return _medicalRecordMapper.MapToResponse(record);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<MedicalRecordResponse>> GetByPatientIdAsync(int patientId)
    {
        _logger.LogInformation("Event: {EventId} - Obteniendo historiales médicos del paciente. ID: {PatientId}", LogEvents.MedicalRecordGetByPatientId, patientId);
        
        // Validar que el paciente exista
        var patient = await _unitOfWork.Patients.GetByIdAsync(patientId);
        if (patient == null)
        {
            _logger.LogWarning("Event: {EventId} - Paciente no encontrado al obtener historiales. ID: {PatientId}", LogEvents.MedicalRecordPatientNotFound, patientId);
            throw new PatientNotFoundException(patientId);
        }

        var records = await _unitOfWork.MedicalRecords.GetByPatientIdAsync(patientId);
        var count = records.Count();
        _logger.LogInformation("Event: {EventId} - Se obtuvieron {RecordCount} historiales médicos del paciente. ID: {PatientId}", LogEvents.MedicalRecordGetByPatientId, count, patientId);
        return records.Select(_medicalRecordMapper.MapToResponse);
    }

    /// <inheritdoc/>
    public async Task<MedicalRecordResponse> CreateAsync(CreateMedicalRecordRequest request)
    {
        _logger.LogInformation("Event: {EventId} - Iniciando creación de historial médico. Paciente: {PatientId}, Fecha: {Fecha}", LogEvents.MedicalRecordCreate, request.PatientId, request.Fecha);
        
        // Validar que el paciente exista
        var patient = await _unitOfWork.Patients.GetByIdAsync(request.PatientId);
        if (patient == null)
        {
            _logger.LogWarning("Event: {EventId} - Paciente no encontrado al crear historial médico. ID: {PatientId}", LogEvents.MedicalRecordPatientNotFound, request.PatientId);
            throw new PatientNotFoundForMedicalRecordException(request.PatientId);
        }

        var medicalRecord = _medicalRecordMapper.MapToEntity(request);
        var createdRecord = await _unitOfWork.MedicalRecords.CreateAsync(medicalRecord);
        await _unitOfWork.SaveChangesAsync();
        _logger.LogInformation("Event: {EventId} - Historial médico creado exitosamente. ID: {RecordId}, Paciente: {PatientId}, Diagnóstico: {Diagnostico}", 
            LogEvents.MedicalRecordCreate, createdRecord.Id, request.PatientId, createdRecord.Diagnostico);
        return _medicalRecordMapper.MapToResponse(createdRecord);
    }

    /// <inheritdoc/>
    public async Task<MedicalRecordResponse?> UpdateAsync(int id, UpdateMedicalRecordRequest request)
    {
        _logger.LogInformation("Event: {EventId} - Iniciando actualización de historial médico. ID: {RecordId}", LogEvents.MedicalRecordUpdate, id);
        
        var record = await _unitOfWork.MedicalRecords.GetByIdAsync(id);
        if (record == null)
        {
            _logger.LogWarning("Event: {EventId} - No se encontró historial médico para actualizar. ID: {RecordId}", LogEvents.MedicalRecordUpdate, id);
            return null;
        }

        _medicalRecordMapper.MapToEntity(request, record);
        var updatedRecord = await _unitOfWork.MedicalRecords.UpdateAsync(record);
        await _unitOfWork.SaveChangesAsync();
        _logger.LogInformation("Event: {EventId} - Historial médico actualizado exitosamente. ID: {RecordId}, Diagnóstico: {Diagnostico}", LogEvents.MedicalRecordUpdate, id, updatedRecord.Diagnostico);
        return _medicalRecordMapper.MapToResponse(updatedRecord);
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteAsync(int id)
    {
        _logger.LogInformation("Event: {EventId} - Iniciando eliminación de historial médico. ID: {RecordId}", LogEvents.MedicalRecordDelete, id);
        
        var result = await _unitOfWork.MedicalRecords.DeleteAsync(id);
        if (result)
        {
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Event: {EventId} - Historial médico eliminado exitosamente. ID: {RecordId}", LogEvents.MedicalRecordDelete, id);
        }
        else
        {
            _logger.LogWarning("Event: {EventId} - No se encontró historial médico para eliminar. ID: {RecordId}", LogEvents.MedicalRecordDelete, id);
        }
        return result;
    }
}

