using System.Net;
using AspNetCore.Testing.Expressions.Web.Features.HttpErrorCodes;
using Microsoft.AspNetCore.Mvc.Testing;

namespace AspNetCore.Testing.Expressions.Tests;

public class HttpErrorCodesControllerTests: WebAppFactoryTestsBase<HttpErrorCodesController>
{
    public HttpErrorCodesControllerTests(WebApplicationFactory<HttpErrorCodesController> factory) : base(factory)
    {
    }

    [Fact]
    public async Task BadRequest_ThrowsHttpRequestException_BadRequestStatusCode()
    {
        var e = await Assert.ThrowsAsync<HttpRequestException>(async () =>
        {
            await ControllerClient.SendAsync(x => x.Get400());
        });
        
        Assert.Equal(HttpStatusCode.BadRequest, e.StatusCode);
    }
    
    [Fact]
    public async Task NotFound_ThrowsBadHttpRequestException_NotFoundStatusCode()
    {
        var e = await Assert.ThrowsAsync<HttpRequestException>(async () =>
        {
            await ControllerClient.SendAsync(x => x.Get404());
        });
        
        Assert.Equal(HttpStatusCode.NotFound, e.StatusCode);
    }
}