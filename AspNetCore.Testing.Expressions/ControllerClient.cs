using System.Linq.Expressions;
using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using static System.String;

namespace AspNetCore.Testing.Expressions;

public class ControllerClient<TController>
{
    public HttpClient HttpClient { get; }

    public ControllerClient(HttpClient httpClient)
    {
        HttpClient = httpClient;
    }

    public ControllerClient<TController> AddBearerToken(string bearerToken)
    {
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
        return this;
    }

    public Task<TResponse?> SendAsync<TResponse>( 
        Expression<Func<TController, Task<ActionResult<TResponse>>>> expression, bool acceptErrorStatusCodes = false) =>
        DoSendAsync<TResponse>(expression, acceptErrorStatusCodes);

    public Task<TResponse?> SendAsync<TResponse>(
        Expression<Func<TController, ActionResult<TResponse>>> expression, bool acceptErrorStatusCodes = false) =>
        DoSendAsync<TResponse>(expression, acceptErrorStatusCodes);

    public Task<TResponse?> SendAsync<TResponse>( 
        Expression<Func<TController, TResponse>> expression, bool acceptErrorStatusCodes = false) =>
        DoSendAsync<TResponse>(expression, acceptErrorStatusCodes);

    public Task<TResponse?> SendAsync<TResponse>( 
        Expression<Func<TController, Task<TResponse>>> expression, bool acceptErrorStatusCodes = false) =>
        DoSendAsync<TResponse>(expression, acceptErrorStatusCodes);
    
    internal async Task<TResponse?> DoSendAsync<TResponse>(LambdaExpression expression, 
        bool acceptErrorStatusCodes = false)
    {
        var body = expression.Body as MethodCallExpression;
        if (body == null)
        {
            throw new NotSupportedException("Provide a method call expression");
        }

        var message = new RequestMessageBuilder().Build(body);
        var response = await HttpClient.SendAsync(message);

        if (!acceptErrorStatusCodes)
        {
            await CheckStatusCodeAsync(response, message.RequestUri!);
        }
        

        if (response.StatusCode == HttpStatusCode.NoContent)
        {
            return default;
        }
        var responseBody = response.Content.Headers.ContentType?.MediaType?.ToUpperInvariant().Contains("JSON") == true
            ? await response.Content.ReadFromJsonAsync<TResponse>()
            : (TResponse)Convert.ChangeType(await response.Content.ReadAsStringAsync(), typeof(TResponse));
        
        return responseBody;
    }
    
    public async Task<TResponse?[]?> FetchArrayAsync<TResponse>( 
        Expression<Func<TController, IEnumerable<TResponse>>> expression, bool acceptErrorStatusCodes = false) =>
        (await DoSendAsync<IEnumerable<TResponse>>(expression, acceptErrorStatusCodes))?.ToArray();    
    
    public async Task<TResponse?[]?> FetchArrayAsync<TResponse>( 
        Expression<Func<TController, Task<IEnumerable<TResponse>>>> expression, bool acceptErrorStatusCodes = false) =>
        (await DoSendAsync<IEnumerable<TResponse>>(expression, acceptErrorStatusCodes))?.ToArray();     
    
    private static async Task CheckStatusCodeAsync(HttpResponseMessage response, Uri requestUri)
    {
        if (response.IsSuccessStatusCode) return;
        var errorMessage = $"{requestUri} returned HTTP code {(int)response.StatusCode} ({SplitCamelCase(response.StatusCode.ToString())})";

        try
        {
            var content = await response.Content.ReadAsStringAsync();
            if (!IsNullOrWhiteSpace(content))
            {
                errorMessage += $": {content}";
            }
        }
        catch (Exception e)
        {
            throw new HttpRequestException(errorMessage, e, response.StatusCode);
        }

        
        throw new HttpRequestException(errorMessage, null, response.StatusCode);
    }
    
    private static string SplitCamelCase(string input)
    {
        return System.Text.RegularExpressions.Regex.Replace(input, "([A-Z])", " $1", 
            System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
    }
}