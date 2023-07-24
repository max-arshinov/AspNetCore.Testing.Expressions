using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AspNetCore.Testing.MoqWebApplicationFactory;

public class MoqWebApplicationFactory<TEntryPoint>: WebApplicationFactory<TEntryPoint> 
    where TEntryPoint : class
{
    private readonly IMockProvider Mocks = new MockProvider();

    protected virtual void ConfigureMocks(IMockProvider mocks) { }
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        ConfigureMocks(Mocks);
        builder.ConfigureTestServices(services =>
        {
            foreach (var mock in Mocks)
            {
                services.AddSingleton(mock.Key, mock.Value.Object);
            }

            foreach (var mock in Mocks)
            {
                var descriptor =
                    new ServiceDescriptor(
                        mock.Key,
                        mock.Value.Object);
            
                services.Replace(descriptor);
            }
        });
    }

    public WebApplicationFactory<TEntryPoint> WithMocks(IMockProvider provider)
    {
        return WithWebHostBuilder(builder => builder.ConfigureTestServices(
            services =>
            {
                foreach (var mock in provider)
                {
                    var descriptor =
                        new ServiceDescriptor(
                            mock.Key,
                            mock.Value.Object);

                    services.Replace(descriptor);
                }
            }));
    }
    
    public (WebApplicationFactory<TEntryPoint>, IMockProvider) WithMocks(Action<IMockProvider> setupMocks)
    {
        IMockProvider provider = new MockProvider();
        setupMocks(provider);
        
        return (WithMocks(provider), provider);
    }
}