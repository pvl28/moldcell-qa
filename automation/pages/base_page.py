from appium.webdriver.common.appiumby import AppiumBy
from selenium.webdriver.support import expected_conditions as EC
from selenium.webdriver.support.ui import WebDriverWait

import config


def by_resource_id(resource_id: str) -> tuple:
    """Compose testTags are exposed as bare resource-ids (no package prefix), so By.ID cannot be used for them."""
    return AppiumBy.ANDROID_UIAUTOMATOR, f'new UiSelector().resourceId("{resource_id}")'


class BasePage:
    def __init__(self, driver):
        self.driver = driver
        self.wait = WebDriverWait(driver, config.EXPLICIT_WAIT_SEC)

    def find(self, locator):
        return self.wait.until(EC.visibility_of_element_located(locator), message=f"Element not visible: {locator}")

    def tap(self, locator):
        self.wait.until(EC.element_to_be_clickable(locator), message=f"Element not clickable: {locator}").click()

    def type(self, locator, text: str):
        field = self.find(locator)
        field.click()
        field.clear()
        field.send_keys(text)

    def text_of(self, locator) -> str:
        return self.find(locator).text

    def is_visible(self, locator) -> bool:
        return self.find(locator).is_displayed()

    def hide_keyboard(self):
        if self.driver.is_keyboard_shown():
            self.driver.hide_keyboard()
