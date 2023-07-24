namespace AspNetCore.Testing.MoqWebApplicationFactory;

public abstract class MoqApiTestsBase<TController> : ApiTestsBase<TController, MoqHttpClientFactory<TController>> 
    where TController : class 
{
    public MoqApiTestsBase(MoqHttpClientFactory<TController> http) : base(http) { }
}