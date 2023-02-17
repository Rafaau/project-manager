using FluentAssertions;
using FluentAssertions.Collections;
using FluentAssertions.Numeric;
using FluentAssertions.Primitives;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;

namespace ProjectManager.E2ETests.Pages;
public static class AssertExtensions
{
  public static AndConstraint<GenericCollectionAssertions<IWebElement>> ShouldContainText(this SharedTestContext context, string textToFind)
  {
    context.Wait.Until(ExpectedConditions.ElementExists(By.XPath($"//*[contains(text(), '{textToFind}')]")));
    var expected = context.Driver.FindElements(By.XPath($"//*[contains(text(), '{textToFind}')]"));
    return expected.Should().NotBeEmpty();
  }

  public static AndConstraint<NumericAssertions<int>> ShouldContainText(this SharedTestContext context, string textToFind, int count)
  {
    context.Wait.Until(ExpectedConditions.ElementExists(By.XPath($"//*[contains(text(), '{textToFind}')]")));
    var expected = context.Driver.FindElements(By.XPath($"//*[contains(text(), '{textToFind}')]"));
    return expected.Count.Should().Be(count);
  }

  public static AndConstraint<GenericCollectionAssertions<IWebElement>> ShouldNotContainText(this SharedTestContext context, string textToFind)
  {
    context.Wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath($"//*[contains(text(), '{textToFind}')]")));
    var expected = context.Driver.FindElements(By.XPath($"//*[contains(text(), '{textToFind}')]"));
    return expected.Should().BeEmpty();
  }

  public static AndConstraint<BooleanAssertions> ShouldNotContainVisibleText(this SharedTestContext context, string textToFind)
  {
    context.Wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath($"//*[contains(text(), '{textToFind}')]")));
    var expected = context.Driver.FindElement(By.XPath($"//*[contains(text(), '{textToFind}')]"));
    return expected.Displayed.Should().BeFalse();
  }

  public static AndConstraint<NumericAssertions<int>> ShouldContainElements(this SharedTestContext context, string className, int count)
  {
    context.Wait.Until(ExpectedConditions.ElementExists(By.ClassName(className)));
    var expected = context.Driver.FindElements(By.ClassName(className));
    return expected.Count.Should().Be(count);
  }

  public static AndConstraint<StringAssertions> UrlShouldContain(this SharedTestContext context, string expected)
  {
    context.Wait.Until(ExpectedConditions.UrlContains(expected));
    return context.Driver.Url.Should().Contain(expected);
  }
}
