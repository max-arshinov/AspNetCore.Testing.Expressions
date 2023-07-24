using System.Net.Http.Headers;
using Refit;

namespace AspNetCore.Testing.MoqWebApplicationFactory;

public class ApiTestsBase<TController, THttpClientFactory>:
    HttpClientTestsBase<THttpClientFactory> 
    where THttpClientFactory : IHttpClientFactory
{
    public ApiTestsBase(THttpClientFactory http) : base(http)
    {
    }

    public TController CreateRestClient(string name, Action<HttpRequestHeaders>? setupHeaders = null)
    {
        var httpClient = CreateHttpClient(name);
        if (setupHeaders != null)
        {
            setupHeaders(httpClient.DefaultRequestHeaders);
        }
        
        var rest = RestService.For<TController>(httpClient);
        return rest;
    }

    public TController CreateRestClient(Action<HttpRequestHeaders>? setupHeaders = null) =>
        CreateRestClient("", setupHeaders);
}