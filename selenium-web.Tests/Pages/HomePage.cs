using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Xunit;
// Add this for WebDriverWait

namespace selenium_web.selenium_web.Tests.Pages
{
    public class HomePage(IWebDriver driver)
    {
        private const string Url = "http://localhost:5206/";

        public void Navigate()
        {
            Console.WriteLine("Navigating to the homepage...");
            driver.Navigate().GoToUrl(Url);
            Console.WriteLine("Page loaded successfully.");
        }

        public void ConfirmScreenshot()
        {
            driver.FindElement(By.Id("urlInput")).SendKeys("https://getbootstrap.com/");
            driver.FindElement(By.Id("btnAction1")).Click();

            var screenshotTaken = new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(d => d.FindElement(By.Id("screenshotTaken")));

            var screenshotImage = screenshotTaken.FindElement(By.TagName("img"));
            Assert.NotNull(screenshotTaken.FindElement(By.TagName("img")));
            Assert.False(string.IsNullOrEmpty(screenshotImage.GetAttribute("src")));

            Thread.Sleep(1500);
        }

        public void ConfirmErrorScreenshot()
        {
            driver.FindElement(By.Id("urlInput")).SendKeys("https://www.000.com/");
            driver.FindElement(By.Id("btnAction1")).Click();
            
            var expectedErrorMessage = new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(d => d.FindElement(By.Id("errorMessage")));

            Assert.Equal("Error capturing screenshot, probably the URL does not exist.", expectedErrorMessage.Text);
            
            Thread.Sleep(1500);
        }
    }
}