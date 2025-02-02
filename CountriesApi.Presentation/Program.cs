
using CountriesApi.Application.Common.Behaviors;
using CountriesApi.Application.Features.SecondLargest;
using CountriesApi.Application.Validators;
using CountriesApi.Domain.Configuration;
using CountriesApi.Infrastructure.Extensions;
using CountriesApi.Infrastructure.HealthChecks;
using CountriesApi.Presentation.Middleware;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Serilog;
using StackExchange.Redis;

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
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

            // Add Validators
            builder.Services.AddValidatorsFromAssemblyContaining<GetSecondLargestNumberQueryValidator>();

            //Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .CreateLogger();

            builder.Host.UseSerilog();

            // Register Behaviors
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));

            // Add services to the container.
            builder.Services.AddHttpClient("CountriesClient", client =>
            {
                client.BaseAddress = new Uri("https://restcountries.com/v3.1/");  // TODO Use Options Pattern to use this one instead of hardcoded uri
            });

            builder.Services.Configure<RedisSettings>(
                builder.Configuration.GetSection(RedisSettings.SectionName));

            builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var configOptions = new ConfigurationOptions
                {
                    EndPoints = { { "redis-15021.c251.east-us-mz.azure.redns.redis-cloud.com", 15021 } },
                    User = "default",
                    Password = "dwdqNKmXlmSWm64n2lynVKndOKFV0DBv",

                    // Add essential resilience settings
                    AbortOnConnectFail = false,
                    ConnectTimeout = 5000,  // 5 seconds
                    ReconnectRetryPolicy = new LinearRetry(2000),  // Retry every 2 seconds
                    //Ssl = true  // Most cloud providers require SSL
                };

                try
                {
                    var connection = ConnectionMultiplexer.Connect(configOptions);

                    // Add connection validation
                    if (!connection.IsConnected)
                    {
                        throw new InvalidOperationException("Failed to connect to Redis");
                    }

                    return connection;
                }
                catch (Exception ex)
                {
                    // Log error and rethrow
                    var logger = sp.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Redis connection failed");
                    throw;
                }
            });

            builder.Services.AddScoped<IDatabase>(sp =>
                sp.GetRequiredService<IConnectionMultiplexer>().GetDatabase());

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

            app.Run();
        }
    }
}
