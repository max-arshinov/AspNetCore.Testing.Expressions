using AspNetCore.Testing.Expressions;

namespace AspNetCore.Testing.MoqWebApplicationFactory;

public class ControllerTestsBase<TController, TTestContext>:
    HttpClientTestsBase<TTestContext> 
    where TTestContext : ITestContext, new()
{
    public ControllerTestsBase() : base(new TTestContext())
    {
    }

    public ControllerClient<TController> CreateControllerClient() => new(CreateHttpClient());
}

public class WafControllerTestsBase<TController, TFactory> :
    HttpClientTestsBase<WafTestContext<TController, TFactory>> 
    where TController : class 
    where TFactory : MoqWebApplicationFactory<TController>, new()
{
    public WafControllerTestsBase() : base(new WafTestContext<TController, TFactory>())
    {
    }
    
    public ControllerClient<TController> CreateControllerClient() => new(CreateHttpClient());
}

public class WafControllerTestsBase<TController> :
    WafControllerTestsBase<TController, MoqWebApplicationFactory<TController>>
    where TController : class 
{
}

public class HttpControllerTestsBase<TController> :
    HttpClientTestsBase<HttpTestContext> 
    where TController : class 
{
    public HttpControllerTestsBase() : base(new HttpTestContext())
    {
    }
    
    public ControllerClient<TController> CreateControllerClient() => new(CreateHttpClient());
}