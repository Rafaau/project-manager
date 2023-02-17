using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace ProjectManager.E2ETests.Pages.PageObjects;

public class LoginPage
{
  private readonly IWebDriver _driver;
  private readonly WebDriverWait _wait;

  public LoginPage(IWebDriver driver, WebDriverWait wait)
  {
    _driver = driver;
    _wait = wait;
  }

  public virtual string Url => "http://localhost:7780/";

  #region WebElements
  public IWebElement LoginInput => _wait.Until(e => e.FindElement(By.Id("login")));
  public IWebElement PasswordInput => _driver.FindElement(By.Id("password"));
  public IWebElement LoginBtn => _driver.FindElement(By.TagName("button"));
  public IWebElement Slider => _driver.FindElement(By.Id("remember-me"));
  public IWebElement SignUpLink => _driver.FindElement(By.Id("sign-up-link"));
  public IWebElement BackToLogin => _driver.FindElement(By.Id("login-link"));
  public IWebElement RegisterFirstname => _driver.FindElement(By.Id("firstname-input"));
  public IWebElement RegisterLastname => _driver.FindElement(By.Id("lastname-input"));
  public IWebElement RegisterEmail => _driver.FindElement(By.Id("email-input"));
  public IWebElement RegisterPassword => _driver.FindElement(By.Id("password-input"));
  public IWebElement RegisterBtn => _driver.FindElement(By.Id("sign-up-btn"));
  public IWebElement TryAgainLink => _driver.FindElement(By.Id("back-to-form-link"));
  public IWebElement BackToSignIn => _driver.FindElement(By.Id("back-to-login-link"));
  #endregion

  public void GoToLoginPage()
  {
    _driver.Navigate().GoToUrl(Url+"login");
  }

  public void GoToHomePage()
  {
    _driver.Navigate().GoToUrl(Url + "pm");
    _wait.Until(e => e.Url == Url + "login");
  }

  public void FillFormAndLogin()
  {
    LoginInput.SendKeys("rooney@gmail.com");
    PasswordInput.SendKeys("pm2022");
    LoginBtn.Click();
    _wait.Until(e => e.Url == Url + "pm");
  }

  public void FillFormCheckRememberMeAndLogin()
  {
    LoginInput.SendKeys("rooney@gmail.com");
    PasswordInput.SendKeys("pm2022");
    Slider.MoveToElementAndClick(_driver);
    LoginBtn.Click();
    _wait.Until(e => e.Url == Url + "pm");
  }

  public void FillFormWithInvalidDataAndLogin()
  {
    LoginInput.SendKeys("test");
    PasswordInput.SendKeys("test");
    Slider.MoveToElementAndClick(_driver);
    LoginBtn.Click();
    _wait.Until(e => e.FindElement(By.XPath("//*[contains(text(), 'Invalid email or password')]")));
  }

  public void GoToRegisterPage()
  {
    _wait.Until(e => e.FindElement(By.Id("sign-up-link")));
    SignUpLink.MoveToElementAndClick(_driver);
    _wait.Until(e => e.Url == Url + "register");
  }

  public void AlreadyHaveAnAccount()
  {
    _wait.Until(e => e.FindElement(By.Id("login-link")));
    BackToLogin.MoveToElementAndClick(_driver);
    _wait.Until(e => e.Url == Url + "login");
  }

  public void FillFormAndRegister()
  {
    _wait.Until(e => e.FindElement(By.Id("firstname-input")));
    RegisterFirstname.SendKeys("Rafau");
    RegisterLastname.SendKeys("Test");
    RegisterEmail.SendKeys("test@gmail.com");
    RegisterPassword.SendKeys("Test123!");
    RegisterBtn.MoveToElementAndClick(_driver);
    _wait.Until(e => e.FindElement(By.XPath("//*[contains(text(), 'SUCCESS')]")));
  }

  public void FillFormWithInvalidDataAndRegister()
  {
    _wait.Until(e => e.FindElement(By.Id("firstname-input")));
    RegisterFirstname.SendKeys("test");
    RegisterLastname.SendKeys("test");
    RegisterEmail.SendKeys("test");
    RegisterPassword.SendKeys("test");
    RegisterBtn.MoveToElementAndClick(_driver);
    _wait.Until(e => e.FindElement(By.XPath("//*[contains(text(), 'SOMETHING WENT WRONG')]")));
  }

  public void ClickTryAgain()
  {
    _wait.Until(e => e.FindElement(By.Id("back-to-form-link")));
    TryAgainLink.Click();
    _wait.Until(e => e.FindElement(By.XPath("//*[contains(text(), 'SIGN UP')]")));
  }

  public void BackToLoginPage()
  {
    _wait.Until(e => e.FindElement(By.Id("back-to-login-link")));
    BackToSignIn.Click();
    _wait.Until(e => e.Url == Url + "login");
  }

  public void FillFormAndLoginWithCreatedAccount()
  {
    LoginInput.SendKeys("test@gmail.com");
    PasswordInput.SendKeys("Test123!");
    LoginBtn.Click();
    _wait.Until(e => e.Url == Url + "pm");
  }

  public void BootstrapSession()
  {
    GoToLoginPage();
    FillFormAndLogin();
    _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[contains(text(), 'Managed projects')]")));
  }

  public void LoginAsAbigailMurray()
  {
    LoginInput.SendKeys("murray@gmail.com");
    PasswordInput.SendKeys("pm2022");
    LoginBtn.Click();
    _wait.Until(e => e.Url == Url + "pm");
  }
}

public static class WebDriverExtensions
{
  public static void GoToHomePage(this IWebDriver driver)
  {
    driver.Navigate().GoToUrl("http://localhost:7780/pm");
  }
}
