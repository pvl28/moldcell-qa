using MoldcellTests.Support;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Android;

namespace MoldcellTests.Pages;

/// <summary>First-launch screen: language selection (classic Android views).</summary>
public class LanguagePage(AndroidDriver driver, TestSettings settings) : BasePage(driver, settings)
{
    private const string Pkg = "md.moldcell.selfservice:id/";

    private static readonly By Logo = By.Id(Pkg + "logo_image_view");
    private static readonly By Title = By.Id(Pkg + "txt_title");
    private static readonly IReadOnlyDictionary<Language, By> LanguageRadio = new Dictionary<Language, By>
    {
        [Language.Romanian] = By.Id(Pkg + "radio_btn_ro"),
        [Language.Russian] = By.Id(Pkg + "radio_btn_ru"),
        [Language.English] = By.Id(Pkg + "radio_btn_en"),
    };
    private static readonly By Continue = By.Id(Pkg + "btn_change_language");

    protected override By LoadedMarker => Title;

    public string GetTitle() => TextOf(Title);

    public IReadOnlyList<Language> GetMissingLanguageOptions() =>
        LanguageRadio.Where(option => !IsVisible(option.Value)).Select(option => option.Key).ToList();

    public LoginPage ChooseLanguage(Language language)
    {
        Tap(LanguageRadio[language]);
        Tap(Continue);
        var loginPage = new LoginPage(Driver, Settings);
        loginPage.WaitLoaded();
        return loginPage;
    }
}
