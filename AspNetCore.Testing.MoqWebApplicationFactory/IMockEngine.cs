using Moq;

namespace AspNetCore.Testing.MoqWebApplicationFactory;

public interface IMockEngine
{
    Mock<T> Mock<T>() where T : class;
    
    IDictionary<Type, object> Mocks { get; }
}