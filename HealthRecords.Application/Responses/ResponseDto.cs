using System;
using System.Collections.Generic;
using System.Net;

namespace HealthRecords.Application.Responses;

/// <summary>
/// DTO genérico para respuestas de la API
/// </summary>
/// <typeparam name="T">Tipo de datos de la respuesta</typeparam>
public class ResponseDto<T>
{
    /// <summary>
    /// Código de estado HTTP
    /// </summary>
    public HttpStatusCode StatusCode { get; set; }

    /// <summary>
    /// Mensaje de la respuesta
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Datos de la respuesta
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// Lista de errores (si los hay)
    /// </summary>
    public List<string>? Errors { get; set; }

    /// <summary>
    /// Indica si la operación fue exitosa
    /// </summary>
    public bool Success => (int)StatusCode >= 200 && (int)StatusCode < 300;

    public ResponseDto()
    {
    }

    public ResponseDto(HttpStatusCode statusCode, string message, T? data = default, List<string>? errors = null)
    {
        StatusCode = statusCode;
        Message = message;
        Data = data;
        Errors = errors;
    }

    /// <summary>
    /// Crea una respuesta exitosa
    /// </summary>
    public static ResponseDto<T> SuccessResponse(T data, string message = "Operación exitosa")
    {
        return new ResponseDto<T>(HttpStatusCode.OK, message, data);
    }

    /// <summary>
    /// Crea una respuesta de error
    /// </summary>
    public static ResponseDto<T> ErrorResponse(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest, List<string>? errors = null)
    {
        return new ResponseDto<T>(statusCode, message, default, errors);
    }
}

