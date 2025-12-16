using HealthRecords.Application.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace HealthRecords.API.Filters;

/// <summary>
/// Action Filter para validar automáticamente el ModelState
/// </summary>
public class ValidateModelAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            var response = ResponseDto<object>.ErrorResponse(
                "Datos de entrada inválidos",
                HttpStatusCode.BadRequest,
                errors
            );

            context.Result = new BadRequestObjectResult(response);
        }
    }
}
