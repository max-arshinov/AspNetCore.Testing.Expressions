using AspNetCore.Testing.Expressions;
using AspNetCore.Testing.Expressions.Web.Features.ProductCatalog;
using FluentAssertions;

namespace AspNetCore.Testing.MoqWebApplicationFactory.Tests;

public abstract class ProductControllerTestsBase<T>: 
    ControllerTestsBase<ProductController, T>,
    IClassFixture<T> 
    where T : class, IHttpClientFactory
{
    protected ProductControllerTestsBase(T http) : base(http)
    {
    }

    [Fact]
    public async Task LoadMany_GetById_CanFetchAllProductsFromGetAll()
    {
        var client = CreateControllerClient();
        var res = await client
            .SendAsync(c => c.Get())
            .LoadMany(
                p => client.SendAsync(c => c.Get(p.Id)),
                p => p.Id);
        
        res.Should().AllSatisfy(x =>
        {
            x.Value.Details.Should().NotBeNull();
            x.Value.Details?.Id.Should().Be(x.Key);
        });
    }
}