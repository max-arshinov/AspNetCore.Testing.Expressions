using Microsoft.Extensions.Configuration;

namespace AspNetCore.Testing.MoqWebApplicationFactory;

public class HttpClientFactory: IHttpClientFactory
{
    private static IConfiguration InitConfiguration()
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("testsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"testsettings.{env}.json", optional: true)
            .AddEnvironmentVariables();

        return builder.Build();
    }

    private static IConfiguration Config = InitConfiguration();
    
    public HttpClient CreateClient(string name)
    {
        if (name == null) throw new ArgumentNullException(nameof(name));
        var host = Config["Host"];
        if (string.IsNullOrWhiteSpace(host))
        {
            throw new InvalidOperationException("Host value is missing from appsettings.json file or env variable");
        }

        var httpCLient = new HttpClient()
        {
            BaseAddress = new Uri(host)
        };

        return httpCLient;
    }
}