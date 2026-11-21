using OpenQA.Selenium;
using selenium_web.Adapters.Interfaces;

namespace selenium_web.Adapters;

public class SeleniumHeadingsStructureAdapter(MyWebDriver driver) : IHeadingsService
{
    public List<Heading> GetHeadings(string url)
    {
        driver.NavigateAndWaitForPage(url);
        return driver
            .FindElements(By.CssSelector("h1,h2,h3,h4,h5,h6"))
            .Select(ToHeading)
            .ToList();
    }

    // Element.Text only returns visible text, so headings for screen readers or inside
    // closed menus come back empty. Their textContent still has the text.
    private static Heading ToHeading(IWebElement element)
    {
        var level = int.Parse(element.TagName.Substring(1));

        var visibleText = element.Text.Trim();
        if (visibleText != "")
            return new Heading { Level = level, Text = visibleText, Visibility = HeadingVisibility.Visible };

        var hiddenText = element.GetAttribute("textContent")?.Trim() ?? "";
        return hiddenText != ""
            ? new Heading { Level = level, Text = hiddenText, Visibility = HeadingVisibility.Hidden }
            : new Heading { Level = level, Text = "", Visibility = HeadingVisibility.Empty };
    }
}
