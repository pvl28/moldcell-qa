using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace MoldcellTests.Support;

public static class TestDataLoader
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        Converters = { new JsonStringEnumConverter() },
    };

    /// <summary>Test data lives in test_data.json; any key can be overridden by an env var of the same name in upper case.</summary>
    public static TestData Load()
    {
        var path = Path.Combine(TestContext.CurrentContext.TestDirectory, "test_data.json");
        var json = JsonNode.Parse(File.ReadAllText(path)) as JsonObject
                   ?? throw new InvalidOperationException($"Empty test data file: {path}");
        foreach (var key in json.Select(property => property.Key).ToList())
        {
            if (Environment.GetEnvironmentVariable(key.ToUpperInvariant()) is { Length: > 0 } value)
                json[key] = value;
        }
        return json.Deserialize<TestData>(Options)!;
    }
}
