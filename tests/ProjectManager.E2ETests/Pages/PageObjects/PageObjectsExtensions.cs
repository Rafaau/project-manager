using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace ProjectManager.E2ETests.Pages.PageObjects;
public static class PageObjectsExtensions
{
  public static void MoveToElementAndClick(this IWebElement webElement, IWebDriver driver)
  {
    Actions act = new Actions(driver);
    act.MoveToElement(webElement).Click().Build().Perform();
  }

  public static void HoverElement(this IWebElement webElement, IWebDriver driver)
  {
    Actions act = new Actions(driver);
    act.MoveToElement(webElement).Perform();
  }
}
