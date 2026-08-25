# SauceDemo BDD Automation

A behavior-driven test automation framework built with **Playwright**, **Reqnroll**, and **C#**, testing the full login → cart → checkout flow on [saucedemo.com](https://www.saucedemo.com/). Built as a hands-on learning project to understand BDD, the Page Object Model, and browser automation from first principles.

## Tech Stack

- **C# (.NET 10)**
- **Playwright** — browser automation
- **Reqnroll** — BDD framework (Gherkin syntax, SpecFlow's maintained successor)
- **NUnit** — test runner
- **Microsoft.Extensions.Configuration** — settings management
- **Reqnroll HTML Formatter** — living-documentation-style test reports

## Project Structure

```text
SauceDemoBDD/
├── Features/                 # Gherkin .feature files (plain-English scenarios)
│   ├── Login.feature
│   ├── AddToCart.feature
│   └── Checkout.feature
├── StepDefinitions/          # C# glue code connecting Gherkin to actions
│   ├── LoginSteps.cs
│   └── CartSteps.cs
│   └── CheckoutSteps.cs
├── Pages/                    # Page Object Model — one class per screen
│   ├── BasePage.cs            # Shared IPage field, inherited by every page
│   ├── LoginPage.cs
│   ├── InventoryPage.cs
│   ├── CartPage.cs
│   └── CheckoutPage.cs
├── Support/                  # Test infrastructure
│   ├── Hooks.cs               # Browser lifecycle, failure screenshots + report attachments
│   ├── PlaywrightContext.cs   # Shared browser page across step classes
│   └── TestSettings.cs        # Strongly-typed config model
├── appsettings.json           # Browser, headless mode, slow-mo settings
├── reqnroll.json              # Reqnroll config incl. HTML report formatter
├── ROADMAP.md                 # Plan of action and progress tracking
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

By default, this launches a real, visible browser and runs every scenario end to end.

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

## Test Reports

Every `dotnet test` run generates a living-documentation-style HTML report via Reqnroll's built-in formatter, configured in `reqnroll.json`:

```json
{
  "formatters": {
    "html": {
      "outputFilePath": "reports/reqnroll_report_{timestamp}.html"
    }
  }
}
```

- Reports are saved to `bin/Debug/net10.0/reports/`, one per run (timestamped, so previous runs aren't overwritten)
- Each report shows every `Feature`, `Scenario`, and step with pass/fail status
- Any scenario that fails automatically gets a screenshot attached via Reqnroll's Output API, viewable directly from the test result in Visual Studio Code's Test Explorer

## What This Project Demonstrates

- **BDD with Gherkin** — scenarios written in plain English (`Given/When/Then`), translated into executable tests by Reqnroll, including `Background:` for shared setup across scenarios in a feature
- **Page Object Model with inheritance** — `BasePage` holds the shared `IPage` field and constructor; every other Page Object inherits from it instead of duplicating that setup
- **Dynamic locators** — `InventoryPage.AddToCartAsync` builds a product's locator on the fly from its name, so one method works for any product on the page rather than one method per item
- **Multi-page-object orchestration** — the Checkout scenario chains four Page Objects (`LoginPage` → `InventoryPage` → `CartPage` → `CheckoutPage`) in a single test, mirroring a real user journey across multiple screens
- **Reused step definitions across features** — the login step defined in `CartSteps.cs` is reused as-is in `Checkout.feature`'s `Background:`, since Reqnroll matches steps project-wide, not per file
- **Dependency injection via Reqnroll's context system** — `PlaywrightContext`, `ScenarioContext`, and `IReqnrollOutputHelper` are all shared across `Hooks` and step classes without static state
- **Configurable test execution** — browser choice, headless mode, and execution speed driven by external config, not hardcoded
- **Automatic failure diagnostics** — screenshots captured on test failure, saved to `ScreenshotsOnFailure/`, and attached directly to the test result via Reqnroll's Output API
- **Tagged scenarios** — `@smoke` and `@regression` tags allow selective runs (`dotnet test --filter "Category=smoke"`)
- **Living-documentation HTML reports** — timestamped, per-run reports generated automatically via Reqnroll's built-in formatter

## Scenarios Covered

| Feature     | Scenario            | Tag           | What it verifies                                                     |
| ----------- | ------------------- | -----------   | ------------------------------------------------------------------   |
| Login       | Successful login    | `@smoke`      | Valid credentials land the user on the inventory page                |
| Login       | Failed login        | `@regression` | Invalid credentials show the correct SauceDemo error message         |
| Add to Cart | Add a single item   | `@regression` | Adding a product updates the cart badge to the correct count         |
| Checkout    | Complete checkout   | `@smoke`      | A full login → cart → checkout flow ends in an order confirmation    |

## Roadmap

See [ROADMAP.md](./ROADMAP.md) for what's built, what's next, and planned improvements (CI/CD, parallel execution).

---
*Built as a hands-on learning project — [Shrawan](https://github.com/ShrawanXIO)*
