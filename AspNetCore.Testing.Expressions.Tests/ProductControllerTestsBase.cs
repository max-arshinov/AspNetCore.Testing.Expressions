using AspNetCore.Testing.Expressions.Web.Controllers;
using AspNetCore.Testing.MoqWebApplicationFactory;

namespace AspNetCore.Testing.Expressions.Tests;

public abstract class ProductControllerTestsBase<T>: 
    ControllerTestsBase<ProductController, T> 
    where T : ITestContext, new()
{
    [Fact]
    public async Task LoadMany_GetById_CanFetchAllProductsFromGetAll()
    {
        Te
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