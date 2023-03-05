namespace AspNetCore.Testing.MoqWebApplicationFactory;

public abstract class HttpClientTestsBase<T>
    where T: IHttpClientFactory
{
    protected readonly T HttpClientFactory;

    public HttpClientTestsBase(T http)
    {
        HttpClientFactory = http;
    }

    protected HttpClient CreateHttpClient(string name = "") => HttpClientFactory.CreateClient(name);
}