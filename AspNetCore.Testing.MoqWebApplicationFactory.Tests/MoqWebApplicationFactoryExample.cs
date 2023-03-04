using AspNetCore.Testing.Expressions.Web.Controllers;
using AspNetCore.Testing.Expressions.Web.Services;

namespace AspNetCore.Testing.MoqWebApplicationFactory.Tests;

public class MoqWebApplicationFactoryExample: MoqWebApplicationFactory<WeatherForecastController>
{
    protected override void ConfigureMocks(IMockEngine mocks)
    {
        mocks
            .Mock<IService>()
            .Setup(x => x.GetString())
            .Returns("String");
    }
}