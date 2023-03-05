using AspNetCore.Testing.Expressions;

namespace AspNetCore.Testing.MoqWebApplicationFactory;

public class ControllerTestsBase<TController, THttpClientFactory>:
    HttpClientTestsBase<THttpClientFactory> 
    where THttpClientFactory : IHttpClientFactory
{
    public ControllerTestsBase(THttpClientFactory http) : base(http)
    {
    }

    public ControllerClient<TController> CreateControllerClient(string name = "") => new(CreateHttpClient(name));
}

public class WebApplicationFactoryControllerTestsBase<TController, TFactory> :
    HttpClientTestsBase<MoqHttpClientFactory<TController, TFactory>> 
    where TController : class 
    where TFactory : MoqWebApplicationFactory<TController>, new()
{
    public WebApplicationFactoryControllerTestsBase(MoqHttpClientFactory<TController, TFactory> httpClientFactory) : base(httpClientFactory)
    {
    }
    
    public ControllerClient<TController> CreateControllerClient(string name = "") => new(CreateHttpClient(name));
}

public class WebApplicationFactoryControllerTestsBase<TController> :
    WebApplicationFactoryControllerTestsBase<TController, MoqWebApplicationFactory<TController>>
    where TController : class 
{
    public WebApplicationFactoryControllerTestsBase(
        MoqHttpClientFactory<TController, MoqWebApplicationFactory<TController>> httpClientFactory) : 
        base(httpClientFactory)
    {
    }
}

public class HttpControllerTestsBase<TController> :
    HttpClientTestsBase<HttpClientFactory> 
    where TController : class 
{
    public HttpControllerTestsBase(HttpClientFactory httpClientFactory) : base(httpClientFactory)
    {
    }
    
    public ControllerClient<TController> CreateControllerClient(string name = "") => new(CreateHttpClient(name));
}