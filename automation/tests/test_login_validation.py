"""Checklist CL-04 (High): client-side validation rejects an incomplete phone number."""
from pages.language_page import LanguagePage
from pages.login_page import LoginPage


def test_login_with_incomplete_phone_shows_format_error(driver, test_data):
    # Precondition: user is on the login screen (language chosen on a clean install).
    LanguagePage(driver).wait_loaded().choose_language(test_data["language"])
    login_page = LoginPage(driver).wait_loaded()

    # Action: submit an incomplete number with any password.
    login_page.login(test_data["invalid_short_phone"], test_data["dummy_password"])

    # Check: inline format error is shown, user stays on the login screen.
    assert login_page.text_of(LoginPage.USERNAME_ERROR) == test_data["expected_username_format_error"], \
        "Wrong or missing validation message for an incomplete phone number"
    assert login_page.text_of(LoginPage.USERNAME) == test_data["invalid_short_phone"], \
        "Entered username should be kept after a validation error"
    assert login_page.is_visible(LoginPage.LOGIN), "User should stay on the login screen"
