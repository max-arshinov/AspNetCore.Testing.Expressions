using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Testing.Expressions;

public static class HttpClientExtensions
{
    public static Task<TResponse?> GetAsync<TController, TResponse>(this HttpClient httpClient, 
        Expression<Func<TController, Task<ActionResult<TResponse>>>> expression) =>
        new RequestSender<TController>(httpClient).SendAsync<TResponse>(expression);

    public static Task<TResponse?> GetAsync<TController, TResponse>(this HttpClient httpClient,
        Expression<Func<TController, ActionResult<TResponse>>> expression) =>
        new RequestSender<TController>(httpClient).SendAsync<TResponse>(expression);

    public static Task<TResponse?> GetAsync<TController, TResponse>(this HttpClient httpClient, 
        Expression<Func<TController, TResponse>> expression)
    {
        throw new NotImplementedException();
    }

    public static Task<TResponse?> GetAsync<TController, TResponse>(this HttpClient httpClient, 
        Expression<Func<TController, Task<TResponse>>> expression)
    {
        throw new NotImplementedException();
    }

}