namespace AspNetCore.Testing.Expressions.Tests;

internal static class Extensions
{
    public static T NotNull<T>(this T? obj)
    {
        obj.Should().NotBeNull();
        return obj!;
    }
    
    public static async Task<T> NotNull<T>(this Task<T?> task)
    {
        var obj = await task;
        obj.Should().NotBeNull();
        return obj!;
    }
}