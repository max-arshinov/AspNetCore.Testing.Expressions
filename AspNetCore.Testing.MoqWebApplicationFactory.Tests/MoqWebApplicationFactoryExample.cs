using AspNetCore.Testing.Expressions.Web.Features.Service.Services;
using AspNetCore.Testing.Expressions.Web.Features.WatherForecast;

namespace AspNetCore.Testing.MoqWebApplicationFactory.Tests;

public class MoqWebApplicationFactoryExample: MoqWebApplicationFactory<WeatherForecastController>
{
    protected override void ConfigureMocks(IMockProvider mocks)
    {
        mocks
            .Mock<IService>()
            .Setup(x => x.GetString())
            .Returns("String");
    }
}