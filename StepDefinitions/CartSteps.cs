using Reqnroll;
using SauceDemoBDD.Pages;
using SauceDemoBDD.Support;

namespace SauceDemoBDD.StepDefinitions;
[Binding]

public class CartSteps
{
    private readonly PlaywrightContext _context;
    private InventoryPage _inventoryPage = null!;
    private LoginPage _loginPage = null!;

    public CartSteps(PlaywrightContext context)
    {
        _context = context;
    }

    [Given(@"I am logged in as ""(.*)""")]
    public async Task GivenIAmLoggedInAs(string username)
    {
        _loginPage = new LoginPage(_context.Page!);
        await _loginPage.GotoAsync();
        await _loginPage.LoginAsync(username, "secret_sauce");
    }

    [When(@"I add ""(.*)"" to the cart")]
    public async Task WhenIAddToTheCart(string productName)
    {
        _inventoryPage = new InventoryPage(_context.Page!);
        await _inventoryPage.AddToCartAsync(productName);
    }
    
    [Then(@"the cart should contain (.*) item")]
    public async Task ThenTheCartShouldContainItem(int expectedItemCount)
    {
        var actualItemCount = await _inventoryPage.GetCartItemCountAsync();
        if (actualItemCount != expectedItemCount)
        {
            throw new Exception($"Expected cart item count: {expectedItemCount}, but got: {actualItemCount}");
        }
    }

    
}




