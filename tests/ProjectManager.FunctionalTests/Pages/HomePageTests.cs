using FluentAssertions;
using OpenQA.Selenium;
using ProjectManager.E2ETests.Pages.PageObjects;
using SeleniumExtras.WaitHelpers;
using Xunit;

namespace ProjectManager.E2ETests.Pages;

[Collection("Test collection")]
public class HomePageTests : IAsyncLifetime
{
  private readonly SharedTestContext _testContext;
  private readonly LoginPage _loginPage;
  private readonly HomePage _homePage;
  private string Url => "http://localhost:7780/";

  public HomePageTests(SharedTestContext testContext)
  {
    _testContext = testContext;
    _loginPage = new LoginPage(testContext.Driver, testContext.Wait);
    _homePage = new HomePage(testContext.Driver, testContext.Wait);
  }

  [Fact]
  public async Task Logout_ShouldRedirectToLoginPage_WhenSucceeded()
  {
    // Arrange
    _loginPage.BootstrapSession();

    // Act
    _homePage.LogoutBtn.Click();
    _testContext.Wait.Until(e => e.Url == Url + "login");

    // Assert
    _testContext.Driver.Url.Should().Be(Url + "login");
  }

  [Fact]
  public async Task EditProfile_ShouldBeAbleToLogin_WhenEditedDataProvided()
  {
    // Arrange
    _loginPage.BootstrapSession();

    // Act
    _homePage.EditProfileAndSave();
    _homePage.LogoutBtn.Click();
    _testContext.Wait.Until(e => e.Url == Url + "login");
    _loginPage.FillFormAndLogin();
    _testContext.Wait.Until(e => e.Url == Url + "pm");

    // Assert
    _testContext.Driver.Url.Should().Be(Url + "pm");
    _testContext.ShouldContainText("Kadie");
  }

  [Fact]
  public async Task CreateProject_ShouldAppearOnProjectList_WhenSucceeded()
  {
    // Arrange
    _loginPage.BootstrapSession();

    // Act
    _homePage.CreateProjectAndMoveCarousel();

    // Assert
    _testContext.ShouldContainText("Test project");
  }

  [Fact]
  public async Task CheckNotificationsCount_ShouldEqualZero_AfterCollapse()
  {
    // Arrange
    _loginPage.BootstrapSession();

    // Act
    _testContext.ShouldContainElements("circle-span", 2); // messages and notifications count spans
    _homePage.CollapseAndHideNotifications();

    // Assert
    _testContext.ShouldContainElements("circle-span", 1); // messages count span
  }

  [Fact]
  public async Task CheckMessagesCount_ShouldBeLessByOne_AfterConversationOpening()
  {
    // Arrange
    _loginPage.BootstrapSession();

    // Act
    var currentCount = _testContext.Wait.Until(e => e.FindElements(By.ClassName("circle-span")));
    currentCount.Count.Should().Be(2); // messages and notifications count
    int.Parse(currentCount.First().Text).Should().Be(3);
    _homePage.OpenFirstConversation();
    _testContext.Wait.Until(ExpectedConditions.ElementExists(By.ClassName("circle-span")));

    // Assert
    currentCount = _testContext.Wait.Until(e => e.FindElements(By.ClassName("circle-span")));
    int.Parse(currentCount.First().Text).Should().Be(2);
  }

  [Fact]
  public async Task DeleteProject_ShouldDisappearFromProjectList_WhenSucceeded()
  {
    // Arrange
    _loginPage.BootstrapSession();

    // Act
    _testContext.ShouldContainText("Project Manager", 2); // Page title and project title
    _homePage.DeleteProject();

    // Assert
    _testContext.ShouldContainText("Project Manager", 1);
  }

  [Fact]
  public async Task GenerateInvitationLink_ShouldBeAbleToJoinProjectByAnotherAccount()
  {
    // Arrange
    _loginPage.BootstrapSession();

    // Act
    _homePage.GenerateInvitationLink();
    var jsExec = (IJavaScriptExecutor)_testContext.Driver;
    await Task.Delay(1000);
    var invitationLink = (string)jsExec.ExecuteScript("return arguments[0].value", _homePage.InvitationLink);
    _homePage.CloseInvitationModal.Click();
    _homePage.LogoutBtn.Click();
    _loginPage.LoginAsAbigailMurray();
    _testContext.Driver.Navigate().GoToUrl(invitationLink);
    _testContext.ShouldContainText("You have been invited to join");
    _homePage.JoinProjectBtn.Click();

    // Assert
    var dupa = _testContext.UrlShouldContain("general");
  }

  [Fact]
  public async Task SendPrivateMessage_ShouldAppearOnChat()
  {
    // Arrange
    _loginPage.BootstrapSession();

    // Act
    _homePage.OpenFirstConversationAndSendMessage();

    // Assert
    _testContext.ShouldContainText("Test message");
  }

  public Task InitializeAsync() => Task.CompletedTask;
  public Task DisposeAsync() => _testContext.ResetDriver();
}
