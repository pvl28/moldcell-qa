using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Android;

namespace MoldcellTests.Pages;

/// <summary>Login screen (Jetpack Compose, testTags exposed as resource-id).</summary>
public class LoginPage(AndroidDriver driver) : BasePage(driver)
{
    public static readonly By Title = ByResourceId("login_title_second");
    public static readonly By Username = ByResourceId("login_input");
    public static readonly By UsernameError = ByResourceId("login_input_error_message");
    public static readonly By Password = ByResourceId("password_input");
    public static readonly By PasswordToggle = ByResourceId("hide_password_image");
    public static readonly By ForgotPassword = ByResourceId("forgot_password_button");
    public static readonly By Login = ByResourceId("login_button");
    public static readonly By SignUp = ByResourceId("create_account_button");
    public static readonly By Terms = ByResourceId("terms_and_conditions_button");

    public static readonly IReadOnlyDictionary<string, By> KeyElements = new Dictionary<string, By>
    {
        ["title"] = Title,
        ["username field"] = Username,
        ["password field"] = Password,
        ["forgot password link"] = ForgotPassword,
        ["login button"] = Login,
        ["sign up link"] = SignUp,
    };

    public LoginPage WaitLoaded()
    {
        Find(Login);
        return this;
    }

    public void LogIn(string username, string password)
    {
        Type(Username, username);
        Type(Password, password);
        HideKeyboard();
        Tap(Login);
    }
}
