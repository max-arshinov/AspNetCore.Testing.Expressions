using System.Collections.Specialized;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using RouteAttribute = Microsoft.AspNetCore.Components.RouteAttribute;

namespace AspNetCore.Testing.Expressions;

internal class RequestMessageBuilder
{
    private const string ControllerTemplate = "[controller]";

    public HttpRequestMessage Build(MethodCallExpression expression)
    {
        var (template, httpMethod, methodInfo) = GetTemplate(expression);
        var (uri, content) = GetUriAndHttpContent(template, methodInfo, expression.Arguments.ToArray());

        return new HttpRequestMessage
        {
            RequestUri = uri,
            Method = httpMethod,
            Content = content
        };
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
            .Where(x => x.GetCustomAttribute<FromServicesAttribute>() == null)
            .ToArray();

        var nvc = HttpUtility.ParseQueryString(string.Empty);
        HttpContent? content = null;
        
        for(var i = 0; i < parameters.Length; i ++)
        {
            var parameter = parameters[i];
            template = ApplyParameter(template, parameterExpressions, parameter, i, nvc, ref content);

            if (parameter.ParameterType.Namespace?.StartsWith("System") != true)
            {
                var parameterProperties = parameter
                    .ParameterType
                    .GetProperties()
                    .Where(x => x is { CanRead: true, CanWrite: true })
                    .ToArray();

                foreach (var property in parameterProperties)
                {
                    template = ApplyParameter(template, parameterExpressions, parameter, i, nvc, 
                        ref content, property);
                }
            }
        }

        var uri = new Uri(template, UriKind.Relative);
        var qs = nvc.ToString();
        if (!string.IsNullOrEmpty(qs))
        {
            uri = new Uri($"{uri}?{qs}", UriKind.Relative);
        }

        return (uri, content);
    }

    private static string ApplyParameter(string template, Expression[] parameterExpressions, ParameterInfo parameter, 
        int i, NameValueCollection nvc, ref HttpContent? content, PropertyInfo? propertyParameterInfo = null)
    {
        var routeAttr = GetAttribute<FromRouteAttribute>(parameter, propertyParameterInfo);
        var queryAttr = GetAttribute<FromQueryAttribute>(parameter, propertyParameterInfo);
        var bodyAttr = GetAttribute<FromBodyAttribute>(parameter, propertyParameterInfo);

        if (routeAttr != null)
        {
            var expressionValue = GetExpressionValue(parameterExpressions, parameter, i, propertyParameterInfo);
            // TODO: {id:type} scenario
            template = template.Replace($"{{{parameter.Name}}}", expressionValue?.ToString() ?? "");
        }

        else if (queryAttr != null)
        {
            var expressionValue = GetExpressionValue(parameterExpressions, parameter, i, propertyParameterInfo);
            nvc[propertyParameterInfo?.Name ?? parameter.Name] = expressionValue?.ToString() ?? "";
        }

        else if (bodyAttr != null)
        {
            var expressionValue = GetExpressionValue(parameterExpressions, parameter, i, propertyParameterInfo);
            content = JsonContent.Create(expressionValue);
        }

        return template;
    }

    private static T? GetAttribute<T>(ParameterInfo parameter, PropertyInfo? propertyParameterInfo)
        where T : Attribute =>
        propertyParameterInfo != null 
            ? propertyParameterInfo.GetCustomAttribute<T>() 
            : parameter.GetCustomAttribute<T>();
    
    private static object? GetExpressionValue(Expression[] parameterExpressions, ParameterInfo parameter, int i,
        PropertyInfo? propertyParameterInfo)
    {
        var expressionValue = InvokeExpression(parameterExpressions[i], parameter.ParameterType);
        if (!string.IsNullOrEmpty(propertyParameterInfo?.Name))
        {
            expressionValue = GetValue(expressionValue, propertyParameterInfo.Name);
        }

        return expressionValue;
    }

    private static (string, HttpMethod, MethodInfo) GetTemplate(MethodCallExpression body)
    {
        var controllerType = body.Method.DeclaringType ??
                             throw new NotSupportedException("Method call must have the declaring type");
        
        var template = controllerType.GetCustomAttribute<RouteAttribute>()?.Template ?? ControllerTemplate;
        var controllerName = controllerType.Name;
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

    private static object? GetValue(object? obj, string propertyName)
    {
        if (obj == null)
        {
            return null;
        }

        return obj.GetType().GetProperty(propertyName)?.GetValue(obj) ?? default;
    }
    
    private HttpContent? GetHttpContent(MethodInfo methodInfo)
    {
        return JsonContent.Create(new { });
    }
}