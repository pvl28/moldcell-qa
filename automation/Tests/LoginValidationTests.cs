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
        new LanguagePage(Driver).WaitLoaded().ChooseLanguage(TestData["language"]);
        var loginPage = new LoginPage(Driver).WaitLoaded();

        // Action: submit an incomplete number with any password.
        loginPage.LogIn(TestData["invalid_short_phone"], TestData["dummy_password"]);

        // Check: inline format error is shown, user stays on the login screen.
        Assert.Multiple(() =>
        {
            Assert.That(loginPage.TextOf(LoginPage.UsernameError), Is.EqualTo(TestData["expected_username_format_error"]),
                "Wrong or missing validation message for an incomplete phone number");
            Assert.That(loginPage.TextOf(LoginPage.Username), Is.EqualTo(TestData["invalid_short_phone"]),
                "Entered username should be kept after a validation error");
            Assert.That(loginPage.IsVisible(LoginPage.Login), Is.True, "User should stay on the login screen");
        });
    }
}
