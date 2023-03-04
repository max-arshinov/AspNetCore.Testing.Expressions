using System.Linq.Expressions;
using System.Reflection;

namespace AspNetCore.Testing.Expressions;

public static class TestData
{
    public static TestDataBuilder<TController> With<TController>()
    {
        throw new NotImplementedException();
    }
}

public class TestDataBuilder<TController>
{
    public TestDataBuilder<TController, TResponse> Setup<TResponse>(
        Expression<Func<TController, TResponse>> testMethodExpression)
    {
        var b = new TestDataBuilder<TController, TResponse>();
        return b;
    }
}

public class TestDataBuilder<TController, TResponse>
{

}