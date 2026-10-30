from pages.base_page import BasePage, by_resource_id


class LoginPage(BasePage):
    """Login screen (Jetpack Compose, testTags exposed as resource-id)."""

    TITLE = by_resource_id("login_title_second")
    USERNAME = by_resource_id("login_input")
    USERNAME_ERROR = by_resource_id("login_input_error_message")
    PASSWORD = by_resource_id("password_input")
    PASSWORD_TOGGLE = by_resource_id("hide_password_image")
    FORGOT_PASSWORD = by_resource_id("forgot_password_button")
    LOGIN = by_resource_id("login_button")
    SIGN_UP = by_resource_id("create_account_button")
    TERMS = by_resource_id("terms_and_conditions_button")

    KEY_ELEMENTS = {
        "title": TITLE,
        "username field": USERNAME,
        "password field": PASSWORD,
        "forgot password link": FORGOT_PASSWORD,
        "login button": LOGIN,
        "sign up link": SIGN_UP,
    }

    def wait_loaded(self) -> "LoginPage":
        self.find(self.LOGIN)
        return self

    def login(self, username: str, password: str) -> None:
        self.type(self.USERNAME, username)
        self.type(self.PASSWORD, password)
        self.hide_keyboard()
        self.tap(self.LOGIN)
