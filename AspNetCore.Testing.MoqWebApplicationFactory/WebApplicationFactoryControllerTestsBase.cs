using AspNetCore.Testing.Expressions;

namespace AspNetCore.Testing.MoqWebApplicationFactory;

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