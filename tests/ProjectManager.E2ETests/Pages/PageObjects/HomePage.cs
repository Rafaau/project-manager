using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace ProjectManager.E2ETests.Pages.PageObjects;
public class HomePage
{
  private readonly IWebDriver _driver;
  private readonly WebDriverWait _wait;

  public HomePage(IWebDriver driver, WebDriverWait wait)
  {
    _driver = driver;
    _wait = wait;
  }

  #region WebElements
  public IWebElement LogoutBtn => _wait.Until(e => e.FindElement(By.Id("logout-btn")));
  public IWebElement EditProfileBtn => _wait.Until(e => e.FindElement(By.Id("edit-profile-btn")));
  public IWebElement EmailInput => _wait.Until(e => e.FindElement(By.Id("edit-email-input")));
  public IWebElement PasswordInput => _wait.Until(e => e.FindElement(By.Id("edit-password-input")));
  public IWebElement PasswordConfirm => _wait.Until(e => e.FindElement(By.Id("edit-password-confirm")));
  public IWebElement SaveBtn => _wait.Until(e => e.FindElement(By.Id("edit-save-btn")));
  public IWebElement AddProjectBtn => _wait.Until(e => e.FindElement(By.Id("add-project-btn")));
  public IWebElement ProjectNameInput => _wait.Until(e => e.FindElement(By.Id("project-name-input")));
  public IWebElement CreateProjectBtn => _wait.Until(e => e.FindElement(By.Id("create-project-btn")));
  public IWebElement Indicator => _wait.Until(e => e.FindElement(By.Id("indicator-2")));
  public IWebElement NotificationsBtn => _wait.Until(e => e.FindElement(By.Id("notifications-btn")));
  public IWebElement HideNotificationsBtn => _wait.Until(e => e.FindElement(By.Id("hide-notifications-btn")));
  public IWebElement MessagesBtn => _wait.Until(e => e.FindElement(By.Id("messages-btn")));
  public IWebElement FirstConversation => _wait.Until(e => e.FindElements(By.ClassName("single-conversations"))).First();
  public IWebElement CloseConversation => _wait.Until(e => e.FindElement(By.Id("close-conversation-btn")));
  public IWebElement HideMessagesBtn => _wait.Until(e => e.FindElement(By.Id("hide-messages-btn")));
  public IWebElement ProjectTab => _wait.Until(e => e.FindElement(By.Id("project-tab")));
  public IWebElement DeleteProjectBtn => _wait.Until(e => e.FindElement(By.Id("delete-project-btn")));
  public IWebElement ConfirmDeleteInput => _wait.Until(e => e.FindElement(By.Id("confirm-delete-input")));
  public IWebElement ConfirmDeleteProjectBtn => _wait.Until(e => e.FindElement(By.Id("confirm-delete-project-btn")));
  public IWebElement InvitationBtn => _wait.Until(e => e.FindElement(By.Id("invitation-btn")));
  public IWebElement GenerateBtn => _wait.Until(e => e.FindElement(By.Id("generate-btn")));
  public IWebElement InvitationLink => _wait.Until(e => e.FindElement(By.ClassName("link-field")));
  public IWebElement CloseInvitationModal => _wait.Until(e => e.FindElement(By.Id("close-invitation-modal")));
  public IWebElement JoinProjectBtn => _wait.Until(e => e.FindElement(By.Id("join-project-btn")));
  public IWebElement PrivateMessageInput => _wait.Until(e => e.FindElement(By.Id("private-message-input")));
  public IWebElement TestProjectTab => _wait.Until(e => e.FindElement(By.Id("test-project-tab")));
  #endregion

  public void EditProfileAndSave()
  {
    EditProfileBtn.Click();
    EmailInput.SendKeys("test@gmail.com");
    PasswordInput.SendKeys("Test123!");
    PasswordConfirm.SendKeys("Test123!");
    SaveBtn.Click();
  }

  public void CreateProjectAndMoveCarousel()
  {
    AddProjectBtn.Click();
    _driver.SwitchTo().ActiveElement();
    Thread.Sleep(1000);
    ProjectNameInput.SendKeys("Test project");
    CreateProjectBtn.Click();
    Indicator.Click();
  }

  public void CollapseAndHideNotifications()
  {
    NotificationsBtn.Click();
    Thread.Sleep(1000);
    HideNotificationsBtn.Click();
    _wait.Until(ExpectedConditions.ElementExists(By.ClassName("circle-span")));
  }

  public void OpenFirstConversation()
  {
    MessagesBtn.Click();
    Thread.Sleep(1000);
    FirstConversation.Click();
    _driver.SwitchTo().ActiveElement();
    Thread.Sleep(1000);
    CloseConversation.Click();
    HideMessagesBtn.Click();
  }

  public void DeleteProject()
  {
    ProjectTab.HoverElement(_driver);
    DeleteProjectBtn.Click();
    _driver.SwitchTo().ActiveElement();
    Thread.Sleep(1000);
    ConfirmDeleteInput.SendKeys("Project Manager");
    ConfirmDeleteProjectBtn.Click();
    _wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[contains(text(), 'Please type the project name to confirm.')]")));
  }

  public void GenerateInvitationLink()
  {
    ProjectTab.HoverElement(_driver);
    InvitationBtn.Click();
    Thread.Sleep(1000);
    GenerateBtn.Click();
  }

  public void OpenFirstConversationAndSendMessage()
  {
    MessagesBtn.Click();
    Thread.Sleep(1000);
    FirstConversation.Click();
    _driver.SwitchTo().ActiveElement();
    Thread.Sleep(1000);
    PrivateMessageInput.SendKeys("Test message");
    PrivateMessageInput.SendKeys(Keys.Enter);
  }

  public void CreateProjectAndOpenIt()
  {
    AddProjectBtn.Click();
    _driver.SwitchTo().ActiveElement();
    Thread.Sleep(1000);
    ProjectNameInput.SendKeys("Test project");
    CreateProjectBtn.Click();
    Indicator.Click();
    Thread.Sleep(1000);
    TestProjectTab.Click();
    _wait.Until(ExpectedConditions.UrlContains("general"));
  }
}
