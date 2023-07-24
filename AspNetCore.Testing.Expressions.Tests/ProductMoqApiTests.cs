using AspNetCore.Testing.Expressions.Web.Features.ProductCatalog;
using AspNetCore.Testing.MoqWebApplicationFactory;

namespace AspNetCore.Testing.Expressions.Tests;

public class ProductMoqApiTests: MoqApiTestsBase<IProductsController>, 
    IClassFixture<MoqHttpClientFactory<IProductsController>>
{
    public ProductMoqApiTests(MoqHttpClientFactory<IProductsController> http) : base(http) { }

    [Fact]
    public async Task A()
    {
        var apiClient = CreateRestClient();

        var res = await apiClient.GetAsync(1);
        res.Should().NotBeNull();
    }
}