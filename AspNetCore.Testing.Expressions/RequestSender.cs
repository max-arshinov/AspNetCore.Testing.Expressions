using System.Collections.Specialized;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace AspNetCore.Testing.Expressions;

internal class RequestSender<T>
{
    private readonly HttpClient _httpClient;
    private const string ControllerTemplate = "[controller]";

    internal RequestSender(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<TResponse?> Send<TResponse>(LambdaExpression expression)
    {
        var body = expression.Body as MethodCallExpression;
        if (body == null)
        {
            throw new NotSupportedException("Provide a method call expression");
        }

        var (template, httpMethod, methodInfo) = GetTemplate(body);
        var response = await _httpClient.SendAsync(new HttpRequestMessage
        {
            RequestUri = GetUri(template, methodInfo, expression.Parameters),
            Method = httpMethod,
            Content = GetHttpContent(methodInfo)
        });

        CheckStatusCode(response);
        
        var responseBody = await response.Content.ReadFromJsonAsync<TResponse>();
        return responseBody;
    }

    private HttpContent? GetHttpContent(MethodInfo methodInfo)
    {
        return null;
    }

    private static Uri GetUri(string template, MethodInfo methodInfo, 
        IReadOnlyCollection<ParameterExpression> parameterExpressions)
    {
        var parameters = methodInfo
            .GetParameters()
            .Where(x => x.GetCustomAttribute<FromQueryAttribute>() != null || 
                        x.GetCustomAttribute<FromRouteAttribute>() != null);

        var nvc = new NameValueCollection();
        foreach (var parameter in parameters)
        {
            var routeAttr = parameter.GetCustomAttribute<FromRouteAttribute>();
            var queryAttr = parameter.GetCustomAttribute<FromQueryAttribute>();

            if (routeAttr != null)
            {
                template = template.Replace($"[{parameter.Name}]", "TODO:FROM-EXPRESSION");
            }
            if (queryAttr != null)
            {
                nvc[parameter.Name] = "TODO:FROM-EXPRESSION";
            }
        }

        var uriBuilder = new UriBuilder(new Uri(template))
        {
            Query = nvc.ToString()
        };
        return uriBuilder.Uri;
    }

    private static void CheckStatusCode(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode) return;
        var errorMessage = $"Server returned {response.StatusCode}";
        try
        {
            var content = response.Content.ReadAsStringAsync();
            errorMessage += $": {content}";
        }
        catch (Exception e)
        {
            throw new HttpRequestException(errorMessage, e);
        }

        throw new HttpRequestException(errorMessage);
    }

    private static (string, HttpMethod, MethodInfo) GetTemplate(MethodCallExpression body)
    {
        var template = "/" + typeof(T).GetCustomAttribute<RouteAttribute>()?.Template ?? ControllerTemplate;
        var controllerName = typeof(T).Name;
        if (controllerName.ToUpperInvariant().EndsWith("CONTROLLER"))
        {
            controllerName = new string(controllerName.Take(controllerName.Length - 10).ToArray());
        }

        template = template.Replace(ControllerTemplate, controllerName);
        var customAttribute = body
            .Method
            .GetCustomAttributes()
            .FirstOrDefault(x => x is HttpMethodAttribute) as HttpMethodAttribute;

        var methodTemplate = customAttribute?.Template;
        if (methodTemplate != null)
        {
            template = template + "/" + methodTemplate;
        }

        return (template, new HttpMethod(customAttribute?.HttpMethods.First() ?? "GET"), body.Method);
    }
}