using HealthRecords.Application.Interfaces;
using HealthRecords.Application.Requests.Patient;
using HealthRecords.Application.Responses;
using HealthRecords.Application.Responses.Patient;
using HealthRecords.API.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace HealthRecords.API.Controllers;

/// <summary>
/// Controller para gestión de pacientes
/// </summary>
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
public class PatientsController : BaseController
{
    private readonly IPatientService _patientService;

    public PatientsController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    /// <summary>
    /// Obtiene todos los pacientes
    /// </summary>
    /// <returns>Lista de pacientes</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ResponseDto<IEnumerable<PatientResponse>>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ResponseDto<IEnumerable<PatientResponse>>>> GetAll()
    {
        var patients = await _patientService.GetAllAsync();
        return Success(patients, "Pacientes obtenidos exitosamente");
    }

    /// <summary>
    /// Obtiene un paciente por su ID
    /// </summary>
    /// <param name="id">ID del paciente</param>
    /// <returns>Paciente encontrado</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseDto<PatientResponse>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ResponseDto<object>), (int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<ResponseDto<PatientResponse>>> GetById(int id)
    {
        var patient = await _patientService.GetByIdAsync(id);
        if (patient == null)
        {
            return NotFoundResponse<PatientResponse>($"No se encontró un paciente con ID: {id}");
        }

        return Success(patient, "Paciente obtenido exitosamente");
    }

    /// <summary>
    /// Obtiene un paciente con sus historiales médicos
    /// </summary>
    /// <param name="id">ID del paciente</param>
    /// <returns>Paciente con historiales médicos</returns>
    [HttpGet("{id}/records")]
    [ProducesResponseType(typeof(ResponseDto<PatientResponse>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ResponseDto<object>), (int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<ResponseDto<PatientResponse>>> GetByIdWithRecords(int id)
    {
        var patient = await _patientService.GetByIdWithRecordsAsync(id);
        if (patient == null)
        {
            return NotFoundResponse<PatientResponse>($"No se encontró un paciente con ID: {id}");
        }

        return Success(patient, "Paciente con historiales obtenido exitosamente");
    }

    /// <summary>
    /// Crea un nuevo paciente
    /// </summary>
    /// <param name="request">Datos del paciente a crear</param>
    /// <returns>Paciente creado</returns>
    [HttpPost]
    [ValidateModel]
    [ProducesResponseType(typeof(ResponseDto<PatientResponse>), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ResponseDto<object>), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<ResponseDto<PatientResponse>>> Create([FromBody] CreatePatientRequest request)
    {
        var patient = await _patientService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = patient.Id },
            ResponseDto<PatientResponse>.SuccessResponse(patient, "Paciente creado exitosamente"));
    }

    /// <summary>
    /// Actualiza un paciente existente
    /// </summary>
    /// <param name="id">ID del paciente</param>
    /// <param name="request">Datos actualizados del paciente</param>
    /// <returns>Paciente actualizado</returns>
    [HttpPut("{id}")]
    [ValidateModel]
    [ProducesResponseType(typeof(ResponseDto<PatientResponse>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ResponseDto<object>), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ResponseDto<object>), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<ResponseDto<PatientResponse>>> Update(int id, [FromBody] UpdatePatientRequest request)
    {
        var patient = await _patientService.UpdateAsync(id, request);
        if (patient == null)
        {
            return NotFoundResponse<PatientResponse>($"No se encontró un paciente con ID: {id}");
        }

        return Success(patient, "Paciente actualizado exitosamente");
    }

    /// <summary>
    /// Elimina un paciente
    /// </summary>
    /// <param name="id">ID del paciente a eliminar</param>
    /// <returns>Resultado de la eliminación</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ResponseDto<object>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ResponseDto<object>), (int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<ResponseDto<object>>> Delete(int id)
    {
        var result = await _patientService.DeleteAsync(id);
        if (!result)
        {
            return NotFoundResponse<object>($"No se encontró un paciente con ID: {id}");
        }

        return Success<object>(new { }, "Paciente eliminado exitosamente");
    }
}

