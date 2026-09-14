using System.Net;
using System.Text.Json;
using FluentValidation;
using TaskFlow.Application.Auth.Commands.Login;
using TaskFlow.Domain.Exceptions;

namespace TaskFlow.Api.Middleware;

/// <summary>
/// Central place that converts exceptions thrown anywhere in the pipeline
/// into consistent, machine-readable JSON error responses.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, title, errors) = exception switch
        {
            ValidationException validationException => (
                HttpStatusCode.BadRequest,
                "Erro de validação.",
                validationException.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray()) as object),

            NotFoundException notFoundException => (
                HttpStatusCode.NotFound, notFoundException.Message, null),

            ConflictException conflictException => (
                HttpStatusCode.Conflict, conflictException.Message, null),

            ForbiddenAccessException forbiddenException => (
                HttpStatusCode.Forbidden, forbiddenException.Message, null),

            UnauthorizedException unauthorizedException => (
                HttpStatusCode.Unauthorized, unauthorizedException.Message, null),

            _ => (HttpStatusCode.InternalServerError, "Ocorreu um erro inesperado.", null)
        };

        if (statusCode == HttpStatusCode.InternalServerError)
        {
            _logger.LogError(exception, "Unhandled exception occurred");
        }

        context.Response.StatusCode = (int)statusCode;

        var payload = new
        {
            status = (int)statusCode,
            title,
            errors
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(payload, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        }));
    }
}
