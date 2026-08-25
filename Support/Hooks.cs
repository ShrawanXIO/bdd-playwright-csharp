using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using Reqnroll;

namespace SauceDemoBDD.Support;

[Binding]
public class Hooks
{
    private readonly PlaywrightContext _context;
    private readonly ScenarioContext _scenarioContext;
    private readonly IReqnrollOutputHelper _OutputHelper;

    public Hooks(PlaywrightContext context, ScenarioContext scenarioContext, IReqnrollOutputHelper OutputHelper)
    {
        _context = context;
        _scenarioContext = scenarioContext;
        _OutputHelper = OutputHelper;
    }

    [BeforeScenario]
    public async Task BeforeScenario()
    {
        //var configuration =  new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        var configuration = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json")
                            .AddEnvironmentVariables()
                            .Build();

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
        if (_context.Page is not null && _scenarioContext.TestError is not null)
        {
            var fileName = $"{_scenarioContext.ScenarioInfo.Title}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
            var path = Path.Combine("ScreenshotsOnFailure", fileName);
            Directory.CreateDirectory("ScreenshotsOnFailure");
            await _context.Page.ScreenshotAsync(new PageScreenshotOptions { Path = path });
            _OutputHelper.AddAttachment(Path.GetFullPath(path));
        }

        if (_context.Page is not null)
        {
            await _context.Page.Context.Browser!.CloseAsync();
        }
    }
}

