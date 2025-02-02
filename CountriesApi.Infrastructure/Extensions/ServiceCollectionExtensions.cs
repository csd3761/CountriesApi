using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CountriesApi.Domain.Configuration;
using CountriesApi.Domain.Entites;
using CountriesApi.Domain.Interfaces;
using CountriesApi.Infrastructure.Data;
using CountriesApi.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace CountriesApi.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<DatabaseSettings>(
                configuration.GetSection(DatabaseSettings.SectionName));

            services.AddDbContext<AppDbContext>((_, options) =>
            options.UseSqlServer(configuration.GetSection(DatabaseSettings.SectionName).Get<DatabaseSettings>().ConnectionString));

            //services.Configure<RedisSettings>(
            //    configuration.GetSection("RedisSettings"));

            //services.AddSingleton<IConnectionMultiplexer>(sp =>
            //{
            //    var redisSettings = sp.GetRequiredService<IOptions<RedisSettings>>().Value;
            //    var configOptions = ConfigurationOptions.Parse(redisSettings.ConnectionString, true);
            //    configOptions.AbortOnConnectFail = false;
            //    return ConnectionMultiplexer.Connect(configOptions);
            //});

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IReposistory<Country>, CountryRepository>();

            return services;
        }
    }
}
