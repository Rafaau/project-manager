using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using OpenQA.Selenium;
using ProjectManager.E2ETests.Pages.PageObjects;
using SeleniumExtras.WaitHelpers;
using Xunit;

namespace ProjectManager.E2ETests.Pages;

[Collection("Test collection")]
public class ProjectPageTests : IAsyncLifetime
{
  private readonly SharedTestContext _testContext;
  private readonly LoginPage _loginPage;
  private readonly HomePage _homePage;
  private readonly ProjectPage _projectPage;

  public ProjectPageTests(SharedTestContext testContext)
  {
    _testContext = testContext;
    _loginPage = new LoginPage(testContext.Driver, testContext.Wait);
    _homePage = new HomePage(testContext.Driver, testContext.Wait);
    _projectPage = new ProjectPage(testContext.Driver, testContext.Wait);
  }

  [Fact]
  public async Task CreateProject_ShouldHaveCreatedChatChannelAndAssignmentStages()
  {
    // Arrange
    _loginPage.BootstrapSession();
    _homePage.CreateProjectAndOpenIt();

    // Act
    _projectPage.ChatNavBtn.Click();

    // Assert
    _testContext.ShouldContainText("General");

    // Act
    _projectPage.BoardNavBtn.Click();

    // Assert
    _testContext.ShouldContainText("To Do");
    _testContext.ShouldContainText("In Progress");
    _testContext.ShouldContainText("Done");
  }

  [Fact]
  public async Task CheckMemberList_ShouldShowDetails_AfterClick()
  {
    // Arrange
    _loginPage.BootstrapSession();
    _homePage.ProjectTab.Click();

    // Act
    _projectPage.FirstMember.Click();

    // Assert
    _testContext.ShouldContainText("Specialization");
    _testContext.ShouldContainText("Assignments");
  }

  [Fact]
  public async Task SendChatMessageWithMention_ShouldAppearInTheChat()
  {
    // Arrange
    _loginPage.BootstrapSession();
    _homePage.ProjectTab.Click();

    // Act
    _projectPage.SendChatMessage();

    // Assert
    _testContext.ShouldContainText("Test chat message");
    _testContext.ShouldContainElements("mention", 3); // 2 existing + new one
  }

  [Fact]
  public async Task SendChatMessageAndEdit_ShouldUpdateTheMessage()
  {
    // Arrange
    _loginPage.BootstrapSession();
    _homePage.ProjectTab.Click();

    // Act
    _projectPage.SendChatMessage();
    _projectPage.EditChatMessage();

    // Assert
    _testContext.ShouldContainText("Edited chat message");
    _testContext.ShouldContainElements("mention", 2); // because mention has been removed
  }

  [Fact]
  public async Task SendChatMessageAndDelete_ShouldDisappearFromTheChat()
  {
    // Arrange
    _loginPage.BootstrapSession();
    _homePage.ProjectTab.Click();

    // Act
    _projectPage.SendChatMessage();
    _projectPage.DeleteChatMessage();

    // Assert
    _testContext.ShouldNotContainVisibleText("Test chat message");
  }

  [Fact]
  public async Task CreateChatChannel_ShouldBeAbleToSendMessages()
  {
    // Arrange
    _loginPage.BootstrapSession();
    _homePage.ProjectTab.Click();

    // Act
    _projectPage.CreateChatChannel();
    _testContext.ShouldContainText("Test channel");
    _projectPage.SendChatMessage();

    // Assert
    _testContext.ShouldContainText("Test chat message");
    _testContext.ShouldContainElements("mention", 1);
  }

  [Fact]
  public async Task CreateAndDeleteChatChannel_ShouldDisappearFromChannelsList()
  {
    // Arrange
    _loginPage.BootstrapSession();
    _homePage.ProjectTab.Click();

    // Act
    _projectPage.CreateChatChannel();
    _testContext.ShouldContainText("Test channel");
    _projectPage.DeleteChatChannel();

    // Assert
    _testContext.ShouldNotContainText("Test channel");
  }

  [Fact]
  public async Task DragAndPlaceAssignmentToTheNextStage_ShouldBeMovedToTheNextStage()
  {
    // Arrange
    _loginPage.BootstrapSession();
    _homePage.ProjectTab.Click();

    // Act
    _projectPage.DragAssignmentToTheNextStage();

    // Assert
    var expected = _testContext.Driver.FindElements(By.XPath("//div[@id='stage-column-2']//div[@id='assignment-to-drag']"));
    expected.Count().Should().Be(1);
  }

  [Fact]
  public async Task CreateAssignment_ShouldAppearInTheStage_WhenSucceeded()
  {
    // Arrange
    _loginPage.BootstrapSession();
    _homePage.ProjectTab.Click();

    // Act
    _projectPage.CreateAssignment();

    // Assert
    var expected = _testContext.Driver.FindElements(By.XPath("//div[@id='stage-column-2']//div[@id='assignment-to-drag']"));
    expected.Count().Should().Be(1);
    _projectPage.CreatedAssignment.Click();
    _testContext.ShouldContainText("Test assignment description");
  }

  [Fact]
  public async Task DeleteAssignmentStage_ShouldDisappearFromTheBoard()
  {
    // Arrange
    _loginPage.BootstrapSession();
    _homePage.ProjectTab.Click();

    // Act
    _projectPage.DeleteAssignmentStage();

    // Assert
    _testContext.ShouldNotContainText("Done");
  }

  [Fact]
  public async Task EditAssignmentStage_ShouldHaveUpdatedName()
  {
    // Arrange
    _loginPage.BootstrapSession();
    _homePage.ProjectTab.Click();

    // Act
    _projectPage.EditAssignmentStage();

    // Assert
    _testContext.ShouldContainText("Undone");
  }

  [Fact]
  public async Task AddNewAssignmentStage_ShouldAppearOnTheBoard()
  {
    // Arrange
    _loginPage.BootstrapSession();
    _homePage.ProjectTab.Click();

    // Act
    _projectPage.AddNewAssignmentStage();

    // Assert
    _testContext.ShouldContainText("New");
  }

  public Task InitializeAsync() => Task.CompletedTask;

  public Task DisposeAsync() => _testContext.ResetDriver();
}
