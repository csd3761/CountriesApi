﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using CountriesApi.Application.Features.Countries.Dtos;
using CountriesApi.Domain.Entites;
using CountriesApi.Domain.Interfaces;
using MediatR;
using StackExchange.Redis;

namespace CountriesApi.Application.Features.Countries.Commands
{
    public class FetchAndSaveCountriesCommandHandler(
    IHttpClientFactory httpClientFactory,
    IReposistory<Country> countryRepository,
    IUnitOfWork unitOfWork,
    IDatabase redis) : IRequestHandler<FetchAndSaveCountriesCommand>
    {
        public async Task Handle(
            FetchAndSaveCountriesCommand request,
            CancellationToken cancellationToken)
        {
            var client = httpClientFactory.CreateClient("CountriesClient");
            var response = await client.GetAsync("all?fields=name,capital,borders", cancellationToken);
            response.EnsureSuccessStatusCode();

            var countries = await response.Content.ReadFromJsonAsync<List<RestCountryDto>>(cancellationToken);

            var countryEntities = countries.Select(c => new Country
            {
                CommonName = c.Name.Common,
                Capital = c.Capital?.FirstOrDefault() ?? "No Capital",
                Borders = c.Borders ?? new List<string>()
            }).ToList();

            // Save to database
            await countryRepository.AddRangeAsync(countryEntities);
            await unitOfWork.CommitAsync(cancellationToken);

            // invalidate cache after saving new data
            await redis.KeyDeleteAsync("CountriesCache");
        }
    }
}
