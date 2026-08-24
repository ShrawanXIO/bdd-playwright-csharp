using Microsoft.Playwright;

namespace SauceDemoBDD.Pages;

public class InventoryPage : BasePage
{
    public InventoryPage(IPage page) : base(page)
    {
    }

    private ILocator CartBadge => _page.Locator("[data-test='shopping-cart-badge']");
  

    public async Task  AddToCartAsync(string productName)
    {
        var slug = productName.ToLower().Replace(" ", "-");
        var button = _page.Locator($"[data-test='add-to-cart-{slug}']");
        await button.ClickAsync(); 
    }

    public async Task<int> GetCartItemCountAsync()
    {
        var countText = await CartBadge.InnerTextAsync();
        return int.Parse(countText);
    }

}