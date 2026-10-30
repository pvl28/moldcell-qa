#!/usr/bin/env bash
# Runs the UI tests with one command. Expects: a booted emulator/device with the app installed,
# adb + appium (with uiautomator2 driver) on PATH. Starts Appium automatically if it is not running.
set -euo pipefail
cd "$(dirname "$0")"

[ -f "$HOME/qa-tools/env.sh" ] && source "$HOME/qa-tools/env.sh"   # local toolchain from this setup (optional)
[ -f .env ] && set -a && source .env && set +a
APPIUM_URL="${APPIUM_URL:-http://127.0.0.1:4723}"

adb get-state >/dev/null 2>&1 || { echo "ERROR: no Android device/emulator connected (adb get-state failed)"; exit 2; }

if ! curl -fs "$APPIUM_URL/status" >/dev/null; then
  echo "Starting Appium server -> automation/artifacts/appium.log"
  mkdir -p automation/artifacts
  appium --log automation/artifacts/appium.log --log-no-colors >/dev/null 2>&1 &
  APPIUM_PID=$!
  trap 'kill $APPIUM_PID 2>/dev/null || true' EXIT
  for _ in $(seq 1 30); do curl -fs "$APPIUM_URL/status" >/dev/null && break; sleep 1; done
fi

if [ ! -x .venv/bin/python ]; then
  python3 -m venv .venv
  .venv/bin/pip install -q -r automation/requirements.txt
fi

cd automation && ../.venv/bin/python -m pytest "$@"
