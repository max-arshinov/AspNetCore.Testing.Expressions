namespace AspNetCore.Testing.MoqWebApplicationFactory;

public class HttpTestContext: ITestContext
{
    public HttpClient CreateHttpClient()
    {
        throw new NotImplementedException();
    }
}