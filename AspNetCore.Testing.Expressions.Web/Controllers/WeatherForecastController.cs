using AspNetCore.Testing.Expressions.Web.Models;
using AspNetCore.Testing.Expressions.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Testing.Expressions.Web.Controllers;

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

    private static WeatherForecast[] GetForecast()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
    
    [HttpGet]
    public ActionResult<IEnumerable<WeatherForecast>> Get()
    {
        return GetForecast();
    }

    [HttpGet(nameof(Get))]
    public ActionResult<IEnumerable<WeatherForecast>> Get(
        [FromServices] IService service,
        [FromRoute] int id,
        [FromQuery] string q1,
        [FromQuery] SomeFilters filters
        ) => Get();

    [HttpPost]
    public IActionResult Post() => Ok();
    
    [HttpPut]
    public IActionResult Put() => Ok();

    [HttpPatch]
    public IActionResult Patch() => Ok();

}