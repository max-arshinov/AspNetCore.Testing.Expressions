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

    [HttpGet(nameof(GetAsync))]
    public async Task<ActionResult<IEnumerable<WeatherForecast>>> GetAsync()
    {
        return GetForecast();
    }
    
    [HttpGet("{id}")]
    public ActionResult<int> GetWithRouteParam([FromRoute] int id) => id;

    [HttpGet(nameof(GetWithQueryParam))]
    public ActionResult<string> GetWithQueryParam([FromQuery] string q1) => q1;

    [HttpGet(nameof(GetWithServiceDependency))]
    public ActionResult<string> GetWithServiceDependency([FromServices] IService service) => 
        service?.GetType().Name ?? "";
    
    [HttpPost]
    public IActionResult Post() => Ok();
    
    [HttpPut]
    public IActionResult Put() => Ok();

    [HttpPatch]
    public IActionResult Patch() => Ok();

}