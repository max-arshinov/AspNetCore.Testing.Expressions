using AspNetCore.Testing.Expressions;

namespace AspNetCore.Testing.MoqWebApplicationFactory;

public class ControllerMoqHttpClientFactory<TController>: 
    ControllerMoqHttpClientFactory<TController, MoqWebApplicationFactory<TController>>
    where TController : class
{
}

public class ControllerMoqHttpClientFactory<TController, TFactory>: MoqHttpClientFactory<TController, TFactory>
    where TController : class 
    where TFactory : MoqWebApplicationFactory<TController>, new()
{
    public ControllerClient<TController> CreateControllerClient() => new(CreateClient());
}