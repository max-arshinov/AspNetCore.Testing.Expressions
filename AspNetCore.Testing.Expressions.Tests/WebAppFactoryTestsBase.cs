using Microsoft.AspNetCore.Mvc.Testing;

namespace AspNetCore.Testing.Expressions.Tests;

public class WebAppFactoryTestsBase<T>: IClassFixture<WebApplicationFactory<T>>
    where T : class
{
    protected readonly ControllerClient<T> ControllerClient;

    public WebAppFactoryTestsBase(WebApplicationFactory<T> factory)
    {
        ControllerClient = new ControllerClient<T>(factory.CreateClient());
    }
}