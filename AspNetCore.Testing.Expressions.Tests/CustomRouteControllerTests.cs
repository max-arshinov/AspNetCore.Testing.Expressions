using AspNetCore.Testing.Expressions.Web.Features.CustomRoutes;
using Microsoft.AspNetCore.Mvc.Testing;

namespace AspNetCore.Testing.Expressions.Tests;

public class CustomRouteControllerTests: WebAppFactoryTestsBase<CustomRouteController>
{
    public CustomRouteControllerTests(WebApplicationFactory<CustomRouteController> factory) : base(factory)
    {
    }

    [Fact]
    public async Task CustomRoute_NoParams()
    {
        var result = await ControllerClient.SendAsync(c => c.CustomRoute());
    }
}