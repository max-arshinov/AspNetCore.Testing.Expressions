using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace AspNetCore.Testing.MoqWebApplicationFactory;

public abstract class MoqWebApplicationFactoryBase<TEntryPoint>: WebApplicationFactory<TEntryPoint>,
    IMockEngine
    where TEntryPoint : class
{
    private Dictionary<Type, object> _mocks = new();

    Mock<T> IMockEngine.Mock<T>()
        where T : class
    {
        var mock = new Mock<T>();
        _mocks[typeof(T)] = mock;
        return mock;
    }

    protected abstract void Mock(IMockEngine mocks);

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Mock(this);
        builder.ConfigureServices(services =>
        {
            foreach (var mock in _mocks)
            {
                services.AddSingleton(mock.Key, (object)((dynamic)mock.Value).Object);
            }            
        });
    }
}