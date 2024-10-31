﻿using Domain.Exceptions;
using System.Net;
using System.Text.Json;
namespace API.Middleware
{
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");
                await HandleExceptionAsync(context, ex); 
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = HttpStatusCode.InternalServerError;
            var message = exception is CustomException ? exception.Message : "An unexpected error occurred. Please try again later.";            

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var response = new { message };
            var jsonResponse = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(jsonResponse);
        }
    }

}