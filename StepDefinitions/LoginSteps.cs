using Reqnroll;
using SauceDemoBDD.Pages;
using SauceDemoBDD.Support;

namespace SauceDemoBDD.StepDefinitions;
[Binding]
public class LoginSteps
{
    private readonly PlaywrightContext _context;
    private LoginPage _loginPage = null!;

    public LoginSteps(PlaywrightContext context)
    {
        _context = context;
    }

    [Given(@"I am on a Saucedemo login page")]
    public async Task GivenIAmOnASaucedemoLoginPage()
    {
        _loginPage = new LoginPage(_context.Page!);
        await _loginPage.GotoAsync();
    }

    [When(@"I log in with username ""(.*)"" And password ""(.*)""")]
    public async Task WhenILogInWithUserNameAndPassword(string username, string password)
    {
        await _loginPage.LoginAsync(username, password);
    }

    [Then(@"I should see the inventory page")]
    public async Task ThenIShouldSeeTheInventoryPage()
    {
        // Here you can add assertions to verify that the inventory page is displayed.
        // For example, you might check for the presence of a specific element on the inventory page.
        // This is a placeholder for demonstration purposes.
        var currentUrl = _context.Page!.Url;
        if (!currentUrl.Contains("inventory"))
        {
            throw new Exception("Inventory page not displayed.");
        }
    }

    [Then(@"I should see an error message ""(.*)""")]
    public async Task ThenIShouldSeeAnErrorMessage(string expectedErrorMessage)
    {
        var actualErrorMessage = await _loginPage.GetErrorMessageAsync();
        if (actualErrorMessage != expectedErrorMessage)
        {
            throw new Exception($"Expected error message: '{expectedErrorMessage}', but got: '{actualErrorMessage}'");
        }
    }
}