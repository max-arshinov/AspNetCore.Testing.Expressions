using Microsoft.AspNetCore.Mvc.Testing;

namespace AspNetCore.Testing.MoqWebApplicationFactory;

public class MoqHttpClientFactory<TEntry> : MoqHttpClientFactory<TEntry, MoqWebApplicationFactory<TEntry>>
    where TEntry : class
{
}

public class MoqHttpClientFactory<TEntry, TFactory>: IHttpClientFactory
    where TEntry : class
    where TFactory: MoqWebApplicationFactory<TEntry>, new()
{
    // private readonly TFactory _factory;

    private Dictionary<string, WebApplicationFactory<TEntry>> _clientFactories;

    private Dictionary<string, IMockProvider> _mocks;
    
    public MoqHttpClientFactory()
    {
        // _factory = factory;
        _clientFactories = new Dictionary<string, WebApplicationFactory<TEntry>>();
        _mocks = new Dictionary<string, IMockProvider>();
    }


    public void ConfigureMocks(Action<IMockProvider> setupMocks) => ConfigureMocks("", setupMocks);

    public void ConfigureMocks(string name, Action<IMockProvider> setupMocks)
    {
        if (name == null) throw new ArgumentNullException(nameof(name));
        var (factory, provider) = new TFactory().WithMocks(setupMocks);
        // TODO: check why original factory is modified
        _clientFactories[name] = factory;
        _mocks[name] = provider;
    }


    public HttpClient CreateClient(string name = "") => 
        (_clientFactories.GetValueOrDefault(name) ?? new TFactory()).CreateClient();
}