using OpenQA.Selenium;
using selenium_web.Adapters.Interfaces;

namespace selenium_web.Adapters;

public class SeleniumScreenshotAdapter(MyWebDriver driver) : IScreenshotService
{
    public string CaptureScreenshot(string url)
    {
        try
        {
            driver.Navigate().GoToUrl(url);
            var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            return screenshot.AsBase64EncodedString;
        }
        catch (Exception ex)
        {
            throw new Exception("Por favor, insira um URL válido.", ex);
        }
    }
}