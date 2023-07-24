using System.Diagnostics.CodeAnalysis;

namespace AspNetCore.Testing.MoqWebApplicationFactory.Tests;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ProductControllerHttpTests:
    ProductControllerTestsBase<HttpClientFactory>
{
    public ProductControllerHttpTests(HttpClientFactory httpClientFactory) : base(httpClientFactory)
    {
    }
}