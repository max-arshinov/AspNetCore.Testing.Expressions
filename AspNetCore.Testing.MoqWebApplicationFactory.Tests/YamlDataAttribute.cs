using System.Reflection;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit.Sdk;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace AspNetCore.Testing.MoqWebApplicationFactory.Tests;

public class YamlDataAttribute : DataAttribute
{
    private readonly string _filePath;

    public YamlDataAttribute(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentNullException(nameof(filePath), "filePath can't be empty");
        }
        
        _filePath = filePath;
    }

    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        var parameters = testMethod.DeclaringType
            ?.GetConstructors()
            .SelectMany(x => x.GetParameters())
            .Select(x => x.ParameterType)
            .ToArray();

        var isWebFactory =
            parameters?.Any(x => IsAssignableToGenericType(x, typeof(WebApplicationFactory<>))) == true;
        
        // Get the absolute path to the JSON file
        var filePath = _filePath;
        
        var path = GetPath(filePath);

        if (!File.Exists(path))
        {
            path = isWebFactory 
                ? GetPath(filePath + ".waf")
                : GetPath(filePath + ".http");
        }

        if (!File.Exists(path))
        {
            throw new ArgumentException($"Could not find file at path: {path}");
        }

        // Load the file
        var fileData = File.ReadAllText(path);

        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)  // see height_in_inches in sample yml 
            .Build();
        
        var data = deserializer.Deserialize<List<object[]>>(fileData);
        return data;
    }

    private static string GetPath(string filePath)
    {
        if (!filePath.EndsWith(".yml"))
        {
            filePath += ".yml";
        }

        var path = Path.IsPathRooted(filePath)
            ? filePath
            : Path.GetRelativePath(Directory.GetCurrentDirectory(), filePath);
        return path;
    }

    public static bool IsAssignableToGenericType(Type givenType, Type genericType)
    {
        var interfaceTypes = givenType.GetInterfaces();

        foreach (var it in interfaceTypes)
        {
            if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
                return true;
        }

        if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
            return true;

        var baseType = givenType.BaseType;
        if (baseType == null) return false;

        return IsAssignableToGenericType(baseType, genericType);
    }
}