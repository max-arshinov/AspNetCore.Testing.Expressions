using AspNetCore.Testing.Expressions;

namespace AspNetCore.Testing.MoqWebApplicationFactory;

public class ControllerWafTestContext<TController>: 
    ControllerWafTestContext<TController, MoqWebApplicationFactory<TController>>
    where TController : class
{
}

public class ControllerWafTestContext<TController, TFactory>: WafTestContext<TController, TFactory>
    where TController : class 
    where TFactory : MoqWebApplicationFactory<TController>, new()
{
    public ControllerClient<TController> CreateControllerClient() => new(CreateHttpClient());
}