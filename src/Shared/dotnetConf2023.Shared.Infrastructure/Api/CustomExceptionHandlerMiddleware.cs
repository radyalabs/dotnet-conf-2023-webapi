﻿using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using dotnetConf2023.Shared.Abstraction.Primitives;
using dotnetConf2023.Shared.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace dotnetConf2023.Shared.Infrastructure.Api;

internal class CustomExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<CustomExceptionHandlerMiddleware> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomExceptionHandlerMiddleware"/> class.
    /// </summary>
    /// <param name="next">The delegate pointing to the next middleware in the chain.</param>
    /// <param name="logger">The logger.</param>
    public CustomExceptionHandlerMiddleware(RequestDelegate next, ILogger<CustomExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Invokes the exception handler middleware with the specified <see cref="HttpContext"/>.
    /// </summary>
    /// <param name="httpContext">The HTTP httpContext.</param>
    /// <returns>The task that can be awaited by the next middleware.</returns>
    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occurred: {Message}", ex.Message);

            await HandleExceptionAsync(httpContext, ex);
        }
    }

    /// <summary>
    /// Handles the specified <see cref="Exception"/> for the specified <see cref="HttpContext"/>.
    /// </summary>
    /// <param name="httpContext">The HTTP httpContext.</param>
    /// <param name="exception">The exception.</param>
    /// <returns>The HTTP response that is modified based on the exception.</returns>
    private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        var (httpStatusCode, errors) = GetHttpStatusCodeAndErrors(exception);

        httpContext.Response.ContentType = "application/json";

        httpContext.Response.StatusCode = (int)httpStatusCode;

        var serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        var response = JsonSerializer.Serialize(new ApiErrorResponse(errors), serializerOptions);

        await httpContext.Response.WriteAsync(response);
    }

    /// <summary>
    /// Extracts the HTTP status code and a collection of errors based on the specified exception.
    /// </summary>
    /// <param name="exception">The exception.</param>
    /// <returns>The HTTP status code and a collection of errors based on the specified exception.</returns>
    private static (HttpStatusCode HttpStatusCode, IReadOnlyCollection<Error> Errors) GetHttpStatusCodeAndErrors(
        Exception exception) =>
        exception switch
        {
            ValidationException validationException => (HttpStatusCode.BadRequest, validationException.Errors),
            _ => (HttpStatusCode.InternalServerError, new[] { Errors.ServerError })
        };
}