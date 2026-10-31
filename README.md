# my moldcell — тестовое задание Middle QA

Ручное тестирование и UI-автотесты (C# / .NET 8, Appium, NUnit) неавторизованной части Android-приложения **my moldcell** (`md.moldcell.selfservice`, версия 1.44.1).

```
README.md                      запуск, окружение, ограничения
run_tests.sh                   запуск автотестов одной командой
.env.example                   параметры окружения (скопировать в .env при необходимости)
manual/
  checklist.md                 15 проверок, приоритеты и результаты
  bugs.md                      2 дефекта + продуктовый риск
  summary.md                   итоговый отчёт
  evidence/                    скриншоты и дампы к дефектам
automation/
  MoldcellTests.csproj         NUnit-проект (.NET 8, Appium.WebDriver 9, Selenium.Support)
  test_data.json               тестовые данные и ожидаемые тексты (не в коде тестов)
  Support/Settings.cs          параметры из переменных окружения, загрузка test_data.json
  Support/BaseTest.cs          сессия Appium на каждый тест, скриншот + page source при падении
  Pages/                       Page Object: все локаторы здесь
  Tests/                       StartScreenTests.cs (позитивный), LoginValidationTests.cs (негативный)
```

## Требования к окружению

- Android-эмулятор или устройство, Android 11+, с установленным my moldcell из Google Play (APK в репозиторий не входит).
- `adb` в PATH, Node.js ≥ 22.12, Appium 3 с драйвером UiAutomator2:
  ```bash
  npm install -g appium@3 && appium driver install uiautomator2
  ```
- .NET SDK 8.0+ (`dotnet --version`). NuGet-пакеты восстанавливаются автоматически при первом запуске.

Проверено на: AVD Pixel 6, Android 14 (API 34, Google Play arm64), Appium 3.9.0, uiautomator2 8.7.3, .NET SDK 8.0.427, macOS arm64.

## Запуск

```bash
./run_tests.sh
```

Скрипт проверяет, что устройство подключено, при необходимости поднимает Appium (лог — `automation/artifacts/appium.log`) и запускает `dotnet test`. Аргументы передаются в `dotnet test`, например `./run_tests.sh --filter LoginValidation`.

Без скрипта (если Appium уже запущен): `dotnet test automation/MoldcellTests.csproj`.

**Результаты:** консоль + отчёты `automation/artifacts/report.html` и `report.trx`. При падении теста в `automation/artifacts/` сохраняются скриншот и page source (XML), они же прикрепляются к результату теста; в консоли — сообщение assertion с ожидаемым и фактическим значением.

**Настройка:** по умолчанию используются `emulator-5554` и `http://127.0.0.1:4723`. Чтобы изменить, скопируйте `.env.example` в `.env`. Любое значение из `test_data.json` можно переопределить переменной окружения с тем же именем в верхнем регистре (например `INVALID_SHORT_PHONE=1234 ./run_tests.sh`).

## Устройство тестов

- **Независимость:** `BaseTest` открывает новую сессию на каждый тест (`[SetUp]`) с `noReset=false` — Appium очищает данные приложения, тест начинается с экрана выбора языка. Порядок запуска не важен.
- **Разрешения:** `autoGrantPermissions=true`, чтобы системный запрос на уведомления не перекрывал приложение.
- **Локаторы** вынесены в `automation/Pages/*Page.cs`. Экран языка — классические View (`By.Id` с пакетом), экран входа — Jetpack Compose, `testTag` отдаётся как «голый» resource-id, поэтому используется `MobileBy.AndroidUIAutomator("new UiSelector().resourceId(...)")` (`By.Id` автоматически добавил бы префикс пакета).
- **Ожидания:** только явные (`WebDriverWait`, 20 с по умолчанию, `EXPLICIT_WAIT_SEC`), implicit wait выключен, фиксированных пауз нет.
- **Данные:** в тестах нет реальных номеров и паролей; используются заведомо невалидный номер `6912` и фиктивный пароль. Негативный тест не отправляет запрос на сервер (срабатывает клиентская валидация).

## Ограничения

- Тексты-ожидания в `test_data.json` рассчитаны на английский язык приложения (тест сам выбирает English).
- Проверялась только часть без авторизации; вход, OTP, регистрация до конца и любые платные операции не выполнялись.
- На тестовом эмуляторе был отключён системный сервис автозаполнения (`settings put secure autofill_service null`), т.к. диалог Google Password Manager перекрывал экран при ручной проверке. Автотесты от него не зависят (они не сохраняют пароль), но на устройстве с автозаполнением диалог теоретически может появиться.
