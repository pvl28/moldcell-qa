using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;

namespace MoldcellTests.Support;

/// <summary>Creates Appium sessions for the app under test.</summary>
public sealed class DriverFactory
{
    private readonly TestSettings _settings;

    public DriverFactory(TestSettings settings)
    {
        _settings = settings;
    }

    public AndroidDriver Create()
    {
        var driver = new AndroidDriver(new Uri(_settings.AppiumUrl), Capabilities(), TimeSpan.FromSeconds(180));
        try
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.Zero; // only explicit waits are used (see BasePage)
            return driver;
        }
        catch
        {
            driver.Dispose(); // do not leave a half-configured session on the Appium server
            throw;
        }
    }

    private AppiumOptions Capabilities()
    {
        var options = new AppiumOptions
        {
            PlatformName = "Android",
            AutomationName = AutomationName.AndroidUIAutomator2,
            DeviceName = _settings.DeviceName,
        };
        options.AddAdditionalAppiumOption("appPackage", _settings.AppPackage);
        options.AddAdditionalAppiumOption("appActivity", _settings.AppActivity);
        // noReset=false -> Appium runs `pm clear` before each session, so every test
        // starts from a fresh install state (language screen) and tests stay independent.
        options.AddAdditionalAppiumOption("noReset", false);
        options.AddAdditionalAppiumOption("fullReset", false);
        // Grants POST_NOTIFICATIONS up front so the system permission dialog does not overlay the app.
        options.AddAdditionalAppiumOption("autoGrantPermissions", true);
        options.AddAdditionalAppiumOption("newCommandTimeout", 120);
        return options;
    }
}
