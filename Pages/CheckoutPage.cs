using Microsoft.Playwright;

namespace SauceDemoBDD.Pages;

public class CheckoutPage : BasePage
{
    public CheckoutPage(IPage page) : base(page)
    {
    }

    private ILocator FirstNameField => _page.Locator("[data-test='firstName']");
    private ILocator LastNameField => _page.Locator("[data-test='lastName']");
    private ILocator ZipCodeField => _page.Locator("[data-test='postalCode']");
    private ILocator ContinueButton => _page.Locator("[data-test='continue']");
    private ILocator FinishButton => _page.Locator("[data-test='finish']");
    private ILocator ConfirmationMessage => _page.Locator("[data-test='complete-header']");

    public async Task FillCheckoutFormAsync(string firstName, string lastName, string zipCode)
    {
        await FirstNameField.FillAsync(firstName);
        await LastNameField.FillAsync(lastName);
        await ZipCodeField.FillAsync(zipCode);
    }

    public async Task ClickContinueButtonAsync()
    {
        await ContinueButton.ClickAsync();
    }

    public async Task ClickFinishButtonAsync()
    {
        await FinishButton.ClickAsync();
    }

    public async Task<string> GetConfirmationMessageAsync()
    {
        return await ConfirmationMessage.InnerTextAsync();
    }
}