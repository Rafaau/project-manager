using ProjectManager.E2ETests.Pages.PageObjects;
using Xunit;

namespace ProjectManager.E2ETests.Pages;

[Collection("Test collection")]
public class CalendarPageTests : IAsyncLifetime
{
  private readonly SharedTestContext _testContext;
  private readonly LoginPage _loginPage;
  private readonly CalendarPage _calendarPage;

  public CalendarPageTests(SharedTestContext testContext)
  {
    _testContext = testContext;
    _loginPage = new LoginPage(testContext.Driver, testContext.Wait);
    _calendarPage = new CalendarPage(testContext.Driver, testContext.Wait);
  }

  [Fact]
  public async Task ClickCalendarNavButton_ShouldRedirectToCalendarPage()
  {
    // Arrange
    _loginPage.BootstrapSession();

    // Act
    _calendarPage.GoToCalendarPage();

    // Assert
    _testContext.UrlShouldContain("general-calendar");
    _testContext.ShouldContainText(DateTime.UtcNow.Month.ToString());
  }

  [Fact]
  public async Task CheckMonthNavigation_ShouldRenderOtherMonth_AfterClick()
  {
    // Arrange
    _loginPage.BootstrapSession();
    _calendarPage.GoToCalendarPage();

    // Act
    _calendarPage.NextMonthBtn.Click();

    // Assert
    _testContext.ShouldContainText(DateTime.UtcNow.AddMonths(1).Month.ToString());

    // Act
    _calendarPage.PreviousMonthBtn.Click();
    _calendarPage.PreviousMonthBtn.Click();

    // Assert
    _testContext.ShouldContainText(DateTime.UtcNow.AddMonths(-1).Month.ToString());

    // Act
    _calendarPage.TodayBtn.Click();

    //Assert
    _testContext.ShouldContainText(DateTime.UtcNow.Month.ToString());
  }

  [Fact]
  public async Task CheckAppointment_ShouldShowDetails_AfterClick()
  {
    // Arrange
    _loginPage.BootstrapSession();
    _calendarPage.GoToCalendarPage();

    // Act
    _calendarPage.FirstAppointment.Click();

    // Assert
    _testContext.ShouldContainText("Weekly");
    _testContext.ShouldContainText(DateTime.UtcNow.AddDays(3).ToString("MMMM dd  yyyy"));
  }

  [Fact]
  public async Task CheckAssignment_ShouldShowDetails_AfterClick()
  {
    // Arrange
    _loginPage.BootstrapSession();
    _calendarPage.GoToCalendarPage();

    // Act
    _calendarPage.FirstAssignment.Click();

    // Assert
    _testContext.ShouldContainText("Refactor services");
    _testContext.ShouldContainText(DateTime.UtcNow.AddDays(4).ToString("MMMM dd  yyyy"));
  }

  [Fact]
  public async Task CreateAppointment_ShouldAppearInCalendar()
  {
    // Arrange
    _loginPage.BootstrapSession();
    _calendarPage.GoToCalendarPage();

    // Act
    _calendarPage.CreateAppointment();
    await Task.Delay(1000);
    _calendarPage.FirstAppointment.Click();

    // Assert
    _testContext.ShouldContainText("Test appointment");
    _testContext.ShouldContainText(DateTime.UtcNow.AddDays(1).ToString("MMMM dd  yyyy"));
  }

  public Task InitializeAsync() => Task.CompletedTask;
  public Task DisposeAsync() => _testContext.ResetDriver();
}
