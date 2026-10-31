using MoldcellTests.Pages;
using MoldcellTests.Support;

namespace MoldcellTests.Tests;

/// <summary>Checklist CL-04 (High): client-side validation rejects an incomplete phone number.</summary>
public class LoginValidationTests : BaseTest
{
    [Test]
    public void Login_WithIncompletePhone_ShowsFormatError()
    {
        // Precondition: user is on the login screen (language chosen on a clean install).
        var languagePage = new LanguagePage(Driver, Settings);
        languagePage.WaitLoaded();
        var loginPage = languagePage.ChooseLanguage(TestData.Language);

        // Action: submit an incomplete number with any password.
        loginPage.LogIn(TestData.InvalidShortPhone, TestData.DummyPassword);

        // Check: inline format error is shown, user stays on the login screen.
        Assert.Multiple(() =>
        {
            Assert.That(loginPage.GetUsernameError(), Is.EqualTo(TestData.ExpectedUsernameFormatError),
                "Wrong or missing validation message for an incomplete phone number");
            Assert.That(loginPage.GetUsername(), Is.EqualTo(TestData.InvalidShortPhone),
                "Entered username should be kept after a validation error");
            Assert.That(loginPage.IsOpen(), Is.True, "User should stay on the login screen");
        });
    }
}
