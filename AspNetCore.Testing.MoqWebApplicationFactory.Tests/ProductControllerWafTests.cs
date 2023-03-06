using AspNetCore.Testing.Expressions.Web.Features.ProductCatalog;
using AspNetCore.Testing.Expressions.Web.Features.Service.Services;
using FluentAssertions;

namespace AspNetCore.Testing.MoqWebApplicationFactory.Tests;

public class ProductControllerWafTests :
    ProductControllerTestsBase<MoqHttpClientFactory<ProductController>>
{
    public ProductControllerWafTests(MoqHttpClientFactory<ProductController> http) : base(http)
    {
    }

    [Fact]
    public async Task B()
    {
    }

    [Fact]
    public async Task A()
    {
        HttpClientFactory.ConfigureMocks(nameof(A), m =>
        {
            m.Mock<IProductRepository>()   
                .Setup(x => x.GetAll())
                .Returns(() => new []
                {
                    new ProductDetails()
                    {
                        Id = 100500
                    }
                });
        });

        var namedClient = CreateControllerClient(nameof(A));
        var res = (await namedClient.SendAsync(c => c.Get()))?.ToArray();

        res.Should().HaveCount(1);
        res?[0].Id.Should().Be(100500);
        
        var defaultClient = CreateControllerClient();
        var res2 = (await defaultClient.SendAsync(c => c.Get()))?.ToArray();

        res2.Should().HaveCount(2);
    }
}