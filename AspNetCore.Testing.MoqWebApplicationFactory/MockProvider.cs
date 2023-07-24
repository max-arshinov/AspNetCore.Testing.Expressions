using System.Collections;
using Moq;

namespace AspNetCore.Testing.MoqWebApplicationFactory;

internal class MockProvider: IMockProvider
{
    private Dictionary<Type, Mock> _mocks = new();
    
    public Mock<T> Mock<T>() where T : class
    {
        // ReSharper disable once InvertIf
        if (!_mocks.ContainsKey(typeof(T)))
        {
            var mock = new Mock<T>();
            _mocks[typeof(T)] = mock;
        }
        

        return (Mock<T>)_mocks[typeof(T)];
    }

    public IMockProvider Merge(IMockProvider namedMockProvider)
    {
        var mocks = _mocks.ToDictionary(x => x.Key, x => x.Value);
        foreach (var kv in namedMockProvider)
        {
            mocks[kv.Key] = kv.Value;
        }
        
        return new MockProvider()
        {
            _mocks = mocks
        };
    }

    public IEnumerator<KeyValuePair<Type, Mock>> GetEnumerator() => _mocks.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}