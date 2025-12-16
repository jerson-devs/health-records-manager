using HealthRecords.API.Extensions;
using HealthRecords.Application.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace HealthRecords.API.Controllers;

/// <summary>
/// Controlador base que proporciona métodos helper comunes para todos los controladores
/// </summary>
[ApiController]
[Produces("application/json")]
public abstract class BaseController : ControllerBase
{
    /// <summary>
    /// Crea una respuesta exitosa
    /// </summary>
    protected ActionResult<ResponseDto<T>> Success<T>(T data, string message = "Operación exitosa", HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        return StatusCode((int)statusCode, ResponseDto<T>.SuccessResponse(data, message));
    }

    /// <summary>
    /// Crea una respuesta de error
    /// </summary>
    protected ActionResult<ResponseDto<T>> Error<T>(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest, List<string>? errors = null)
    {
        return StatusCode((int)statusCode, ResponseDto<T>.ErrorResponse(message, statusCode, errors));
    }

    /// <summary>
    /// Crea una respuesta de error con los errores del ModelState
    /// </summary>
    protected ActionResult<ResponseDto<T>> ValidationError<T>(string message = "Datos de entrada inválidos")
    {
        var errors = this.GetModelStateErrors();
        return BadRequest(ResponseDto<T>.ErrorResponse(message, HttpStatusCode.BadRequest, errors));
    }

    /// <summary>
    /// Crea una respuesta de recurso no encontrado
    /// </summary>
    protected ActionResult<ResponseDto<T>> NotFoundResponse<T>(string message)
    {
        return NotFound(ResponseDto<T>.ErrorResponse(message, HttpStatusCode.NotFound));
    }
}
