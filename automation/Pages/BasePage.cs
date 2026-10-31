using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Support.UI;
using MoldcellTests.Support;

namespace MoldcellTests.Pages;

public abstract class BasePage
{
    protected readonly AndroidDriver Driver;
    private readonly WebDriverWait _wait;

    protected BasePage(AndroidDriver driver)
    {
        Driver = driver;
        _wait = new WebDriverWait(driver, Settings.ExplicitWait);
        _wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));
    }

    /// <summary>Compose testTags are exposed as bare resource-ids (no package prefix), so By.Id cannot be used for them.</summary>
    protected static By ByResourceId(string resourceId) =>
        MobileBy.AndroidUIAutomator($"new UiSelector().resourceId(\"{resourceId}\")");

    public IWebElement Find(By locator)
    {
        _wait.Message = $"Element not visible: {locator}";
        return _wait.Until(d => d.FindElement(locator) is { Displayed: true } e ? e : null)!;
    }

    public void Tap(By locator)
    {
        _wait.Message = $"Element not clickable: {locator}";
        _wait.Until(d => d.FindElement(locator) is { Displayed: true, Enabled: true } e ? e : null)!.Click();
    }

    public void Type(By locator, string text)
    {
        var field = Find(locator);
        field.Click();
        field.Clear();
        field.SendKeys(text);
    }

    public string TextOf(By locator) => Find(locator).Text;

    public bool IsVisible(By locator) => Find(locator).Displayed;

    public void HideKeyboard()
    {
        if (Driver.IsKeyboardShown())
            Driver.HideKeyboard();
    }
}
