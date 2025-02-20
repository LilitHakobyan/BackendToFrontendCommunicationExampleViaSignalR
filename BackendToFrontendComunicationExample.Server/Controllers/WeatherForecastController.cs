using BackendToFrontendComunicationExample.Server.Models;
using BackendToFrontendComunicationExample.Server.SignalRConfigurations;
using Microsoft.AspNetCore.Mvc;

namespace BackendToFrontendComunicationExample.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;

        }

        [HttpGet]
        public IEnumerable<Entry> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new Entry()
            {
                StarTime = DateTime.Now.AddDays(index),
                Id = Random.Shared.Next(1, 6),
                Name = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();

        }
    }
}
