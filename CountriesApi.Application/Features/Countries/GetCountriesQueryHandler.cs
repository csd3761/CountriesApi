using System.Net.Http.Json;
using CountriesApi.Application.Features.Countries.Dtos;
using MediatR;

namespace CountriesApi.Application.Features.Countries
{
    public class GetCountriesQueryHandler(
        IHttpClientFactory httpClientFactory
    ) : IRequestHandler<GetCountriesQuery, List<CountryResponse>>
    {
        public async Task<List<CountryResponse>> Handle(GetCountriesQuery request, CancellationToken cancellationToken)
        {
            var client = httpClientFactory.CreateClient("CountriesClient");
            var response = await client.GetAsync("all?fields=name,capital,borders", cancellationToken);

            response.EnsureSuccessStatusCode();

            var countries = await response.Content.ReadFromJsonAsync<List<RestCountryDto>>(cancellationToken);

            return countries.Select(c => new CountryResponse
            {
                CommonName = c.Name.Common,
                Capital = c.Capital?.FirstOrDefault(),
                Borders = c.Borders ?? new List<string>()
            }).ToList();
        }
    }
}
