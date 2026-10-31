using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Android;

namespace MoldcellTests.Support;

/// <summary>On test failure saves a screenshot and the page source into automation/artifacts/.</summary>
public sealed class FailureArtifacts
{
    private readonly AndroidDriver _driver;

    public FailureArtifacts(AndroidDriver driver)
    {
        _driver = driver;
    }

    // bin/<config>/<tfm>/ -> automation/artifacts
    private static string ArtifactsDir =>
        Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, "..", "..", "..", "artifacts"));

    public void Save()
    {
        try
        {
            Directory.CreateDirectory(ArtifactsDir);
            var stem = Path.Combine(ArtifactsDir, $"{TestContext.CurrentContext.Test.Name}_{DateTime.Now:yyyyMMdd_HHmmss}");
            _driver.GetScreenshot().SaveAsFile(stem + ".png");
            File.WriteAllText(stem + ".xml", _driver.PageSource);
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
