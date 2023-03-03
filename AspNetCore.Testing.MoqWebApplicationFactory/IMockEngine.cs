using Moq;

namespace AspNetCore.Testing.MoqWebApplicationFactory;

public interface IMockEngine
{
    public Mock<T> Mock<T>() where T : class;
}