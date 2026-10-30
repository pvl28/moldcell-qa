"""Environment configuration. Values come from .env (see .env.example) or the process environment."""
import json
import os
from pathlib import Path

from dotenv import load_dotenv

ROOT = Path(__file__).resolve().parent
load_dotenv(ROOT.parent / ".env")

APPIUM_URL = os.getenv("APPIUM_URL", "http://127.0.0.1:4723")
DEVICE_NAME = os.getenv("DEVICE_NAME", "emulator-5554")
APP_PACKAGE = os.getenv("APP_PACKAGE", "md.moldcell.selfservice")
APP_ACTIVITY = os.getenv("APP_ACTIVITY", ".screens.splash.SplashActivity")
EXPLICIT_WAIT_SEC = int(os.getenv("EXPLICIT_WAIT_SEC", "20"))
ARTIFACTS_DIR = ROOT / "artifacts"


def load_test_data() -> dict:
    """Test data lives in test_data.json; any key can be overridden by an env var of the same name in upper case."""
    data = json.loads((ROOT / "test_data.json").read_text(encoding="utf-8"))
    for key in data:
        if isinstance(data[key], str) and os.getenv(key.upper()):
            data[key] = os.environ[key.upper()]
    return data
