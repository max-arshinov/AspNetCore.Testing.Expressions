using Moq;

namespace AspNetCore.Testing.MoqWebApplicationFactory;

internal class MockEngine: IMockEngine
{
    public IDictionary<Type, object> Mocks { get; } = new Dictionary<Type, object>();
    
    public Mock<T> Mock<T>() where T : class
    {
        var mock = new Mock<T>();
        Mocks[typeof(T)] = mock;
        return mock;
    }
}