using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace HealthRecords.API.Extensions;

/// <summary>
/// Extensiones para ControllerBase que proporcionan métodos helper comunes
/// </summary>
public static class ControllerExtensions
{
    /// <summary>
    /// Obtiene todos los errores de validación del ModelState
    /// </summary>
    /// <param name="controller">El controlador del cual obtener los errores</param>
    /// <returns>Lista de mensajes de error</returns>
    public static List<string> GetModelStateErrors(this ControllerBase controller)
    {
        return controller.ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();
    }
}
