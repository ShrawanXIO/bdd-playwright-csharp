using Microsoft.Extensions.Configuration;
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
        var configuration =  new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        var testSettings = configuration.GetSection("TestSettings").Get<TestSettings>() ?? new TestSettings();

        var playWright = await Playwright.CreateAsync();
        var browser = await playWright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = testSettings.Headless,
            Channel = testSettings.Browser,
            SlowMo = testSettings.SlowMoMilliseconds
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

