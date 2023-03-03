using System.Collections.Specialized;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using static System.String;

namespace AspNetCore.Testing.Expressions;

public class ControllerClient<TController>
{
    private readonly HttpClient _httpClient;
    private const string ControllerTemplate = "[controller]";

    public ControllerClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<TResponse?> SendAsync<TResponse>( 
        Expression<Func<TController, Task<ActionResult<TResponse>>>> expression) =>
        DoSendAsync<TResponse>(expression);

    public Task<TResponse?> SendAsync<TResponse>(
        Expression<Func<TController, ActionResult<TResponse>>> expression) =>
        DoSendAsync<TResponse>(expression);

    public Task<TResponse?> SendAsync<TResponse>( 
        Expression<Func<TController, TResponse>> expression) =>
        DoSendAsync<TResponse>(expression);

    public Task<TResponse?> SendAsync<TResponse>( 
        Expression<Func<TController, Task<TResponse>>> expression) =>
        DoSendAsync<TResponse>(expression);
    
    internal async Task<TResponse?> DoSendAsync<TResponse>(LambdaExpression expression)
    {
        var body = expression.Body as MethodCallExpression;
        if (body == null)
        {
            throw new NotSupportedException("Provide a method call expression");
        }

        var (template, httpMethod, methodInfo) = GetTemplate(body);
        var (uri, content) = GetUriAndHttpContent(template, methodInfo, body.Arguments.ToArray());
        
        var response = await _httpClient.SendAsync(new HttpRequestMessage
        {
            RequestUri = uri,
            Method = httpMethod,
            Content = content
        });

        await CheckStatusCodeAsync(response);
        var responseBody = response.Content.Headers.ContentType?.MediaType?.ToUpperInvariant().Contains("JSON") == true
            ? await response.Content.ReadFromJsonAsync<TResponse>()
            : (TResponse)Convert.ChangeType(await response.Content.ReadAsStringAsync(), typeof(TResponse));
        
        return responseBody;
    }

    private HttpContent? GetHttpContent(MethodInfo methodInfo)
    {
        return JsonContent.Create(new { });
    }
    
    private static object? InvokeExpression(Expression e, Type returnType) => Expression
        .Lambda(typeof (Func<>).MakeGenericType(returnType), e)
        .Compile()
        .DynamicInvoke();

    private static (Uri, HttpContent?) GetUriAndHttpContent(string template, MethodInfo methodInfo, 
        Expression[] parameterExpressions)
    {
        var routeAttribute = methodInfo.GetCustomAttribute<RouteAttribute>();
        if (routeAttribute != null)
        {
            template += $"/{routeAttribute.Template}";
        }
        
        var parameters = methodInfo
            .GetParameters()
            .Where(x => x.GetCustomAttribute<FromRouteAttribute>() != null ||
                        x.GetCustomAttribute<FromQueryAttribute>() != null || 
                        x.GetCustomAttribute<FromBodyAttribute>() != null)
            .ToArray();

        var nvc = HttpUtility.ParseQueryString(Empty);
        HttpContent? content = null;
        
        for(var i = 0; i < parameters.Length; i ++)
        {
            var parameter = parameters[i];
            var routeAttr = parameter.GetCustomAttribute<FromRouteAttribute>();
            var queryAttr = parameter.GetCustomAttribute<FromQueryAttribute>();
            var bodyAttr = parameter.GetCustomAttribute<FromBodyAttribute>();

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

            if (bodyAttr != null)
            {
                var expressionValue = InvokeExpression(parameterExpressions[i], parameter.ParameterType);
                content = JsonContent.Create(expressionValue);
            }
        }

        var uri = new Uri(template, UriKind.Relative);
        var qs = nvc.ToString();
        if (!IsNullOrEmpty(qs))
        {
            uri = new Uri($"{uri}?{qs}", UriKind.Relative);
        }

        return (uri, content);
    }

    private static async Task CheckStatusCodeAsync(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode) return;
        var errorMessage = $"HTTP Code {(int)response.StatusCode} ({SplitCamelCase(response.StatusCode.ToString())})";

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
        return System.Text.RegularExpressions.Regex.Replace(input, "([A-Z])", " $1", System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
    }

    private static (string, HttpMethod, MethodInfo) GetTemplate(MethodCallExpression body)
    {
        var template = typeof(TController).GetCustomAttribute<RouteAttribute>()?.Template ?? ControllerTemplate;
        var controllerName = typeof(TController).Name;
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