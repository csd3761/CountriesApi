
using CountriesApi.Application.Common.Behaviors;
using CountriesApi.Application.Features.SecondLargest;
using CountriesApi.Application.Validators;
using CountriesApi.Infrastructure.Extensions;
using CountriesApi.Infrastructure.HealthChecks;
using CountriesApi.Presentation.Middleware;
using FluentValidation;
using MediatR;
using Serilog;

namespace CountriesApi.Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddInfrastructure(builder.Configuration);

            // Add MediatR
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetSecondLargestNumberQuery).Assembly));

            // Add Validators
            builder.Services.AddValidatorsFromAssemblyContaining<GetSecondLargestNumberQueryValidator>();

            //Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .CreateLogger();

            builder.Host.UseSerilog();

            // Register Behaviors
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

            // Add services to the container.
            builder.Services.AddHttpClient("CountriesClient", client =>
            {
                client.BaseAddress = new Uri("https://restcountries.com/v3.1/");  // TODO Use Options Pattern to use this one instead of hardcoded uri
            });

            // Add HealthChecks
            builder.Services.AddHealthChecks().AddCheck<DatabaseHealthCheck>("Database");

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Add the exception middleware
            app.UseMiddleware<ExceptionMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.MapHealthChecks("/health");

            app.Run();
        }
    }
}
