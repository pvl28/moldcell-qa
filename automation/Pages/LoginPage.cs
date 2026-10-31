using MoldcellTests.Support;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Android;

namespace MoldcellTests.Pages;

/// <summary>Login screen (Jetpack Compose, testTags exposed as resource-id).</summary>
public class LoginPage(AndroidDriver driver, TestSettings settings) : BasePage(driver, settings)
{
    private static readonly By Title = ByResourceId("login_title_second");
    private static readonly By Username = ByResourceId("login_input");
    private static readonly By UsernameError = ByResourceId("login_input_error_message");
    private static readonly By Password = ByResourceId("password_input");
    private static readonly By PasswordToggle = ByResourceId("hide_password_image");
    private static readonly By ForgotPassword = ByResourceId("forgot_password_button");
    private static readonly By Login = ByResourceId("login_button");
    private static readonly By SignUp = ByResourceId("create_account_button");
    private static readonly By Terms = ByResourceId("terms_and_conditions_button");

    private static readonly IReadOnlyDictionary<string, By> KeyElements = new Dictionary<string, By>
    {
        ["title"] = Title,
        ["username field"] = Username,
        ["password field"] = Password,
        ["forgot password link"] = ForgotPassword,
        ["login button"] = Login,
        ["sign up link"] = SignUp,
    };

    protected override By LoadedMarker => Login;

    public string GetTitle() => TextOf(Title);

    public string GetUsername() => TextOf(Username);

    public string GetUsernameError() => TextOf(UsernameError);

    public IReadOnlyList<string> GetMissingKeyElements() =>
        KeyElements.Where(element => !IsVisible(element.Value)).Select(element => element.Key).ToList();

    public void LogIn(string username, string password)
    {
        Type(Username, username);
        Type(Password, password);
        HideKeyboard();
        Tap(Login);
    }
}
