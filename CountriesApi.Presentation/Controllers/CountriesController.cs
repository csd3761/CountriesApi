using CountriesApi.Application.Features.Countries;
using CountriesApi.Application.Features.Countries.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CountriesApi.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CountriesController(IMediator mediator) 
        { 
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetCountries()
        {
            var result = await _mediator.Send(new GetCountriesQuery());
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAndSaveCountriesOnDB()
        {
            var existingCountries = await _mediator.Send(new GetCountriesQuery());

            if (existingCountries.Any())
            {
                return Ok(existingCountries);
            }

            await _mediator.Send(new FetchAndSaveCountriesCommand());

            var updatedCountries = await _mediator.Send(new GetCountriesQuery());
            return Ok(updatedCountries);
        }
    }
}
