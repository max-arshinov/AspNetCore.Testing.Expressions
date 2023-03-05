using Microsoft.AspNetCore.Mvc.Testing;

namespace AspNetCore.Testing.MoqWebApplicationFactory;

public class WafTestContext<TEntry> : WafTestContext<TEntry, MoqWebApplicationFactory<TEntry>>
    where TEntry : class
{
}

public class WafTestContext<TEntry, TFactory>: ITestContext
    where TEntry : class
    where TFactory: MoqWebApplicationFactory<TEntry>, new()
{
    private readonly MoqWebApplicationFactory<TEntry> _factory;

    private WebApplicationFactory<TEntry>? _clientFactory;

    public WafTestContext()
    {
        _factory = new MoqWebApplicationFactory<TEntry>();
    }

    public HttpClient CreateHttpClient() => (_clientFactory ?? _factory).CreateClient();

    public IMockEngine Mocks => _factory.Mocks;
    
    public void ConfigureMocks(Action<IMockEngine> setupMocks)
    {
        _clientFactory = _factory.WithMocks(setupMocks);
    }
}