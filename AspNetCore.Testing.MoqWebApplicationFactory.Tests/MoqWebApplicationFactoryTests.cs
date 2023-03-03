namespace AspNetCore.Testing.MoqWebApplicationFactory.Tests;

public class MoqWebApplicationFactoryTests : IClassFixture<MoqWebApplicationFactory>
{
    private readonly MoqWebApplicationFactory _moqWebApplicationFactory;

    public MoqWebApplicationFactoryTests(MoqWebApplicationFactory moqWebApplicationFactory)
    {
        _moqWebApplicationFactory = moqWebApplicationFactory;
    }
    
    [Fact]
    public async Task MockService_ReturnsAsConfigured()
    {
        var client = _moqWebApplicationFactory.CreateClient();
        var res = await client.GetAsync("Service");
        var con = await res.Content.ReadAsStringAsync();
        
        Assert.Equal("GetString", con);
    }
}