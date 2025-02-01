using System.Net.Http.Json;
using CountriesApi.Application.Features.Countries.Dtos;
using CountriesApi.Domain.Entites;
using CountriesApi.Domain.Interfaces;
using MediatR;

namespace CountriesApi.Application.Features.Countries
{
    public class GetCountriesQueryHandler(
    IReposistory<Country> countryRepository)
    : IRequestHandler<GetCountriesQuery, List<CountryResponse>>
    {
        public async Task<List<CountryResponse>> Handle(
            GetCountriesQuery request,
            CancellationToken cancellationToken)
        {
            var countries = await countryRepository.GetAllAsync(cancellationToken);
            return countries.Select(c => new CountryResponse
            {
                CommonName = c.CommonName,
                Capital = c.Capital,
                Borders = c.Borders
            }).ToList();
        }
    }
}
