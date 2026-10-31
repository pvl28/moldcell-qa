using MoldcellTests.Pages;
using MoldcellTests.Support;

namespace MoldcellTests.Tests;

/// <summary>Checklist CL-01 (High): first launch shows language screen and leads to the login screen with key elements.</summary>
public class StartScreenTests : BaseTest
{
    [Test]
    public void FirstLaunch_LanguageThenLoginScreen()
    {
        // Precondition: clean app data (noReset=false), notifications permission granted.
        var languagePage = new LanguagePage(Driver, Settings);
        languagePage.WaitLoaded();

        Assert.That(languagePage.GetTitle(), Does.Contain(TestData.ExpectedLanguageTitle),
            "Language screen title is not shown on first launch");
        Assert.That(languagePage.GetMissingLanguageOptions(), Is.Empty, "Language options are not visible");

        // Action: choose language and continue.
        var loginPage = languagePage.ChooseLanguage(TestData.Language);

        // Check: login screen with all key elements.
        Assert.That(loginPage.GetTitle(), Is.EqualTo(TestData.ExpectedLoginTitle),
            "Unexpected login screen title");
        Assert.That(loginPage.GetMissingKeyElements(), Is.Empty, "Key elements missing on login screen");
    }
}
