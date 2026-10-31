using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Android;

namespace MoldcellTests.Pages;

/// <summary>First-launch screen: language selection (classic Android views).</summary>
public class LanguagePage(AndroidDriver driver) : BasePage(driver)
{
    private const string Pkg = "md.moldcell.selfservice:id/";

    public static readonly By Logo = By.Id(Pkg + "logo_image_view");
    public static readonly By Title = By.Id(Pkg + "txt_title");
    public static readonly IReadOnlyDictionary<string, By> LanguageRadio = new Dictionary<string, By>
    {
        ["Română"] = By.Id(Pkg + "radio_btn_ro"),
        ["Русский"] = By.Id(Pkg + "radio_btn_ru"),
        ["English"] = By.Id(Pkg + "radio_btn_en"),
    };
    public static readonly By Continue = By.Id(Pkg + "btn_change_language");

    public LanguagePage WaitLoaded()
    {
        Find(Title);
        return this;
    }

    public void ChooseLanguage(string language)
    {
        Tap(LanguageRadio[language]);
        Tap(Continue);
    }
}
