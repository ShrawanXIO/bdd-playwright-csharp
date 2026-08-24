using Microsoft.Playwright;

namespace SauceDemoBDD.Pages;

public class CartPage : BasePage
{
    public CartPage(IPage page) : base(page)
    {
    }

    private ILocator CheckoutButton => _page.Locator("[data-test='checkout']");

    public async Task ProceedToCheckoutAsync()
    {
        await CheckoutButton.ClickAsync();
    }
}