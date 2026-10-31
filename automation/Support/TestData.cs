namespace MoldcellTests.Support;

/// <summary>Typed view of test_data.json (snake_case keys in the file).</summary>
public sealed record TestData
{
    public required Language Language { get; init; }
    public required string InvalidShortPhone { get; init; }
    public required string DummyPassword { get; init; }
    public required string ExpectedLanguageTitle { get; init; }
    public required string ExpectedLoginTitle { get; init; }
    public required string ExpectedUsernameFormatError { get; init; }
}
