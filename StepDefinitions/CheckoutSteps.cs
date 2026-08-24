using Reqnroll;
using SauceDemoBDD.Pages;
using SauceDemoBDD.Support;

namespace SauceDemoBDD.StepDefinitions;

[Binding]
public class CheckoutSteps
{
    private readonly PlaywrightContext _context;
    private InventoryPage _inventoryPage = null!;
    private CartPage _cartPage = null!;
    private CheckoutPage _checkoutPage = null!;

    public CheckoutSteps(PlaywrightContext context)
    {
        _context = context;
    }

    [When(@"I proceed to checkout")]
    public async Task WhenIProceedToCheckout()
    {
        _inventoryPage = new InventoryPage(_context.Page!);
        _cartPage = new CartPage(_context.Page!);
        await _inventoryPage.GoToCartAsync();
        await _cartPage.ProceedToCheckoutAsync();
    }

    [When(@"I fill in checkout information with first name ""(.*)"", last name ""(.*)"", and zip code ""(.*)""")]
    public async Task WhenIFillInCheckoutInformation(string firstName, string lastName, string zipCode)
    {
        _checkoutPage = new CheckoutPage(_context.Page!);
        await _checkoutPage.FillCheckoutFormAsync(firstName, lastName, zipCode);
        await _checkoutPage.ClickContinueButtonAsync();
    }

    [When(@"I complete the checkout")]
    public async Task WhenICompleteTheCheckout()
    {
        await _checkoutPage.ClickFinishButtonAsync();
    }

    [Then(@"I should see the order confirmation ""(.*)""")]
    public async Task ThenIShouldSeeTheOrderConfirmation(string expectedMessage)
    {
        var actualMessage = await _checkoutPage.GetConfirmationMessageAsync();
        if (actualMessage != expectedMessage)
        {
            throw new Exception($"Expected confirmation '{expectedMessage}' but got '{actualMessage}'");
        }
    }
}