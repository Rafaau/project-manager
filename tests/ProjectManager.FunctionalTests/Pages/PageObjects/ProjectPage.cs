using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace ProjectManager.E2ETests.Pages.PageObjects;

public class ProjectPage
{
  private readonly IWebDriver _driver;
  private readonly WebDriverWait _wait;

  public ProjectPage(IWebDriver driver, WebDriverWait wait)
  {
    _driver = driver;
    _wait = wait;
  }

  #region WebElements
  public IWebElement ChatNavBtn => _wait.Until(e => e.FindElement(By.Id("chat-nav-btn")));
  public IWebElement BoardNavBtn => _wait.Until(e => e.FindElement(By.Id("board-nav-btn")));
  public IWebElement FirstMember => _wait.Until(e => e.FindElement(By.Id("single-member")));
  public IWebElement ChatMessageInput => _wait.Until(e => e.FindElement(By.Id("chat-message-input")));
  public IWebElement MessageToEdit => _wait.Until(e => e.FindElements(By.ClassName("single-message"))).Last();
  public IWebElement EditMessageBtn => _wait.Until(e => e.FindElement(By.Id("edit-message-btn")));
  public IWebElement EditMessageInput => _wait.Until(e => e.FindElement(By.Id("edit-message-input")));
  public IWebElement DeleteMessageBtn => _wait.Until(e => e.FindElement(By.Id("delete-message-btn")));
  public IWebElement ConfirmDeleteBtn => _wait.Until(e => e.FindElement(By.Id("confirm-delete-btn")));
  public IWebElement CreateChatChannelBtn => _wait.Until(e => e.FindElement(By.Id("dropdownCreateChatChannel")));
  public IWebElement ChannelNameInput => _wait.Until(e => e.FindElement(By.Id("channel-name-input")));
  public IWebElement SubmitChatChannel => _wait.Until(e => e.FindElement(By.Id("createChannel")));
  public IWebElement CreatedChannel => _wait.Until(e => e.FindElements(By.Id("chat-channel-tab"))).Last();
  public IWebElement DeleteChannelBtn => _wait.Until(e => e.FindElements(By.Id("delete-channel-btn"))).Last();
  public IWebElement ConfirmDeleteChannel => _wait.Until(e => e.FindElement(By.Id("confirm-delete-channel")));
  public IWebElement FirstAssignment => _wait.Until(e => e.FindElements(By.Id("assignment-to-drag"))).First();
  public IWebElement SourceStage => _wait.Until(e => e.FindElement(By.Id("stage-column-1")));
  public IWebElement DestinationStage => _wait.Until(e => e.FindElement(By.Id("stage-column-2")));
  public IWebElement AddAssignmentBtn => _wait.Until(e => e.FindElements(By.Id("add-assignment-btn"))).Skip(1).First();
  public IWebElement AssignmentNameInput => _wait.Until(e => e.FindElement(By.Id("assignment-name-input")));
  public IWebElement AssignmentDescriptionInput => _wait.Until(e => e.FindElement(By.Id("assignment-description-input")));
  public IWebElement AssignmentDateInput => _wait.Until(e => e.FindElement(By.Id("assignment-date-input")));
  public IWebElement BoundDevsDropdown => _wait.Until(e => e.FindElement(By.Id("boundDevsDropdown")));
  public IWebElement DevToBound => _wait.Until(e => e.FindElements(By.Id("user-to-bound"))).First();
  public IWebElement SubmitAssignmentBtn => _wait.Until(e => e.FindElement(By.Id("submit-assignment-btn")));
  public IWebElement CreatedAssignment => _wait.Until(e => e.FindElements(By.Id("assignment-to-drag"))).Last();
  public IWebElement DeleteStageBtn => _wait.Until(e => e.FindElements(By.Id("delete-stage-btn"))).Last();
  public IWebElement DeleteStageConfirm => _wait.Until(e => e.FindElement(By.Id("delete-stage-confirm")));
  public IWebElement EditStageBtn => _wait.Until(e => e.FindElements(By.Id("edit-stage-btn"))).Last();
  public IWebElement EditStageInput => _wait.Until(e => e.FindElement(By.Id("edit-stage-input")));
  public IWebElement EditStageSave => _wait.Until(e => e.FindElement(By.Id("edit-stage-save")));
  public IWebElement AddNewStageBtn => _wait.Until(e => e.FindElement(By.ClassName("new-stage-button")));
  #endregion

  public void SendChatMessage()
  {
    ChatNavBtn.Click();
    ChatMessageInput.SendKeys("Test chat message @Hugh Campbell");
    ChatMessageInput.SendKeys(Keys.Enter);
  }

  public void EditChatMessage()
  {
    _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[contains(text(), 'Test chat message')]")));
    MessageToEdit.HoverElement(_driver);
    EditMessageBtn.Click();
    EditMessageInput.Clear();
    EditMessageInput.SendKeys("Edited chat message");
    EditMessageInput.SendKeys(Keys.Enter);
  }

  public void DeleteChatMessage()
  {
    _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[contains(text(), 'Test chat message')]")));
    MessageToEdit.HoverElement(_driver);
    DeleteMessageBtn.Click();
    _wait.Until(ExpectedConditions.ElementIsVisible(By.Id("confirm-delete-btn")));
    ConfirmDeleteBtn.Click();
  }

  public void CreateChatChannel()
  {
    ChatNavBtn.Click();
    CreateChatChannelBtn.Click();
    ChannelNameInput.SendKeys("Test channel");
    SubmitChatChannel.Click();
    Thread.Sleep(1000);
    CreatedChannel.Click();
  }

  public void DeleteChatChannel()
  {
    CreatedChannel.HoverElement(_driver);
    DeleteChannelBtn.Click();
    _wait.Until(ExpectedConditions.ElementIsVisible(By.Id("confirm-delete-channel")));
    ConfirmDeleteChannel.Click();
  }

  public void DragAssignmentToTheNextStage()
  {
    BoardNavBtn.Click();
    Actions action = new Actions(_driver);
    Thread.Sleep(1000);
    _wait.Until(ExpectedConditions.ElementIsVisible(By.Id("assignment-to-drag")));
    action.DragAndDrop(FirstAssignment, DestinationStage).Build().Perform();
  }

  public void CreateAssignment()
  {
    BoardNavBtn.Click();
    _wait.Until(ExpectedConditions.ElementIsVisible(By.Id("add-assignment-btn")));
    AddAssignmentBtn.Click();
    _wait.Until(ExpectedConditions.ElementIsVisible(By.Id("assignment-name-input")));
    AssignmentNameInput.SendKeys("Test assignment");
    AssignmentDescriptionInput.SendKeys("Test assignment description");
    AssignmentDateInput.SendKeys(DateTime.UtcNow.AddDays(2).Date.ToString());
    BoundDevsDropdown.Click();
    _wait.Until(ExpectedConditions.ElementIsVisible(By.Id("user-to-bound")));
    DevToBound.Click();
    SubmitAssignmentBtn.Click();
    _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@id='stage-column-2']//div[@id='assignment-to-drag']")));
  }

  public void DeleteAssignmentStage()
  {
    BoardNavBtn.Click();
    _wait.Until(ExpectedConditions.ElementIsVisible(By.Id("delete-stage-btn")));
    DeleteStageBtn.Click();
    _wait.Until(ExpectedConditions.ElementIsVisible(By.Id("delete-stage-confirm")));
    DeleteStageConfirm.Click();
    _wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[contains(text(), 'Done')]")));
  }

  public void EditAssignmentStage()
  {
    BoardNavBtn.Click();
    _wait.Until(ExpectedConditions.ElementIsVisible(By.Id("edit-stage-btn")));
    EditStageBtn.Click();
    _wait.Until(ExpectedConditions.ElementIsVisible(By.Id("edit-stage-input")));
    EditStageInput.Clear();
    EditStageInput.SendKeys("Undone");
    EditStageSave.Click();
    _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[contains(text(), 'Undone')]")));
  }

  public void AddNewAssignmentStage()
  {
    BoardNavBtn.Click();
    _wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("new-stage-button")));
    AddNewStageBtn.Click();
    _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[contains(text(), 'New')]")));
  }
}
