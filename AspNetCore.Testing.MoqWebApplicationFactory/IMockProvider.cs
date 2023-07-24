using Moq;

namespace AspNetCore.Testing.MoqWebApplicationFactory;

public interface IMockProvider: IEnumerable<KeyValuePair<Type, Mock>>
{
    Mock<T> Mock<T>() where T : class;
    
    IMockProvider Merge(IMockProvider namedMockProvider);
}