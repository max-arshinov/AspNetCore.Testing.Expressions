using AspNetCore.Testing.Expressions.Web.Controllers;
using AspNetCore.Testing.Expressions.Web.Services;

namespace AspNetCore.Testing.MoqWebApplicationFactory.Tests;

public class MoqWebApplicationFactory: MoqWebApplicationFactoryBase<WeatherForecastController>
{
    protected override void Mock(IMockEngine mocks)
    {
        mocks
            .Mock<IService>()
            .Setup(x => x.GetString())
            .Returns("GetString");
    }
}