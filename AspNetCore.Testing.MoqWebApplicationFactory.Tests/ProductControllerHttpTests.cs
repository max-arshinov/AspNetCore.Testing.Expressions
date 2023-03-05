namespace AspNetCore.Testing.MoqWebApplicationFactory.Tests;

public class ProductControllerHttpTests:
    ProductControllerTestsBase<HttpClientFactory>
{
    public ProductControllerHttpTests(HttpClientFactory httpClientFactory) : base(httpClientFactory)
    {
    }
}