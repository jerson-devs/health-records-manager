using HealthRecords.Application.Constants;
using HealthRecords.Application.Responses;
using HealthRecords.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace HealthRecords.Infrastructure.Middleware;

/// <summary>
/// Middleware para manejo global de excepciones
/// </summary>
public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Event: {EventId} - Error no manejado capturado por middleware. Path: {Path}, Method: {Method}, Error: {ErrorMessage}", 
                LogEvents.UnhandledException, context.Request.Path, context.Request.Method, ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        var response = context.Response;
        
        var isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

        var errorResponse = new ResponseDto<object>(
            HttpStatusCode.InternalServerError,
            "Ocurrió un error al procesar la solicitud",
            null,
            new List<string> { exception.Message }
        );

        switch (exception)
        {
            case ArgumentNullException:
            case ArgumentException:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.StatusCode = HttpStatusCode.BadRequest;
                errorResponse.Message = "Parámetros inválidos";
                errorResponse.Errors = new List<string> { exception.Message };
                break;

            case DomainException domainEx:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.StatusCode = HttpStatusCode.BadRequest;
                errorResponse.Message = domainEx.Message;
                errorResponse.Errors = new List<string> { domainEx.Message };
                break;

            case InvalidOperationException:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.StatusCode = HttpStatusCode.BadRequest;
                errorResponse.Message = exception.Message;
                errorResponse.Errors = new List<string> { exception.Message };
                break;

            case UnauthorizedAccessException:
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
                errorResponse.StatusCode = HttpStatusCode.Unauthorized;
                errorResponse.Message = "No autorizado";
                errorResponse.Errors = new List<string> { exception.Message };
                break;

            case DbUpdateException dbEx:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.StatusCode = HttpStatusCode.BadRequest;
                errorResponse.Message = "Error al guardar en la base de datos";
                
                var errors = new List<string> { dbEx.Message };
                
                if (dbEx.InnerException != null)
                {
                    errors.Add($"Error interno: {dbEx.InnerException.Message}");
                }
                
                if (isDevelopment && dbEx.InnerException != null)
                {
                    errors.Add($"Stack trace: {dbEx.InnerException.StackTrace?.Split('\n')[0]}");
                }
                
                errorResponse.Errors = errors;
                break;

            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                errorResponse.StatusCode = HttpStatusCode.InternalServerError;
                errorResponse.Message = "Error interno del servidor";
                
                // En desarrollo, mostrar más detalles del error
                if (isDevelopment)
                {
                    var errorList = new List<string> { exception.Message };
                    
                    if (exception.InnerException != null)
                    {
                        errorList.Add($"Error interno: {exception.InnerException.Message}");
                    }
                    
                    // Solo agregar la primera línea del stack trace para no hacer la respuesta muy larga
                    if (!string.IsNullOrEmpty(exception.StackTrace))
                    {
                        var firstLine = exception.StackTrace.Split('\n')[0];
                        errorList.Add($"Ubicación: {firstLine}");
                    }
                    
                    errorResponse.Errors = errorList;
                }
                else
                {
                    errorResponse.Errors = new List<string> { "Ha ocurrido un error inesperado" };
                }
                break;
        }

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var jsonResponse = JsonSerializer.Serialize(errorResponse, options);
        await response.WriteAsync(jsonResponse);
    }
}

