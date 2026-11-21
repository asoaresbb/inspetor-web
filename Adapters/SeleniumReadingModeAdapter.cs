using OpenQA.Selenium;
using selenium_web.Adapters.Interfaces;

namespace selenium_web.Adapters;

public class SeleniumReadingModeAdapter(MyWebDriver driver) : IReadingModeService
{
    public string GetContentReadingMode(string url)
    {
        driver.Navigate().GoToUrl(url);
        try
        {
            return driver.FindElement(By.TagName("article")).GetAttribute("innerHTML");
        }
        catch (NoSuchElementException)
        {
            try
            {
                return driver.FindElement(By.TagName("main")).GetAttribute("innerHTML");
            }
            catch (NoSuchElementException)
            {
                try
                {
                    return driver.FindElement(By.Id("content")).GetAttribute("innerHTML");
                }
                catch (NoSuchElementException)
                {
                    throw new Exception("Sem conteúdo a mostrar");
                }
            }
        }
    }
}