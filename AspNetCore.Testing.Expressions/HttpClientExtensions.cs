using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Testing.Expressions;

public static class HttpClientExtensions
{
    public static Task<TResponse?> SendAsync<TController, TResponse>(this HttpClient httpClient, 
        Expression<Func<TController, Task<ActionResult<TResponse>>>> expression) =>
        new ControllerClient<TController>(httpClient).DoSendAsync<TResponse>(expression);

    public static Task<TResponse?> SendAsync<TController, TResponse>(this HttpClient httpClient,
        Expression<Func<TController, ActionResult<TResponse>>> expression) =>
        new ControllerClient<TController>(httpClient).DoSendAsync<TResponse>(expression);

    public static Task<TResponse?> SendAsync<TController, TResponse>(this HttpClient httpClient, 
        Expression<Func<TController, TResponse>> expression) =>
        new ControllerClient<TController>(httpClient).DoSendAsync<TResponse>(expression);

    public static Task<TResponse?> SendAsync<TController, TResponse>(this HttpClient httpClient, 
        Expression<Func<TController, Task<TResponse>>> expression) =>
        new ControllerClient<TController>(httpClient).DoSendAsync<TResponse>(expression);
}