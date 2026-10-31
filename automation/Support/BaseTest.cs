using NUnit.Framework.Interfaces;
using OpenQA.Selenium.Appium.Android;

namespace MoldcellTests.Support;

public abstract class BaseTest
{
    protected TestSettings Settings = null!;
    protected AndroidDriver Driver = null!;
    protected TestData TestData = null!;

    [SetUp]
    public void StartSession()
    {
        Settings = TestSettings.FromEnvironment();
        TestData = TestDataLoader.Load();
        Driver = new DriverFactory(Settings).Create();
    }

    [TearDown]
    public void StopSession()
    {
        try
        {
            // SetUp may have failed before the session was created.
            if (Driver is not null && TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
                new FailureArtifacts(Driver).Save();
        }
        finally
        {
            Driver?.Dispose(); // Dispose() also quits the Appium session
        }
    }
}
