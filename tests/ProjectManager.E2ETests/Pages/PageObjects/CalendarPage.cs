using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace ProjectManager.E2ETests.Pages.PageObjects;
public class CalendarPage
{
  private readonly IWebDriver _driver;
  private readonly WebDriverWait _wait;

  public CalendarPage(IWebDriver driver, WebDriverWait wait)
  {
    _driver = driver;
    _wait = wait;
  }

  #region WebElements
  public IWebElement CalendarNavBtn => _wait.Until(e => e.FindElement(By.Id("calendar-nav-btn")));
  public IWebElement NextMonthBtn => _wait.Until(e => e.FindElement(By.Id("next-month-btn")));
  public IWebElement PreviousMonthBtn => _wait.Until(e => e.FindElement(By.Id("previous-month-btn")));
  public IWebElement TodayBtn => _wait.Until(e => e.FindElement(By.Id("today-btn")));
  public IWebElement FirstAppointment => _wait.Until(e => e.FindElement(By.Id("appointmentDetailsDropdown")));
  public IWebElement FirstAssignment => _wait.Until(e => e.FindElements(By.Id("appointmentDetailsDropdown"))).Skip(1).First();
  public IWebElement NextDay => _wait.Until(e => e.FindElement(By.Id($"day-{DateTime.UtcNow.AddDays(1).Day}")));
  public IWebElement AddAppointmentBtn => _wait.Until(e => e.FindElement(By.Id("add-appointment-btn")));
  public IWebElement AppointmentNameInput => _wait.Until(e => e.FindElement(By.Id("appointment-name-input")));
  public IWebElement AppointmentDescriptionInput => _wait.Until(e => e.FindElement(By.Id("appointment-description-input")));
  public IWebElement AppointmentDateInput => _wait.Until(e => e.FindElement(By.Id("appointment-date-input")));
  public IWebElement SubmitAppointmentBtn => _wait.Until(e => e.FindElement(By.Id("submit-appointment-btn")));
  #endregion

  public void GoToCalendarPage()
  {
    _wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[contains(text(), 'Managed projects')]"))); // user is not null
    CalendarNavBtn.Click();
    _wait.Until(ExpectedConditions.ElementExists(By.Id("appointmentDetailsDropdown"))); // appointments loaded
  }

  public void CreateAppointment()
  {
    NextDay.HoverElement(_driver);
    _wait.Until(ExpectedConditions.ElementIsVisible(By.Id("add-appointment-btn")));
    AddAppointmentBtn.Click();
    _wait.Until(ExpectedConditions.ElementIsVisible(By.Id("appointment-name-input")));
    AppointmentNameInput.SendKeys("Test appointment");
    AppointmentDescriptionInput.SendKeys("Test appointment description");
    AppointmentDateInput.SendKeys($"{DateTime.UtcNow.Date}");
    SubmitAppointmentBtn.Click();
  }
}
