using System.Text.Json;

namespace MoldcellTests.Support;

/// <summary>Environment settings. Values come from environment variables (run_tests.sh loads .env) with defaults.</summary>
public static class Settings
{
    public static string AppiumUrl => Env("APPIUM_URL", "http://127.0.0.1:4723");
    public static string DeviceName => Env("DEVICE_NAME", "emulator-5554");
    public static string AppPackage => Env("APP_PACKAGE", "md.moldcell.selfservice");
    public static string AppActivity => Env("APP_ACTIVITY", ".screens.splash.SplashActivity");
    public static TimeSpan ExplicitWait => TimeSpan.FromSeconds(int.Parse(Env("EXPLICIT_WAIT_SEC", "20")));
    // bin/<config>/<tfm>/ -> automation/artifacts
    public static string ArtifactsDir =>
        Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, "..", "..", "..", "artifacts"));

    private static string Env(string name, string fallback) =>
        Environment.GetEnvironmentVariable(name) is { Length: > 0 } value ? value : fallback;

    /// <summary>
    /// Test data lives in test_data.json; any key can be overridden by an env var of the same name in upper case.
    /// </summary>
    public static IReadOnlyDictionary<string, string> LoadTestData()
    {
        var path = Path.Combine(TestContext.CurrentContext.TestDirectory, "test_data.json");
        var data = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(path))
                   ?? throw new InvalidOperationException($"Empty test data file: {path}");
        foreach (var key in data.Keys.ToList())
        {
            if (Environment.GetEnvironmentVariable(key.ToUpperInvariant()) is { Length: > 0 } value)
                data[key] = value;
        }
        return data;
    }
}
