from appium.webdriver.common.appiumby import AppiumBy

from pages.base_page import BasePage

_PKG = "md.moldcell.selfservice:id/"


class LanguagePage(BasePage):
    """First-launch screen: language selection (classic Android views)."""

    LOGO = (AppiumBy.ID, _PKG + "logo_image_view")
    TITLE = (AppiumBy.ID, _PKG + "txt_title")
    LANGUAGE_RADIO = {
        "Română": (AppiumBy.ID, _PKG + "radio_btn_ro"),
        "Русский": (AppiumBy.ID, _PKG + "radio_btn_ru"),
        "English": (AppiumBy.ID, _PKG + "radio_btn_en"),
    }
    CONTINUE = (AppiumBy.ID, _PKG + "btn_change_language")

    def wait_loaded(self) -> "LanguagePage":
        self.find(self.TITLE)
        return self

    def choose_language(self, language: str) -> None:
        self.tap(self.LANGUAGE_RADIO[language])
        self.tap(self.CONTINUE)
