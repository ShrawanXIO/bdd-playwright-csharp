using Microsoft.Playwright;

namespace SauceDemoBDD.Pages;

public class BasePage
{
    protected readonly IPage _page;

    public BasePage(IPage page)
    {
        _page = page;
    }
}