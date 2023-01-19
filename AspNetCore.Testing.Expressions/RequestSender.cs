using System.Collections.Specialized;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using static System.String;

namespace AspNetCore.Testing.Expressions;

internal class RequestSender<T>
{
    private readonly HttpClient _httpClient;
    private const string ControllerTemplate = "[controller]";

    internal RequestSender(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<TResponse?> SendAsync<TResponse>(LambdaExpression expression)
    {
        var body = expression.Body as MethodCallExpression;
        if (body == null)
        {
            throw new NotSupportedException("Provide a method call expression");
        }

        var (template, httpMethod, methodInfo) = GetTemplate(body);
        var uri = GetUri(template, methodInfo, body.Arguments.ToArray());
        
        var response = await _httpClient.SendAsync(new HttpRequestMessage
        {
            RequestUri = uri,
            Method = httpMethod,
            Content = GetHttpContent(methodInfo)
        });

        await CheckStatusCodeAsync(response);
        var responseBody = response.Content.Headers.ContentType?.MediaType?.ToUpperInvariant().Contains("JSON") == true
            ? await response.Content.ReadFromJsonAsync<TResponse>()
            : (TResponse)Convert.ChangeType(await response.Content.ReadAsStringAsync(), typeof(TResponse));
        
        return responseBody;
    }

    private HttpContent? GetHttpContent(MethodInfo methodInfo)
    {
        return null;
    }
    
    private static object? InvokeExpression(Expression e, Type returnType) => Expression
        .Lambda(typeof (Func<>).MakeGenericType(returnType), e)
        .Compile()
        .DynamicInvoke();

    private static Uri GetUri(string template, MethodInfo methodInfo, 
        Expression[] parameterExpressions)
    {
        var parameters = methodInfo
            .GetParameters()
            .Where(x => x.GetCustomAttribute<FromRouteAttribute>() != null ||
                        x.GetCustomAttribute<FromQueryAttribute>() != null || 
                        x.GetCustomAttribute<FromBodyAttribute>() != null)
            .ToArray();

        var nvc = HttpUtility.ParseQueryString(Empty);

        for(var i = 0; i < parameters.Length; i ++)
        {
            var parameter = parameters[i];
            var routeAttr = parameter.GetCustomAttribute<FromRouteAttribute>();
            var queryAttr = parameter.GetCustomAttribute<FromQueryAttribute>();

            if (routeAttr != null)
            {
                var expressionValue = InvokeExpression(parameterExpressions[i], parameter.ParameterType);
                // TODO: {id:type} scenario
                template = template.Replace($"{{{parameter.Name}}}", expressionValue?.ToString() ?? "");
            }
            if (queryAttr != null)
            {
                var expressionValue = InvokeExpression(parameterExpressions[i], parameter.ParameterType);
                nvc[parameter.Name] = expressionValue?.ToString() ?? "";
            }
        }

        var uriBuilder = new UriBuilder(new Uri(template))
        {
            Query = nvc.ToString()
        };
        return uriBuilder.Uri;
    }

    private static async Task CheckStatusCodeAsync(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode) return;
        var errorMessage = $"Server returned {(int)response.StatusCode}: {response.StatusCode}";

        try
        {
            var content = await response.Content.ReadAsStringAsync();
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