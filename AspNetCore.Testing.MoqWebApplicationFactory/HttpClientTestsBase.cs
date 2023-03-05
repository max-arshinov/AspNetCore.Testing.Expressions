namespace AspNetCore.Testing.MoqWebApplicationFactory;

public abstract class HttpClientTestsBase<T>
    where T: ITestContext
{
    protected readonly T TestContext;

    public HttpClientTestsBase(T testContext)
    {
        TestContext = testContext;
    }

    protected HttpClient CreateHttpClient() => TestContext.CreateHttpClient();
}