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
        var languagePage = new LanguagePage(Driver).WaitLoaded();

        Assert.That(languagePage.TextOf(LanguagePage.Title), Does.Contain(TestData["expected_language_title"]),
            "Language screen title is not shown on first launch");
        foreach (var (name, locator) in LanguagePage.LanguageRadio)
            Assert.That(languagePage.IsVisible(locator), Is.True, $"Language option '{name}' is not visible");

        // Action: choose language and continue.
        languagePage.ChooseLanguage(TestData["language"]);

        // Check: login screen with all key elements.
        var loginPage = new LoginPage(Driver).WaitLoaded();
        Assert.That(loginPage.TextOf(LoginPage.Title), Is.EqualTo(TestData["expected_login_title"]),
            "Unexpected login screen title");
        var missing = LoginPage.KeyElements.Where(e => !loginPage.IsVisible(e.Value)).Select(e => e.Key).ToList();
        Assert.That(missing, Is.Empty, "Key elements missing on login screen");
    }
}
