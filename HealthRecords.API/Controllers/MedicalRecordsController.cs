using HealthRecords.Application.Interfaces;
using HealthRecords.Application.Requests.MedicalRecord;
using HealthRecords.Application.Responses;
using HealthRecords.Application.Responses.MedicalRecord;
using HealthRecords.API.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace HealthRecords.API.Controllers;

/// <summary>
/// Controller para gestión de historiales médicos
/// </summary>
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
public class MedicalRecordsController : BaseController
{
    private readonly IMedicalRecordService _medicalRecordService;

    public MedicalRecordsController(IMedicalRecordService medicalRecordService)
    {
        _medicalRecordService = medicalRecordService;
    }

    /// <summary>
    /// Obtiene todos los historiales médicos
    /// </summary>
    /// <returns>Lista de historiales médicos</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ResponseDto<IEnumerable<MedicalRecordResponse>>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ResponseDto<IEnumerable<MedicalRecordResponse>>>> GetAll()
    {
        var records = await _medicalRecordService.GetAllAsync();
        return Success(records, "Historiales médicos obtenidos exitosamente");
    }

    /// <summary>
    /// Obtiene un historial médico por su ID
    /// </summary>
    /// <param name="id">ID del historial médico</param>
    /// <returns>Historial médico encontrado</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseDto<MedicalRecordResponse>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ResponseDto<object>), (int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<ResponseDto<MedicalRecordResponse>>> GetById(int id)
    {
        var record = await _medicalRecordService.GetByIdAsync(id);
        if (record == null)
        {
            return NotFoundResponse<MedicalRecordResponse>($"No se encontró un historial médico con ID: {id}");
        }

        return Success(record, "Historial médico obtenido exitosamente");
    }

    /// <summary>
    /// Crea un nuevo historial médico
    /// </summary>
    /// <param name="request">Datos del historial médico a crear</param>
    /// <returns>Historial médico creado</returns>
    [HttpPost]
    [ValidateModel]
    [ProducesResponseType(typeof(ResponseDto<MedicalRecordResponse>), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ResponseDto<object>), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<ResponseDto<MedicalRecordResponse>>> Create([FromBody] CreateMedicalRecordRequest request)
    {
        var record = await _medicalRecordService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = record.Id },
            ResponseDto<MedicalRecordResponse>.SuccessResponse(record, "Historial médico creado exitosamente"));
    }

    /// <summary>
    /// Actualiza un historial médico existente
    /// </summary>
    /// <param name="id">ID del historial médico</param>
    /// <param name="request">Datos actualizados del historial médico</param>
    /// <returns>Historial médico actualizado</returns>
    [HttpPut("{id}")]
    [ValidateModel]
    [ProducesResponseType(typeof(ResponseDto<MedicalRecordResponse>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ResponseDto<object>), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ResponseDto<object>), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<ResponseDto<MedicalRecordResponse>>> Update(int id, [FromBody] UpdateMedicalRecordRequest request)
    {

        var record = await _medicalRecordService.UpdateAsync(id, request);
        if (record == null)
        {
            return NotFoundResponse<MedicalRecordResponse>($"No se encontró un historial médico con ID: {id}");
        }

        return Success(record, "Historial médico actualizado exitosamente");
    }

    /// <summary>
    /// Elimina un historial médico
    /// </summary>
    /// <param name="id">ID del historial médico a eliminar</param>
    /// <returns>Resultado de la eliminación</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ResponseDto<object>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ResponseDto<object>), (int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<ResponseDto<object>>> Delete(int id)
    {
        var result = await _medicalRecordService.DeleteAsync(id);
        if (!result)
        {
            return NotFoundResponse<object>($"No se encontró un historial médico con ID: {id}");
        }

        return Success<object>(new { }, "Historial médico eliminado exitosamente");
    }
}

