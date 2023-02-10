using AspNetCore.Testing.Expressions.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;

namespace AspNetCore.Testing.Expressions.Tests;

public class HttpErrorCodesControllerTests: WebAppFactoryTestsBase<HttpErrorCodesController>
{
    public HttpErrorCodesControllerTests(WebApplicationFactory<HttpErrorCodesController> factory) : base(factory)
    {
    }

    [Fact]
    public async Task BadRequest_ThrowsBadHttpRequestException()
    {
        await Assert.ThrowsAsync<BadHttpRequestException>(async () =>
        {
            await ControllerClient.SendAsync(x => x.BadRequest());
        });
    }
}