using AspNetCore.Testing.Expressions.Web.Controllers;
using Microsoft.AspNetCore.Mvc.Testing;

namespace AspNetCore.Testing.Expressions.Tests;

public class UnitTest1: IClassFixture<WebApplicationFactory<WeatherForecastController>>
{
    private readonly WebApplicationFactory<WeatherForecastController> _factory;

    public UnitTest1(WebApplicationFactory<WeatherForecastController> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Test1()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync((WeatherForecastController c) => c.Get());
        Assert.Equal(5, response?.Count());
    }
    
    [Fact]
    public async Task Test2()
    {
        var client = _factory.CreateClient();
        // var response = await client.GetAsync((WeatherForecastController c) => c.Get2(new SomeFilters()));
    }    
}