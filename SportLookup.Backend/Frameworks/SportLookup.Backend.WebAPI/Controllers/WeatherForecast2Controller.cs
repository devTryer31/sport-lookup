using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SportLookup.Backend.WebAPI.Controllers
{
    [Authorize]
    [ApiVersion("2.0")]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    public class WeatherForecast2Controller : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecast2Controller> _logger;

        public WeatherForecast2Controller(ILogger<WeatherForecast2Controller> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Выдача погоды.
        /// </summary>
        /// <returns>Погода</returns>
        [HttpGet(Name = "GetWeatherForecast")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IEnumerable<WeatherForecast> Get()
        {
            var claims = HttpContext.User.Claims.ToList();

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}