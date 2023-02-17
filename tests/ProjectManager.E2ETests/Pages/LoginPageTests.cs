using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using Xunit;
using ProjectManager.E2ETests.Pages.PageObjects;
using FluentAssertions;
using OpenQA.Selenium.Chrome;

namespace ProjectManager.E2ETests.Pages;

[Collection("Test collection")]
public class LoginPageTests : IAsyncLifetime
{
  private readonly SharedTestContext _testContext;
  private readonly LoginPage _loginPage;
  private string Url => "http://localhost:7780/";

  public LoginPageTests(SharedTestContext sharedTestContext)
  {
    _testContext = sharedTestContext;
    _loginPage = new LoginPage(_testContext.Driver, _testContext.Wait);
  }

  [Fact]
  public async Task GoToHomePage_ShouldRedirectToLogin_WhenNotAuthorized()
  {
    // Act
    _testContext.Driver.GoToHomePage();
    await Task.Delay(1000);

    // Assert
    _testContext.Driver.Url.Should().Be(Url+"login");
  }

  [Fact]
  public async Task Login_ShouldRedirectToHome()
  {
    // Act
    _loginPage.GoToLoginPage();
    _loginPage.FillFormAndLogin();

    // Assert
    _testContext.Driver.Url.Should().Be(Url + "pm");
  }

  [Fact]
  public async Task LoginWithoutSession_ShouldBeNotAuthorized_WhenNewWindowOpened()
  {
    // Act
    _loginPage.GoToLoginPage();
    _loginPage.FillFormAndLogin();
    _testContext.Driver.SwitchTo().NewWindow(WindowType.Window);
    _testContext.Driver.GoToHomePage();
    await Task.Delay(1000);

    // Assert
    _testContext.Driver.Url.Should().Be(Url + "login");
  }

  [Fact]
  public async Task LoginWithSession_ShouldBeAuthorized_WhenNewWindowOpened()
  {
    // Act
    _loginPage.GoToLoginPage();
    _loginPage.FillFormCheckRememberMeAndLogin();
    _testContext.Driver.SwitchTo().NewWindow(WindowType.Window);
    _testContext.Driver.GoToHomePage();
    await Task.Delay(1000);

    // Assert
    _testContext.Driver.Url.Should().Be(Url + "pm");
  }

  [Fact]
  public async Task Login_ShouldShowValidationMessage_WhenDataIsInvalid()
  {
    // Act
    _loginPage.GoToLoginPage();
    _loginPage.FillFormWithInvalidDataAndLogin();

    // Assert
    var expected = _testContext.Driver.FindElements(By.XPath("//*[contains(text(), 'Invalid email or password')]"));
    expected.Should().NotBeEmpty();
  }

  [Fact]
  public async Task GetDriverUrl_ShouldBeEqualToRegisterPage_AfterSignUpLinkClick()
  {
    // Act
    _loginPage.GoToLoginPage();
    _loginPage.GoToRegisterPage();

    // Assert
    _testContext.Driver.Url.Should().Be(Url + "register");
  }

  [Fact]
  public async Task GetDriverUrl_ShouldBeEqualToLoginPage_AfterBackToLoginClick()
  {
    // Act
    _loginPage.GoToLoginPage();
    _loginPage.GoToRegisterPage();
    _loginPage.AlreadyHaveAnAccount();

    // Assert
    _testContext.Driver.Url.Should().Be(Url + "login");
  }

  [Fact]
  public async Task Register_ShouldShowSuccessView_WhenProvidedValidData()
  {
    // Act
    _loginPage.GoToLoginPage();
    _loginPage.GoToRegisterPage();
    _loginPage.FillFormAndRegister();

    // Assert
    var expected = _testContext.Driver.FindElement(By.TagName("h3")).Text;
    expected.Should().Be("SUCCESS");
  }

  [Fact]
  public async Task Register_ShouldShowFailView_WhenProvidedInvalidData()
  {
    // Act
    _loginPage.GoToLoginPage();
    _loginPage.GoToRegisterPage();
    _loginPage.FillFormWithInvalidDataAndRegister();

    // Assert
    var expected = _testContext.Driver.FindElement(By.TagName("h3")).Text;
    expected.Should().Be("SOMETHING WENT WRONG");

    // Act
    _loginPage.ClickTryAgain();

    // Assert
    var nextExpected = _testContext.Driver.FindElement(By.TagName("h3")).Text;
    nextExpected.Should().Be("SIGN UP");
  }

  [Fact]
  public async Task RegisterAndLogin_ShouldRedirectToHomePage_WhenSucceeded()
  {
    // Act
    _loginPage.GoToLoginPage();
    _loginPage.GoToRegisterPage();
    _loginPage.FillFormAndRegister();
    _loginPage.BackToLoginPage();
    _loginPage.FillFormAndLoginWithCreatedAccount();

    // Assert
    _testContext.Driver.Url.Should().Be(Url + "pm");
  }

  public Task InitializeAsync() => Task.CompletedTask;
  public Task DisposeAsync() => _testContext.ResetDriver();
}
