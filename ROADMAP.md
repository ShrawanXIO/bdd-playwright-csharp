# Roadmap

Plan of action for this project — what's built, what's next, and what's still just an idea. Updated as work progresses.

## Status at a glance

| Area | Status |
| ---------------------- | -------------- |
| Core framework (POM, DI, hooks) | ✅ Done |
| Login feature | ✅ Done |
| Add to Cart feature | ✅ Done |
| Checkout feature | ✅ Done |
| Configurable settings | ✅ Done |
| Screenshot on failure | ✅ Done |
| README | ✅ Done |
| Tags for selective runs | ✅ Done |
| Structured reporting | ✅ Done |
| CI/CD (GitHub Actions) | ✅ Done |
| Parallel execution | ⬜ Not started |

All core roadmap items are complete. Remaining items are stretch goals in the backlog below.

## Completed

### Framework foundation

- [x] Reqnroll + Playwright + NUnit project scaffolded
- [x] `BasePage` — shared `IPage` field and constructor, inherited by every Page Object
- [x] `PlaywrightContext` — shared browser page injected across Hooks and step classes (no static state)
- [x] `Hooks.cs` — opens a fresh browser per scenario, closes it after
- [x] `appsettings.json` + `TestSettings.cs` — configurable browser (Chrome/Edge), headless mode, and SlowMo, no code changes needed to switch
- [x] Screenshot-on-failure — `AfterScenario` checks `ScenarioContext.TestError` and saves a `.png` to `ScreenshotsOnFailure/`

### Features

- [x] **Login** — `Login.feature`, `LoginSteps.cs`, `LoginPage.cs`
  - Successful login with valid credentials (`@smoke`)
  - Failed login with invalid credentials (`@regression`, error message check)
- [x] **Add to Cart** — `AddToCart.feature`, `CartSteps.cs`, `InventoryPage.cs`
  - Add a single item, verify cart badge count (`@regression`)
  - Uses `Background:` for shared login setup
- [x] **Checkout** — `Checkout.feature`, `CheckoutSteps.cs`, `CartPage.cs`, `CheckoutPage.cs`
  - Full flow: login → add to cart → cart → checkout form → finish → confirmation (`@smoke`)
  - First multi-page-object scenario (4 Page Objects orchestrated in one test)

### Infrastructure

- [x] **README** — reflects the full 3-feature structure, tags, reporting, and CI/CD
- [x] **Tags** (`@smoke`, `@regression`) — mark scenarios so subsets can run selectively (`dotnet test --filter "Category=smoke"`)
- [x] **Structured reporting** — Reqnroll's built-in HTML formatter, configured in `reqnroll.json`, generates a timestamped living-documentation report (`reports/reqnroll_report_{timestamp}.html`) on every run
- [x] **Failure screenshots attached to reports** — via `IReqnrollOutputHelper.AddAttachment`, so a failed test's screenshot is one click away in Test Explorer, not just sitting in a folder
- [x] **CI/CD** — `.github/workflows/ci.yml` runs the full suite on every push to `main`, on a fresh Linux VM, using environment variables (`TestSettings__Browser`, `TestSettings__Headless`) to override local config for headless Chromium — no code changes needed between local and CI runs
- [x] **CI artifacts** — the HTML report and any failure screenshots are uploaded as downloadable artifacts (`if: always()`, so they're captured even when the run fails), retained for 90 days on GitHub

## Backlog / ideas

- [ ] **Parallel execution** — run scenarios concurrently using isolated `BrowserContext`s
- [ ] **Retry logic** — auto-retry a scenario once on failure before marking it failed, for flaky-test resilience
- [ ] **More scenarios** — e.g. remove item from cart, multiple items in one order, sort/filter products on inventory page
- [ ] **Data-driven scenarios** — `Scenario Outline` + `Examples` table for testing multiple login combinations at once
- [ ] **Shorten artifact retention** — currently defaults to 90 days; could reduce via `retention-days` in the workflow if storage becomes a concern

## Notes for next session

- Working directory: `E:\SauceDemoBDD`
- Repo: [github.com/ShrawanXIO/bdd-playwright-csharp](https://github.com/ShrawanXIO/bdd-playwright-csharp)
- Recurring gotcha to remember: Gherkin step text must match `[Given]/[When]/[Then]` attribute text **exactly**, including casing — this has caused most debugging sessions so far. The same casing rule bit us again in YAML (`on`/`push`/`jobs`/`steps` must be lowercase) — it's a pattern worth watching for in any config format, not just Gherkin.
- `CartSteps.cs` owns the shared `Given I am logged in as "..."` step used in `Background:` across multiple features — don't redefine it elsewhere, causes ambiguous-step errors
- HTML reports land in `bin/Debug/net10.0/reports/`, one per run — `.gitignore`'d, same as `ScreenshotsOnFailure/`
- CI overrides local config via environment variables (`TestSettings__Browser=chromium`, `TestSettings__Headless=true`) rather than a separate config file — `Hooks.cs` reads both `appsettings.json` and environment variables, with env vars taking precedence
