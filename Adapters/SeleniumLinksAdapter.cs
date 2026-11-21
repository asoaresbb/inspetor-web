using OpenQA.Selenium;
using selenium_web.Adapters.Interfaces;

namespace selenium_web.Adapters;

public class SeleniumLinksAdapter(MyWebDriver driver) : ILinksService
{
    public List<Link> GetLinks(string url)
    {
        driver.Navigate().GoToUrl(url);
        return driver
            .FindElements(By.TagName("a"))
            .Select(element =>
                new Link
                {
                    Url = element.GetAttribute("href"),
                    Text = element.Text
                })
            .ToList();
    }
    
    public async Task<List<Link>> GetLinksAsync(string url)
    {
        return await Task.Run(() => GetLinks(url));
    }
}