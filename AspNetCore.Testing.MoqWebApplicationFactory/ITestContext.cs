namespace AspNetCore.Testing.MoqWebApplicationFactory;

public interface ITestContext
{
    HttpClient CreateHttpClient();
}

