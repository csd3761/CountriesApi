namespace CountriesApi.Application.Features.Countries.Dtos
{
    public class RestCountryDto
    {
        public NameDto Name { get; set; }
        public List<string> Capital { get; set; }
        public List<string> Borders { get; set; }
    }

    public class NameDto
    {
        public string Common { get; set; }
    }
}
