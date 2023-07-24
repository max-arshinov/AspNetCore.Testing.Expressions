namespace AspNetCore.Testing.Expressions;

public abstract class ApiComparerTestsBase
{
    protected abstract string? ExpectedBaseAddress { get; }
    
    protected abstract string? ActualBaseAddress { get; }

    protected abstract HttpClient CreateHttpClient(string? baseAddress);

    protected virtual HttpClient CreateExpectedHttpClient(string? baseAddress) => CreateHttpClient(baseAddress);
    
    protected virtual HttpClient CreateActualHttpClient(string? baseAddress) => CreateHttpClient(baseAddress);

    protected async Task CompareAsync<T>(Func<HttpClient, Task<T>> expected, Func<HttpClient, Task<T>> actual, 
        Action<T,T> comparer )
    {
        var expectedHttpClient = CreateExpectedHttpClient(ExpectedBaseAddress);
        var actualHttpClient = CreateExpectedHttpClient(ActualBaseAddress);

        var apiComparer = new ApiComparer<T>(expected, actual);
        await apiComparer.CompareAsync(expectedHttpClient, actualHttpClient, comparer);
    }
}