using AspNetCore.Testing.Expressions.Web.Controllers;
using AspNetCore.Testing.Expressions.Web.Models;
using Microsoft.AspNetCore.Mvc.Testing;

namespace AspNetCore.Testing.Expressions.Tests;

public class WeatherForecastControllerTests: WebAppFactoryTestsBase<WeatherForecastController>
{
    public WeatherForecastControllerTests(WebApplicationFactory<WeatherForecastController> factory) : base(factory)
    {
    }
    
    [Fact]
    public async Task Get_WithoutParameters()
    {
        var response = await ControllerClient.SendAsync(c => c.Get());
        Assert.Equal(5, response?.Count());
    }

    [Fact]
    public async Task Get_WithRouteParameter()
    {
        var prm = 1;
        var response = await ControllerClient.SendAsync(c => c.GetWithRouteParam(prm));
        Assert.Equal(prm, response);
    }
    
    [Fact]
    public async Task Get_WithQueryParameter()
    {
        var prm = "q1";
        var response = await ControllerClient.SendAsync(c => c.GetWithQueryParam(prm));
        Assert.Equal(prm, response);
    }
    
    [Fact]
    public async Task Get_WithServiceParameter()
    {
        var svc = new Service();
        var response = await ControllerClient.SendAsync(c => c.GetWithServiceDependency(svc));
        Assert.Equal(svc.GetType().Name, response);
        
        response = await ControllerClient.SendAsync(c => c.GetWithServiceDependency(null!));
        Assert.Equal(svc.GetType().Name, response);
    }
    
    [Fact]
    public async Task GetAsync_WithoutParameters()
    {
        var response = await ControllerClient.SendAsync(c => c.GetAsync());
        Assert.Equal(5, response?.Count());
    }

    [Fact]
    public async Task Post_WithFromBody()
    {
        var prm = new SomePararms()
        {
            Param = "Param"
        };
        
        var response = await ControllerClient.SendAsync(c => c.Post(prm));
        Assert.Equal(prm.Param, response);
    }
}