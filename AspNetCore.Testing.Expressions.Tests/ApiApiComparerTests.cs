using AspNetCore.Testing.Expressions.Web.Features.ErrorCodes;
using AspNetCore.Testing.Expressions.Web.Features.ProductCatalog;
using Microsoft.AspNetCore.Mvc.Testing;

namespace AspNetCore.Testing.Expressions.Tests;

public class ApiApiComparerTests: ApiComparerTestsBase, 
    IClassFixture<WebApplicationFactory<ProductsController>>
{
    private readonly WebApplicationFactory<ProductsController> _factory;

    public ApiApiComparerTests(WebApplicationFactory<ProductsController> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task CompareSelf()
    {
        await CompareAsync(
            h => h.SendAsync((ProductsController c) => c.Get(1)),
            h => h.SendAsync((ProductsController c) => c.Get(1)),
            (e, a) =>
            {
                Assert.NotNull(e);
                Assert.NotNull(a);
                Assert.Equal(e!.Id, a!.Id);
            }
        );
    }

    [Fact]
    public async Task Compare400()
    {
        await CompareAsync(
            h => h.SendAsync((ErrorCodeController c) => c.Get400(), true),
            h => h.SendAsync((ErrorCodeController c) => c.Get400(), true),
            (e, a) =>
            {
                Assert.NotNull(e);
                Assert.NotNull(a);
                Assert.Equal(e, a);
                Assert.Equal("string", e);
            }
        );
    }
    
    [Fact]
    public async Task CompareMutation()
    {
        await MutateAndCompareAsync(
            h => h.SendAsync((ProductsController c) => c.Post()),
            h => h.SendAsync((ProductsController c) => c.Get(1)),
            h => h.SendAsync((ProductsController c) => c.Get(1)),
            (e, a) =>
            {
                Assert.NotNull(e);
                Assert.NotNull(a);
                Assert.Equal(e!.Id, a!.Id);
            }
        );
    }
    
    [Fact]
    public async Task Compare500()
    {
        await MutateAndCompareAsync(
            h => h.SendAsync((ErrorCodeController c) => c.Get500(), true),
            h => h.SendAsync((ErrorCodeController c) => c.Get500(), true),
            h => h.SendAsync((ErrorCodeController c) => c.Get500(), true),
            (e, a) =>
            {
                Assert.NotNull(e);
                Assert.NotNull(a);
                Assert.Equal(e, a);
                Assert.Equal("string", e);
            }
        );
    }    

    protected override string? ExpectedBaseAddress => null;

    protected override string? ActualBaseAddress => null;

    protected override HttpClient CreateHttpClient(string? baseAddress) => _factory.CreateClient();
}