using Microsoft.AspNetCore.Mvc.Testing;

namespace AspNetCore.Testing.MoqWebApplicationFactory;

public class MoqHttpClientFactory<TEntry> : MoqHttpClientFactory<TEntry, MoqWebApplicationFactory<TEntry>>
    where TEntry : class
{
}

public class MoqHttpClientFactory<TEntry, TWebApplicationFactory>: IHttpClientFactory
    where TEntry : class
    where TWebApplicationFactory: MoqWebApplicationFactory<TEntry>, new()
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
        IMockProvider provider = new MockProvider();
        setupMocks(provider);
        _mocks[name] = provider;
    }


    public HttpClient CreateClient(string name = "") =>
        (_clientFactories.GetValueOrDefault(name) ?? CreateNewClient(name)).CreateClient();

    private WebApplicationFactory<TEntry> CreateNewClient(string name)
    {
        var defaultMockProvider = _mocks.GetValueOrDefault("");
        var namedMockProvider = _mocks.GetValueOrDefault(name);

        if (defaultMockProvider != null)
        {
            var mockProvider = namedMockProvider != null
                ? defaultMockProvider.Merge(namedMockProvider)
                : defaultMockProvider;
            
            _clientFactories[name] = new TWebApplicationFactory().WithMocks(mockProvider);
        }
        else
        {
            if (namedMockProvider != null)
            {
                _clientFactories[name] = new TWebApplicationFactory().WithMocks(namedMockProvider);
            }
            else
            {
                _clientFactories[name] = new TWebApplicationFactory();
            }
        }

        return _clientFactories[name];
    }
}