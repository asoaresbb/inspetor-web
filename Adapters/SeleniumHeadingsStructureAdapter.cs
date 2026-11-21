using OpenQA.Selenium;
using selenium_web.Adapters.Interfaces;

namespace selenium_web.Adapters;

public class SeleniumHeadingsStructureAdapter(MyWebDriver driver) : IHeadingsService
{
    public List<Heading> GetHeadings(string url)
    {
        driver.Navigate().GoToUrl(url);
        return driver
            .FindElements(By.CssSelector("h1,h2,h3,h4,h5,h6"))
            .Select(element =>
                new Heading 
                {
                    Level = int.Parse(element.TagName.Substring(1)),
                    Text = element.Text
                })
            .ToList();
    }
}