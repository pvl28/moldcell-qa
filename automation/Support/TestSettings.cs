namespace MoldcellTests.Support;

/// <summary>Immutable environment settings. Values come from environment variables (run_tests.sh loads .env) with defaults.</summary>
public sealed record TestSettings(
    string AppiumUrl,
    string DeviceName,
    string AppPackage,
    string AppActivity,
    TimeSpan ExplicitWait)
{
    public static TestSettings FromEnvironment() => new(
        Env("APPIUM_URL", "http://127.0.0.1:4723"),
        Env("DEVICE_NAME", "emulator-5554"),
        Env("APP_PACKAGE", "md.moldcell.selfservice"),
        Env("APP_ACTIVITY", ".screens.splash.SplashActivity"),
        TimeSpan.FromSeconds(int.Parse(Env("EXPLICIT_WAIT_SEC", "20"))));

    private static string Env(string name, string fallback) =>
        Environment.GetEnvironmentVariable(name) is { Length: > 0 } value ? value : fallback;
}
