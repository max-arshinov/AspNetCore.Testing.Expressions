using AspNetCore.Testing.Expressions;
using AspNetCore.Testing.Expressions.Web.Features.ProductCatalog;
using FluentAssertions;

namespace AspNetCore.Testing.MoqWebApplicationFactory.Tests;

public abstract class ProductControllerTestsBase<T>: 
    ControllerTestsBase<ProductsController, T>,
    IClassFixture<T> 
    where T : class, IHttpClientFactory
{
    protected ProductControllerTestsBase(T http) : base(http)
    {
    }

    [Fact]
    public async Task LoadMany_GetById_CanFetchAllProductsFromGetAll()
    {
        var client = CreateControllerClient(nameof(LoadMany_GetById_CanFetchAllProductsFromGetAll));
        var res = await client
            .SendAsync(c => c.Get(new Paging()))
            .LoadMany(
                p => client.SendAsync(c => c.Get(p.Id)),
                p => p.Id);
        
        res.Should().AllSatisfy(x =>
        {
            x.Value.Details.Should().NotBeNull();
            x.Value.Details?.Id.Should().Be(x.Key);
        });
    }
    
    private static async Task CheckDefault(ControllerClient<ProductsController> defaultClient)
    {
        var defaultResult = (await defaultClient.SendAsync(c => c.Get(new Paging())))?.ToArray();
        defaultResult.Should().HaveCount(3);
        defaultResult?[0].Id.Should().Be(1050);
        defaultResult?[1].Id.Should().Be(2060);
        defaultResult?[2].Id.Should().Be(3070);
    }
    
    [Fact]
    public async Task A()
    {
        var defaultClient = CreateControllerClient();
        await CheckDefault(defaultClient);
        
        var namedAClient = CreateControllerClient(nameof(A));
        var namedAResult = (await namedAClient.SendAsync(c => c.Get(new Paging())))?.ToArray();
        namedAResult.Should().HaveCount(2);
        namedAResult?[0].Id.Should().Be(100500);
        namedAResult?[1].Id.Should().Be(200600);
        
        
        var namedBClient = CreateControllerClient(nameof(B));
        await CheckDefault(namedBClient);
    }

    [Fact]
    public async Task B()
    {
        var namedBClient = CreateControllerClient(nameof(B));
        await CheckDefault(namedBClient);
    }    
}