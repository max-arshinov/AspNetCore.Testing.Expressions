using AspNetCore.Testing.Expressions.Web.Features.ProductCatalog;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace AspNetCore.Testing.Expressions.Tests;

public class ProductControllerTests: WebAppFactoryTestsBase<ProductController>
{
    public ProductControllerTests(WebApplicationFactory<ProductController> factory) : base(factory)
    {
    }

    [Fact]
    public async Task GetById_CanFetchAllProductsFromGetAll()
    {
        var products = (await ControllerClient.SendAsync(x => x.Get()))?.ToArray() ?? Array.Empty<ProductListItem>();
        products.Should().NotBeNull();
        
        var productDetailTasks = products
            .Select(async x =>
            {
                var res = await ControllerClient.SendAsync(c => c.Get(x.Id));
                return new KeyValuePair<int, ProductDetails?>(x.Id, res);
            })
            .ToList();

        var productDetails =await Task.WhenAll(productDetailTasks);
        

        productDetails.Should().AllSatisfy(x =>
        {
            x.Value.Should().NotBeNull();
            x.Value?.Id.Should().Be(x.Key);
        });
    }
    
    [Fact]
    public async Task LoadMany_GetById_CanFetchAllProductsFromGetAll()
    {
        var res = await ControllerClient
            .SendAsync(c => c.Get())
            .LoadMany(
                p => ControllerClient.SendAsync(c => c.Get(p.Id)),
                p => p.Id);
        
        res.Should().AllSatisfy(x =>
        {
            x.Value.Details.Should().NotBeNull();
            x.Value.Details?.Id.Should().Be(x.Key);
        });
    }    
}

