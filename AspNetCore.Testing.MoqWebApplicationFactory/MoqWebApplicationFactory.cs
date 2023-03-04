using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AspNetCore.Testing.MoqWebApplicationFactory;

public class MoqWebApplicationFactory<TEntryPoint>: WebApplicationFactory<TEntryPoint>
    where TEntryPoint : class
{
    private MockEngine _mocks = new MockEngine();

    protected virtual void ConfigureMocks(IMockEngine mocks) { }
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        ConfigureMocks(_mocks);
        builder.ConfigureTestServices(services =>
        {
            foreach (var mock in _mocks.Mocks)
            {
                services.AddSingleton(mock.Key, (object)((dynamic)mock.Value).Object);
            }

            foreach (var mock in _mocks.Mocks)
            {
                var descriptor =
                    new ServiceDescriptor(
                        mock.Key,
                        (object)((dynamic)mock.Value).Object);
            
                services.Replace(descriptor);
            }
        });
    }

    public WebApplicationFactory<TEntryPoint> WithMocks(Action<IMockEngine> setupMocks)
    {
        IMockEngine engine = new MockEngine();
        setupMocks(engine);
        
        return WithWebHostBuilder(builder => builder.ConfigureTestServices(
            services =>
            {
                foreach (var mock in engine.Mocks)
                {
                    var descriptor =
                        new ServiceDescriptor(
                            mock.Key,
                            (object)((dynamic)mock.Value).Object);
    
                    services.Replace(descriptor);
                }
            }));
    }
}