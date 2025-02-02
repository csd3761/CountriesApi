using CountriesApi.Application.Common.Interfaces;
using MediatR;

namespace CountriesApi.Application.Features.Countries
{
    public class GetCountriesQuery : IRequest<List<CountryResponse>>, ICacheableQuery
    {
        public string CacheKey => "CountriesCache";
    }

    public class CountryResponse
    {
        public string CommonName { get; set; }
        public string Capital { get; set; }
        public List<string> Borders { get; set; }
    }
}
