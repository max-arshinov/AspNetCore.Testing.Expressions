namespace AspNetCore.Testing.Expressions;

public abstract class ApiComparerTestsBase
{
    protected abstract string? ExpectedBaseAddress { get; }
    
    protected abstract string? ActualBaseAddress { get; }

    protected abstract HttpClient CreateHttpClient(string? baseAddress);

    protected virtual HttpClient CreateExpectedHttpClient(string? baseAddress) => CreateHttpClient(baseAddress);
    
    protected virtual HttpClient CreateActualHttpClient(string? baseAddress) => CreateHttpClient(baseAddress);

    protected async Task CompareAsync<T>(Func<HttpClient, Task<T>> expected, Func<HttpClient, Task<T>> actual, 
        Action<T,T> comparer)
    {
        var expectedHttpClient = CreateExpectedHttpClient(ExpectedBaseAddress);
        var actualHttpClient = CreateActualHttpClient(ActualBaseAddress);

        var apiComparer = new ApiComparer<T>(expected, actual);
        await apiComparer.CompareAsync(expectedHttpClient, actualHttpClient, comparer);
    }
    
    protected async Task MutateAndCompareAsync<T,TM>(
        Func<HttpClient, Task<TM>> mutate,
        Func<HttpClient, Task<T>> expected, Func<HttpClient, Task<T>> actual, 
        Action<T,T> comparer)
    {
        var expectedHttpClient = CreateExpectedHttpClient(ExpectedBaseAddress);
        var actualHttpClient = CreateActualHttpClient(ActualBaseAddress);

        await mutate(expectedHttpClient);
        var apiComparer = new ApiComparer<T>(expected, actual);
        await apiComparer.CompareAsync(expectedHttpClient, actualHttpClient, comparer);
        
        await mutate(actualHttpClient);
        expectedHttpClient = CreateExpectedHttpClient(ExpectedBaseAddress);
        actualHttpClient = CreateActualHttpClient(ActualBaseAddress);
        await apiComparer.CompareAsync(expectedHttpClient, actualHttpClient, comparer);
    }  
    
    protected async Task MutateAndCompareAsync<T>(
        Func<HttpClient, Task> mutate,
        Func<HttpClient, Task<T>> expected, Func<HttpClient, Task<T>> actual, 
        Action<T,T> comparer)
    {
        var expectedHttpClient = CreateExpectedHttpClient(ExpectedBaseAddress);
        var actualHttpClient = CreateActualHttpClient(ActualBaseAddress);

        await mutate(expectedHttpClient);
        var apiComparer = new ApiComparer<T>(expected, actual);
        await apiComparer.CompareAsync(expectedHttpClient, actualHttpClient, comparer);
        
        await mutate(actualHttpClient);
        expectedHttpClient = CreateExpectedHttpClient(ExpectedBaseAddress);
        actualHttpClient = CreateActualHttpClient(ActualBaseAddress);
        await apiComparer.CompareAsync(expectedHttpClient, actualHttpClient, comparer);
    }     
}