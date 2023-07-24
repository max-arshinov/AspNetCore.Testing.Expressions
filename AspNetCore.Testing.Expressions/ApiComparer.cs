namespace AspNetCore.Testing.Expressions;

public class ApiComparer<T>
{
    private readonly Func<HttpClient, Task<T>> _expected;

    private readonly Func<HttpClient, Task<T>> _actual;

    public ApiComparer(Func<HttpClient, Task<T>> expected, Func<HttpClient, Task<T>> actual)
    {
        _expected = expected;
        _actual = actual;
    }

    public async Task CompareAsync(HttpClient expectedClient, HttpClient actualClient, Action<T,T> comparator)
    {
        var et = _expected(expectedClient);
        var at = _actual(actualClient);

        var expected = await et;
        var actual = await at;
        comparator(expected, actual);
    }
}