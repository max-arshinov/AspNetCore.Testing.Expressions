using AspNetCore.Testing.Expressions.Web.Features.ProductCatalog;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq.DataAttributes;

namespace AspNetCore.Testing.Expressions.Tests;

public class YamlTests: WebAppFactoryTestsBase<ProductsController>
{
    public YamlTests(WebApplicationFactory<ProductsController> factory) : base(factory)
    {
    }

    [Theory, YamlData(nameof(YamlTests))]
    public async Task A(int id)
    {
        var r = await ControllerClient.SendAsync(c => c.Get(id));
        Assert.Equal(id, r?.Id);
    }
}