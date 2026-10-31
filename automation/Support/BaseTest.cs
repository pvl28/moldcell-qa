using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;

namespace MoldcellTests.Support;

public abstract class BaseTest
{
    protected AndroidDriver Driver = null!;
    protected IReadOnlyDictionary<string, string> TestData = null!;

    [SetUp]
    public void StartSession()
    {
        TestData = Settings.LoadTestData();
        Driver = new AndroidDriver(new Uri(Settings.AppiumUrl), Capabilities(), TimeSpan.FromSeconds(180));
        Driver.Manage().Timeouts().ImplicitWait = TimeSpan.Zero; // only explicit waits are used (see BasePage)
    }

    [TearDown]
    public void StopSession()
    {
        try
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
                SaveFailureArtifacts();
        }
        finally
        {
            Driver?.Dispose(); // Dispose() also quits the Appium session
        }
    }

    private static AppiumOptions Capabilities()
    {
        var options = new AppiumOptions
        {
            PlatformName = "Android",
            AutomationName = AutomationName.AndroidUIAutomator2,
            DeviceName = Settings.DeviceName,
        };
        options.AddAdditionalAppiumOption("appPackage", Settings.AppPackage);
        options.AddAdditionalAppiumOption("appActivity", Settings.AppActivity);
        // noReset=false -> Appium runs `pm clear` before each session, so every test
        // starts from a fresh install state (language screen) and tests stay independent.
        options.AddAdditionalAppiumOption("noReset", false);
        options.AddAdditionalAppiumOption("fullReset", false);
        // Grants POST_NOTIFICATIONS up front so the system permission dialog does not overlay the app.
        options.AddAdditionalAppiumOption("autoGrantPermissions", true);
        options.AddAdditionalAppiumOption("newCommandTimeout", 120);
        return options;
    }

    /// <summary>On test failure save a screenshot and the page source into automation/artifacts/.</summary>
    private void SaveFailureArtifacts()
    {
        try
        {
            Directory.CreateDirectory(Settings.ArtifactsDir);
            var stem = Path.Combine(Settings.ArtifactsDir, $"{TestContext.CurrentContext.Test.Name}_{DateTime.Now:yyyyMMdd_HHmmss}");
            Driver.GetScreenshot().SaveAsFile(stem + ".png");
            File.WriteAllText(stem + ".xml", Driver.PageSource);
            TestContext.AddTestAttachment(stem + ".png", "Screenshot on failure");
            TestContext.AddTestAttachment(stem + ".xml", "Page source on failure");
            TestContext.Out.WriteLine($"Failure artifacts saved: {stem}.png|.xml");
        }
        catch (WebDriverException ex) // session may already be dead
        {
            TestContext.Out.WriteLine($"Could not collect failure artifacts: {ex.Message}");
        }
    }
}
