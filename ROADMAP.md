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
| Structured reporting | ⬜ Not started |
| CI/CD (GitHub Actions) | ⬜ Not started |
| Parallel execution | ⬜ Not started |

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
  - Successful login with valid credentials
  - Failed login with invalid credentials (error message check)
- [x] **Add to Cart** — `AddToCart.feature`, `CartSteps.cs`, `InventoryPage.cs`
  - Add a single item, verify cart badge count
  - Uses `Background:` for shared login setup
- [x] **Checkout** — `Checkout.feature`, `CheckoutSteps.cs`, `CartPage.cs`, `CheckoutPage.cs`
  - Full flow: login → add to cart → cart → checkout form → finish → confirmation
  - First multi-page-object scenario (4 Page Objects orchestrated in one test)
- [x] **Update README** — reflect the Checkout feature and current 3-feature structure
- [x] **Tags** (`@smoke`, `@regression`) — mark scenarios so subsets can run selectively (`dotnet test --filter`), makes more sense now that there are 3+ features to organize

## Next up

- [ ] **Structured reporting** — Reqnroll LivingDoc or similar, so a test run produces a readable HTML report instead of raw console output

## Backlog / ideas

- [ ] **CI/CD** — run `dotnet test` automatically on push via GitHub Actions, headless
- [ ] **Parallel execution** — run scenarios concurrently using isolated `BrowserContext`s
- [ ] **Retry logic** — auto-retry a scenario once on failure before marking it failed, for flaky-test resilience
- [ ] **More scenarios** — e.g. remove item from cart, multiple items in one order, sort/filter products on inventory page
- [ ] **Data-driven scenarios** — `Scenario Outline` + `Examples` table for testing multiple login combinations at once

## Notes for next session

- Working directory: `E:\SauceDemoBDD`
- Repo: [github.com/ShrawanXIO/bdd-playwright-csharp](https://github.com/ShrawanXIO/bdd-playwright-csharp)
- Recurring gotcha to remember: Gherkin step text must match `[Given]/[When]/[Then]` attribute text **exactly**, including casing — this has caused most debugging sessions so far
- `CartSteps.cs` owns the shared `Given I am logged in as "..."` step used in `Background:` across multiple features — don't redefine it elsewhere, causes ambiguous-step errors
