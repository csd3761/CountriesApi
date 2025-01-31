using MediatR;

namespace CountriesApi.Application.Features.Countries
{
    public class GetCountriesQuery : IRequest<List<CountryResponse>>
    {
    }

    public class CountryResponse
    {
        public string CommonName { get; set; }
        public string Capital { get; set; }
        public List<string> Borders { get; set; }
    }
}
