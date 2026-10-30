import logging
from datetime import datetime

import pytest
from appium import webdriver
from appium.options.android import UiAutomator2Options

import config

log = logging.getLogger(__name__)


def _capabilities() -> UiAutomator2Options:
    opts = UiAutomator2Options()
    opts.platform_name = "Android"
    opts.automation_name = "UiAutomator2"
    opts.device_name = config.DEVICE_NAME
    opts.app_package = config.APP_PACKAGE
    opts.app_activity = config.APP_ACTIVITY
    # noReset=false -> Appium runs `pm clear` before each session, so every test
    # starts from a fresh install state (language screen) and tests stay independent.
    opts.no_reset = False
    opts.full_reset = False
    # Grants POST_NOTIFICATIONS up front so the system permission dialog does not overlay the app.
    opts.auto_grant_permissions = True
    opts.new_command_timeout = 120
    return opts


@pytest.fixture
def driver():
    drv = webdriver.Remote(config.APPIUM_URL, options=_capabilities())
    drv.implicitly_wait(0)  # only explicit waits are used (see pages/base_page.py)
    yield drv
    drv.quit()


@pytest.fixture
def test_data() -> dict:
    return config.load_test_data()


@pytest.hookimpl(hookwrapper=True)
def pytest_runtest_makereport(item, call):
    """On test failure save a screenshot and the page source into automation/artifacts/."""
    outcome = yield
    report = outcome.get_result()
    if report.when != "call" or not report.failed:
        return
    drv = item.funcargs.get("driver")
    if drv is None:
        return
    config.ARTIFACTS_DIR.mkdir(exist_ok=True)
    stem = f"{item.name}_{datetime.now():%Y%m%d_%H%M%S}"
    try:
        drv.save_screenshot(str(config.ARTIFACTS_DIR / f"{stem}.png"))
        (config.ARTIFACTS_DIR / f"{stem}.xml").write_text(drv.page_source, encoding="utf-8")
        log.error("Failure artifacts saved: artifacts/%s.png|.xml", stem)
    except Exception as exc:  # session may already be dead
        log.error("Could not collect failure artifacts: %s", exc)
