using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using StackExchange.Redis;

namespace CountriesApi.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthChecksController : ControllerBase
    {
        private readonly HealthCheckService _healthCheckService;
        private readonly IConnectionMultiplexer _redis;

        public HealthChecksController(
            HealthCheckService healthCheckService,
            IConnectionMultiplexer redis)
        {
            _healthCheckService = healthCheckService;
            _redis = redis;
        }

        [HttpGet("CountriesDb")]
        public async Task<IActionResult> CheckHealthCountriesDb()
        {
            var report = await _healthCheckService.CheckHealthAsync();
            return report.Status == HealthStatus.Healthy
                ? Ok("Database is healthy")
                : StatusCode(503, "Database is unhealthy");
        }

        [HttpGet("RedisCache")]
        public async Task<IActionResult> CheckHealthRedisCache()
        {
            try
            {
                var db = _redis.GetDatabase();
                await db.PingAsync();
                return Ok("Redis is healthy");
            }
            catch
            {
                return StatusCode(503, "Redis is unhealthy");
            }
        }
    }
}
