using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Net.Mime;

namespace Presentation.Middlewares;
public class GlobalExceptionHandling : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandling> _logger;
    private readonly ProblemDetailsFactory _problemDetailsFactory;
    public GlobalExceptionHandling(ILogger<GlobalExceptionHandling> logger,
        ProblemDetailsFactory problemDetailsFactory)
    {
        _logger = logger;
        _problemDetailsFactory = problemDetailsFactory;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Exception occurred: {message}", exception.Message);

        await HandleExceptionAsync(httpContext, exception);

        return true;
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var problemDetails = _problemDetailsFactory.CreateProblemDetails(
            httpContext: context,
           statusCode: StatusCodes.Status500InternalServerError, "Internal Server Error.",
           type: @"https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
          detail: exception.Message,
          instance: context.Request.Path);

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        context.Response.ContentType = MediaTypeNames.Application.Json;

        await context.Response.WriteAsJsonAsync(problemDetails);
    }
}
