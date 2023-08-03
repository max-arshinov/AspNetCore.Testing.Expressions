using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Testing.Expressions;

public static class HttpClientExtensions
{
    public static Task<TResponse?> SendAsync<TController, TResponse>(this HttpClient httpClient, 
        Expression<Func<TController, Task<ActionResult<TResponse>>>> expression, bool acceptErrorStatusCodes = false) =>
        new ControllerClient<TController>(httpClient).DoSendAsync<TResponse>(expression, acceptErrorStatusCodes);

    public static Task<TResponse?> SendAsync<TController, TResponse>(this HttpClient httpClient,
        Expression<Func<TController, ActionResult<TResponse>>> expression, bool acceptErrorStatusCodes = false) =>
        new ControllerClient<TController>(httpClient).DoSendAsync<TResponse>(expression, acceptErrorStatusCodes);

    public static Task<TResponse?> SendAsync<TController, TResponse>(this HttpClient httpClient, 
        Expression<Func<TController, TResponse>> expression, bool acceptErrorStatusCodes = false) =>
        new ControllerClient<TController>(httpClient).DoSendAsync<TResponse>(expression, acceptErrorStatusCodes);

    public static Task<TResponse?> SendAsync<TController, TResponse>(this HttpClient httpClient, 
        Expression<Func<TController, Task<TResponse>>> expression, bool acceptErrorStatusCodes = false) =>
        new ControllerClient<TController>(httpClient).DoSendAsync<TResponse>(expression, acceptErrorStatusCodes);
}