# SauceDemo BDD Automation

A behavior-driven test automation framework built with **Playwright**, **Reqnroll**, and **C#**, testing the full login → cart → checkout flow on [saucedemo.com](https://www.saucedemo.com/). Built as a hands-on learning project to understand BDD, the Page Object Model, browser automation, and CI/CD from first principles.

## Tech Stack

- **C# (.NET 10)**
- **Playwright** — browser automation
- **Reqnroll** — BDD framework (Gherkin syntax, SpecFlow's maintained successor)
- **NUnit** — test runner
- **Microsoft.Extensions.Configuration** — settings management (JSON + environment variable overrides)
- **Reqnroll HTML Formatter** — living-documentation-style test reports
- **GitHub Actions** — CI pipeline, runs the full suite on every push

## Project Structure

```text
SauceDemoBDD/
├── .github/workflows/
│   └── ci.yml                 # GitHub Actions CI pipeline
├── Features/                  # Gherkin .feature files (plain-English scenarios)
│   ├── Login.feature
│   ├── AddToCart.feature
│   └── Checkout.feature
├── StepDefinitions/           # C# glue code connecting Gherkin to actions
│   ├── LoginSteps.cs
│   ├── CartSteps.cs
│   └── CheckoutSteps.cs
├── Pages/                     # Page Object Model — one class per screen
│   ├── BasePage.cs             # Shared IPage field, inherited by every page
│   ├── LoginPage.cs
│   ├── InventoryPage.cs
│   ├── CartPage.cs
│   └── CheckoutPage.cs
├── Support/                   # Test infrastructure
│   ├── Hooks.cs                # Browser lifecycle, failure screenshots + report attachments
│   ├── PlaywrightContext.cs    # Shared browser page across step classes
│   └── TestSettings.cs         # Strongly-typed config model
├── appsettings.json            # Browser, headless mode, slow-mo settings (local defaults)
├── reqnroll.json                # Reqnroll config incl. HTML report formatter
├── ROADMAP.md                   # Plan of action and progress tracking
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

Run only a subset by tag:

```powershell
dotnet test --filter "Category=smoke"
dotnet test --filter "Category=regression"
```

## Configuration

Browser behavior is controlled via `appsettings.json` locally — no code changes needed to switch:

```json
{
  "TestSettings": {
    "Browser": "msedge",
    "Headless": false,
    "SlowMoMilliseconds": 0
  }
}
```

- `Browser` — `"chrome"` or `"msedge"` locally (both run on the Chromium engine); CI uses plain `"chromium"`, since Edge/Chrome aren't installed on the CI machine
- `Headless` — `true` runs the browser invisibly (used in CI, where no display exists); `false` shows it on screen locally
- `SlowMoMilliseconds` — artificial delay added before each action, useful for watching tests run in real time

These settings can be overridden without touching the file, via environment variables (`TestSettings__Browser`, `TestSettings__Headless`, `TestSettings__SlowMoMilliseconds`) — this is exactly how CI runs headless Chromium while your local machine keeps its own settings.

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

## CI/CD Pipeline

Every push to `main` automatically triggers a GitHub Actions workflow (`.github/workflows/ci.yml`) that:

1. Checks out the code on a fresh Linux virtual machine
2. Installs the .NET SDK and restores/builds the project
3. Installs headless Chromium via Playwright
4. Runs the full test suite (`TestSettings__Browser=chromium`, `TestSettings__Headless=true` set via environment variables — no code or config file changes needed)
5. Uploads the HTML report and any failure screenshots as downloadable **artifacts**, even if tests fail (`if: always()`), so a failed run can be fully diagnosed after the fact

View live runs and download artifacts from the [Actions tab](https://github.com/ShrawanXIO/bdd-playwright-csharp/actions). Artifacts are retained for 90 days by default.

## What This Project Demonstrates

- **BDD with Gherkin** — scenarios written in plain English (`Given/When/Then`), translated into executable tests by Reqnroll, including `Background:` for shared setup across scenarios in a feature
- **Page Object Model with inheritance** — `BasePage` holds the shared `IPage` field and constructor; every other Page Object inherits from it instead of duplicating that setup
- **Dynamic locators** — `InventoryPage.AddToCartAsync` builds a product's locator on the fly from its name, so one method works for any product on the page rather than one method per item
- **Multi-page-object orchestration** — the Checkout scenario chains four Page Objects (`LoginPage` → `InventoryPage` → `CartPage` → `CheckoutPage`) in a single test, mirroring a real user journey across multiple screens
- **Reused step definitions across features** — the login step defined in `CartSteps.cs` is reused as-is in `Checkout.feature`'s `Background:`, since Reqnroll matches steps project-wide, not per file
- **Dependency injection via Reqnroll's context system** — `PlaywrightContext`, `ScenarioContext`, and `IReqnrollOutputHelper` are all shared across `Hooks` and step classes without static state
- **Layered, overridable configuration** — JSON file for local defaults, environment variables to override in CI, without maintaining two separate config files
- **Automatic failure diagnostics** — screenshots captured on test failure, attached directly to the test result via Reqnroll's Output API, and uploaded as CI artifacts
- **Tagged scenarios** — `@smoke` and `@regression` tags allow selective runs (`dotnet test --filter "Category=smoke"`)
- **Living-documentation HTML reports** — timestamped, per-run reports generated automatically via Reqnroll's built-in formatter
- **CI/CD** — the full suite runs headlessly on every push via GitHub Actions, on a platform-independent configuration, proving the framework isn't tied to any one machine

## Scenarios Covered

| Feature     | Scenario                | Tag           | What it verifies                                                    |
| ----------- | -------------------     | -----------   | ------------------------------------------------------------------  |
| Login       | Successful login        | `@smoke`      | Valid credentials land the user on the inventory page               |
| Login       | Failed login            | `@regression` | Invalid credentials show the correct SauceDemo error message        |
| Add to Cart | Add a single item       | `@regression` | Adding a product updates the cart badge to the correct count        |
| Checkout    | Complete checkout       | `@smoke`      | A full login → cart → checkout flow ends in an order confirmation   |

## Roadmap

See [ROADMAP.md](./ROADMAP.md) for what's built and what's planned next (parallel execution, retry logic, more scenarios).

---
*Built as a hands-on learning project — [Shrawan](https://github.com/ShrawanXIO)*
