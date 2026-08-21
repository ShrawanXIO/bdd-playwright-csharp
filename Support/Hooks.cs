using Microsoft.Playwright;
using Reqnroll;

namespace SauceDemoBDD.Support;

[Binding]
public class Hooks
{
    private readonly PlaywrightContext _context;

    public Hooks(PlaywrightContext context)
    {
        _context = context;
    }

    [BeforeScenario]
    public async Task BeforeScenario()
    {
        var playWright = await Playwright.CreateAsync();
        var browser = await playWright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false
        });
        _context.Page = await browser.NewPageAsync();
    }

    [AfterScenario]
    public async Task AfterScenario()
    {
        if (_context.Page is not null)
        {
            await _context.Page.Context.Browser!.CloseAsync();
        }
    }
}

