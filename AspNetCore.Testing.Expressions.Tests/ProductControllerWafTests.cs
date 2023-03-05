using AspNetCore.Testing.Expressions.Web.Controllers;
using AspNetCore.Testing.Expressions.Web.Services;
using AspNetCore.Testing.MoqWebApplicationFactory;
using Moq;

namespace AspNetCore.Testing.Expressions.Tests;

public class ProductControllerWafTests :
    ProductControllerTestsBase<ControllerWafTestContext<ProductController>>
{
    [Fact]
    public void A()
    {
        TestContext.ConfigureMocks();
        var m = new Mock<IService>();
        m
            .Setup(x => x.GetString())
            .Returns(() => "string");

        var o = m.Object;


        var r = o.GetString();

        Assert.Equal("string", r);
    }
}