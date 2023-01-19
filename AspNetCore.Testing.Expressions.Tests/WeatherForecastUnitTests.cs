using AspNetCore.Testing.Expressions.Web.Controllers;
using AspNetCore.Testing.Expressions.Web.Models;
using Microsoft.AspNetCore.Mvc.Testing;

namespace AspNetCore.Testing.Expressions.Tests;

public class WeatherForecastUnitTests: IClassFixture<WebApplicationFactory<WeatherForecastController>>
{
    private readonly WebApplicationFactory<WeatherForecastController> _factory;

    public WeatherForecastUnitTests(WebApplicationFactory<WeatherForecastController> factory)
    {
        _factory = factory;
    }

    private HttpClient CreateClient()
    {
        return _factory.CreateClient();
    }
    
    [Fact]
    public async Task Get_WithoutParameters()
    {
        var client = CreateClient();
        var response = await client.GetAsync((WeatherForecastController c) => c.Get());
        Assert.Equal(5, response?.Count());
    }

    [Fact]
    public async Task Get_WithRouteParameter()
    {
        var client = _factory.CreateClient();
        var prm = 1;
        var response = await client.GetAsync((WeatherForecastController c) => c.GetWithRouteParam(prm));
        Assert.Equal(prm, response);
    }
    
    [Fact]
    public async Task Get_WithQueryParameter()
    {
        var client = _factory.CreateClient();
        var prm = "q1";
        var response = await client.GetAsync((WeatherForecastController c) => c.GetWithQueryParam(prm));
        Assert.Equal(prm, response);
    }
    
    [Fact]
    public async Task Get_WithServiceParameter()
    {
        var client = _factory.CreateClient();
        var svc = new Service();
        var response = await client.GetAsync((WeatherForecastController c) => c.GetWithServiceDependency(svc));
        Assert.Equal(svc.GetType().Name, response);
        
        response = await client.GetAsync((WeatherForecastController c) => c.GetWithServiceDependency(null!));
        Assert.Equal(svc.GetType().Name, response);
    }
    
    [Fact]
    public async Task GetAsync_WithoutParameters()
    {
        var client = CreateClient();
        var response = await client.GetAsync((WeatherForecastController c) => c.GetAsync());
        Assert.Equal(5, response?.Count());
    }
}