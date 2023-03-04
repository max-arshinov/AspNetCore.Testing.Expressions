using System.Reflection;
using Xunit.Sdk;

namespace AspNetCore.Testing.MoqWebApplicationFactory.Tests;

public class SmartDataAttribute: DataAttribute
{
    public SmartDataAttribute(Type t)
    {
    }

    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        yield return new object[] { "http://google.com" };
    }
}