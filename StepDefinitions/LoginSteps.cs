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
}