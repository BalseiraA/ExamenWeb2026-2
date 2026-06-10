using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TicketSystem.Application.Common.Exceptions;

namespace TicketSystem.API;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        var (statusCode, title) = exception switch
        {
            ValidationException => (StatusCodes.Status400BadRequest, "Error de validación"),
            BusinessRuleException => (StatusCodes.Status409Conflict, "Regla de negocio violada"),
            NotFoundException => (StatusCodes.Status404NotFound, "Recurso no encontrado"),
            UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "No autorizado"),
            _ => (StatusCodes.Status500InternalServerError, "Error interno del servidor")
        };

        var details = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = exception is ValidationException vex
                ? string.Join(" | ", vex.Errors.Select(e => e.ErrorMessage))
                : exception.Message
        };

        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsJsonAsync(details, cancellationToken);
        return true;
    }
}
