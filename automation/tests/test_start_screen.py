"""Checklist CL-01 (High): first launch shows language screen and leads to the login screen with key elements."""
from pages.language_page import LanguagePage
from pages.login_page import LoginPage


def test_first_launch_language_then_login_screen(driver, test_data):
    # Precondition: clean app data (noReset=false), notifications permission granted.
    language_page = LanguagePage(driver).wait_loaded()

    assert test_data["expected_language_title"] in language_page.text_of(LanguagePage.TITLE), \
        "Language screen title is not shown on first launch"
    for name, locator in LanguagePage.LANGUAGE_RADIO.items():
        assert language_page.is_visible(locator), f"Language option '{name}' is not visible"

    # Action: choose language and continue.
    language_page.choose_language(test_data["language"])

    # Check: login screen with all key elements.
    login_page = LoginPage(driver).wait_loaded()
    assert login_page.text_of(LoginPage.TITLE) == test_data["expected_login_title"], "Unexpected login screen title"
    missing = [name for name, loc in LoginPage.KEY_ELEMENTS.items() if not login_page.is_visible(loc)]
    assert not missing, f"Key elements missing on login screen: {missing}"
