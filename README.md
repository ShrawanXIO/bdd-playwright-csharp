# SauceDemo BDD Automation

A behavior-driven test automation framework built with **Playwright**, **Reqnroll**, and **C#**, testing the login flow on [saucedemo.com](https://www.saucedemo.com/). Built as a hands-on learning project to understand BDD, the Page Object Model, and browser automation from first principles.

## Tech Stack

- **C# (.NET 10)**
- **Playwright** — browser automation
- **Reqnroll** — BDD framework (Gherkin syntax, SpecFlow's maintained successor)
- **NUnit** — test runner
- **Microsoft.Extensions.Configuration** — settings management

## Project Structure

```
SauceDemoBDD/
├── Features/                 # Gherkin .feature files (plain-English scenarios)
│   └── Login.feature
├── StepDefinitions/          # C# glue code connecting Gherkin to actions
│   └── LoginSteps.cs
├── Pages/                    # Page Object Model — one class per screen
│   └── LoginPage.cs
├── Support/                  # Test infrastructure
│   ├── Hooks.cs               # Browser lifecycle (open/close per scenario)
│   ├── PlaywrightContext.cs   # Shared browser page across step classes
│   └── TestSettings.cs        # Strongly-typed config model
├── appsettings.json           # Browser, headless mode, slow-mo settings
└── SauceDemoBDD.csproj
```

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- Google Chrome or Microsoft Edge installed

## Setup

```powershell
git clone https://github.com/ShrawanXIO/bdd-playwright-csharp.git
cd bdd-playwright-csharp/SauceDemoBDD
dotnet build
.\bin\Debug\net10.0\playwright.ps1 install chromium
```

## Running the Tests

```powershell
dotnet test
```

By default, this launches a real, visible browser and runs both scenarios end to end.

## Configuration

Browser behavior is controlled via `appsettings.json` — no code changes needed:

```json
{
  "TestSettings": {
    "Browser": "msedge",
    "Headless": false,
    "SlowMoMilliseconds": 0
  }
}
```

- `Browser` — `"chrome"` or `"msedge"` (both run on the Chromium engine)
- `Headless` — `true` runs the browser invisibly (useful for CI); `false` shows it on screen
- `SlowMoMilliseconds` — artificial delay added before each action, useful for watching tests run in real time

## What This Project Demonstrates

- **BDD with Gherkin** — scenarios written in plain English (`Given/When/Then`), translated into executable tests by Reqnroll
- **Page Object Model** — UI locators and actions isolated in `Pages/`, keeping step definitions readable and maintainable
- **Dependency injection via Reqnroll's context system** — `PlaywrightContext` is shared across `Hooks` and step classes without static state
- **Configurable test execution** — browser choice, headless mode, and execution speed driven by external config, not hardcoded
- **Automatic failure diagnostics** — screenshots captured on test failure, saved to `ScreenshotsOnFailure/`

## Scenarios Covered

| Scenario | What it verifies |
|---|---|
| Successful login | Valid credentials land the user on the inventory page |
| Failed login | Invalid credentials show the correct SauceDemo error message |

---
*Built as a hands-on learning project — [Shrawan](https://github.com/ShrawanXIO)*
