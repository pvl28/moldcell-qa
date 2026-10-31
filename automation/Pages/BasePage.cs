using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Support.UI;
using MoldcellTests.Support;

namespace MoldcellTests.Pages;

public abstract class BasePage
{
    protected readonly AndroidDriver Driver;
    protected readonly TestSettings Settings;
    private readonly WebDriverWait _wait;

    protected BasePage(AndroidDriver driver, TestSettings settings)
    {
        Driver = driver;
        Settings = settings;
        _wait = new WebDriverWait(driver, settings.ExplicitWait);
        _wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));
    }

    /// <summary>Element that is visible only while this page is open.</summary>
    protected abstract By LoadedMarker { get; }

    public void WaitLoaded() => Find(LoadedMarker);

    public bool IsOpen() => IsVisible(LoadedMarker);

    /// <summary>Compose testTags are exposed as bare resource-ids (no package prefix), so By.Id cannot be used for them.</summary>
    protected static By ByResourceId(string resourceId) =>
        MobileBy.AndroidUIAutomator($"new UiSelector().resourceId(\"{resourceId}\")");

    protected IWebElement Find(By locator)
    {
        _wait.Message = $"Element not visible: {locator}";
        return _wait.Until(d => d.FindElement(locator) is { Displayed: true } e ? e : null)!;
    }

    protected void Tap(By locator)
    {
        _wait.Message = $"Element not clickable: {locator}";
        _wait.Until(d => d.FindElement(locator) is { Displayed: true, Enabled: true } e ? e : null)!.Click();
    }

    protected void Type(By locator, string text)
    {
        var field = Find(locator);
        field.Click();
        field.Clear();
        field.SendKeys(text);
    }

    protected string TextOf(By locator) => Find(locator).Text;

    /// <summary>Waits up to the explicit timeout; returns false instead of throwing when the element does not appear.</summary>
    protected bool IsVisible(By locator)
    {
        try
        {
            Find(locator);
            return true;
        }
        catch (WebDriverTimeoutException)
        {
            return false;
        }
    }

    protected void HideKeyboard()
    {
        if (Driver.IsKeyboardShown())
            Driver.HideKeyboard();
    }
}
