using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace CountriesApi.Presentation.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _environment;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger,
            IHostEnvironment environment
        )
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occured.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var statusCode = (int)HttpStatusCode.InternalServerError;
            var problem = new ProblemDetails();

            switch (ex)
            {
                default:
                    problem = new ProblemDetails
                    {
                        Title = "Internal Server Error",
                        Detail = _environment.IsDevelopment() ? ex.Message : "An error occured",
                        Status = statusCode,
                        Instance = context.Request.Path
                    };
                    break;
            }

            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsJsonAsync( problem );
        }
    }
}
