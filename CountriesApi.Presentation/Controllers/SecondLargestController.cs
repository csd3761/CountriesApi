using CountriesApi.Application.Features.SecondLargest;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CountriesApi.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SecondLargestController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SecondLargestController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GetSecondLargestNumberQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
