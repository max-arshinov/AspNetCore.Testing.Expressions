using AspNetCore.Testing.Expressions.Web.Controllers;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq.DataAttributes;

namespace AspNetCore.Testing.Expressions.Tests;

public class YamlTests: WebAppFactoryTestsBase<ProductController>
{
    public YamlTests(WebApplicationFactory<ProductController> factory) : base(factory)
    {
    }

    [Theory, YamlData(nameof(YamlTests))]
    public async Task A(int id)
    {
        var r = await ControllerClient.SendAsync(c => c.Get(id));
        Assert.Equal(id, r?.Id);
    }
}