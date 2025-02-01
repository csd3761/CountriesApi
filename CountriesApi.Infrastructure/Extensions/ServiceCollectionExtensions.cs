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

namespace CountriesApi.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<DatabaseSettings>(
                configuration.GetSection("DatabaseSettings.ConnectionString"));

            services.AddDbContext<AppDbContext>((serviceProvider, options) =>
            {
                var databaseSettings = configuration.GetSection("DatabaseSettings.ConnectionString").Get<DatabaseSettings>();
                options.UseSqlServer("DatabaseSettings.ConnectionString");
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IReposistory<Country>, CountryRepository>();

            return services;
        }
    }
}
