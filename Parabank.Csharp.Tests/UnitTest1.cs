using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Parabank.Csharp.Tests;

public class Tests
{
    private const string BaseUrl = "https://parabank.parasoft.com/parabank/index.htm";
    private const string ValidUsername = "il.test.user";
    private const string ValidPassword = "il.test.password1234";
    private const string OverviewUrlFragment = "overview.htm";
    private const string InvalidUsername = "invalidUsername";
    private const string InvalidPassword = "invalidPassword";
    private const string InvalidUsernameOrPasswordErrorMessage = "The username and password could not be verified.";

    // Element selectors
    private const string UsernameSelector = "username";
    private const string PasswordSelector = "password";
    private const string SubmitButtonSelector = "input[type='submit']";

    private IWebDriver driver;

    [SetUp]
    public void Setup()
    {
        driver = new ChromeDriver();
        driver.Navigate().GoToUrl(BaseUrl);
    }

    [TearDown]
    public void Teardown()
    {
        driver.Quit();
    }

    [Test]
    public void HappyPathLoginTest()
    {
        driver.FindElement(By.Name(UsernameSelector)).SendKeys(ValidUsername);
        driver.FindElement(By.Name(PasswordSelector)).SendKeys(ValidPassword);
        driver.FindElement(By.CssSelector(SubmitButtonSelector)).Click();

        driver.Url.Should().Contain(OverviewUrlFragment);
    }

    [Test]
    public void InvalidUsernameTest()
    {
        driver.FindElement(By.Name(UsernameSelector)).SendKeys(InvalidUsername);
        driver.FindElement(By.Name(PasswordSelector)).SendKeys(ValidPassword);
        driver.FindElement(By.CssSelector(SubmitButtonSelector)).Click();
        Task.Delay(100).Wait();

        driver.PageSource.Should().Contain(InvalidUsernameOrPasswordErrorMessage);
    }

    [Test]
    public void InvalidPasswordTest()
    {
        driver.FindElement(By.Name(UsernameSelector)).SendKeys(ValidUsername);
        driver.FindElement(By.Name(PasswordSelector)).SendKeys(InvalidPassword);
        driver.FindElement(By.CssSelector(SubmitButtonSelector)).Click();
        Task.Delay(100).Wait();

        driver.PageSource.Should().Contain(InvalidUsernameOrPasswordErrorMessage);
    }
}
