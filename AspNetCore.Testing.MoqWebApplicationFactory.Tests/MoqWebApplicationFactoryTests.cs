using AspNetCore.Testing.Expressions.Web.Features.Service.Services;

namespace AspNetCore.Testing.MoqWebApplicationFactory.Tests;

public class MoqWebApplicationFactoryTests : IClassFixture<MoqWebApplicationFactoryExample>
{
    private readonly MoqWebApplicationFactoryExample _moqWebApplicationFactoryExample;

    public MoqWebApplicationFactoryTests(MoqWebApplicationFactoryExample moqWebApplicationFactoryExample)
    {
        _moqWebApplicationFactoryExample = moqWebApplicationFactoryExample;
    }
    
    [Fact]
    public async Task MockService_ReturnsAsConfigured()
    {
        var client = _moqWebApplicationFactoryExample.CreateClient();
        var res = await client.GetAsync("Service");
        var con = await res.Content.ReadAsStringAsync();

        Assert.Equal("String", con);
    }

    [Fact]
    public async Task MockService_ReturnsAsConfigured2()
    {
        var (factory, _) = _moqWebApplicationFactoryExample.WithMocks(m =>
        {
            m.Mock<IService>()
                .Setup(x => x.GetString())
                .Returns("Get Another String");
        });

        var client = factory.CreateClient();


        var res = await client.GetAsync("Service");
        var con = await res.Content.ReadAsStringAsync();

        Assert.Equal("Get Another String", con);
    }
}