using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using selenium_web.selenium_web.Tests.Pages;
using Xunit;

namespace selenium_web.selenium_web.Tests.Tests
{
    public class HomePageTests : IDisposable
    {
        private readonly ChromeDriver _driver;
        private readonly HomePage _homePage;

        public HomePageTests()
        {
            var options = new ChromeOptions();
            //options.AddArgument("--headless");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");
            
            _driver = new ChromeDriver(options);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
            _homePage = new HomePage(_driver);
        }

        [Fact]
        [Trait("Category", "Screenshot")]
        [Trait("Description", "Verify that a screenshot was taken successfully")]
        public void ScreenshotWasTaken()
        {
            _homePage.Navigate();
            _homePage.ConfirmScreenshot();
        }

        [Fact]
        [Trait("Category", "Screenshot")]
        [Trait("Description", "Verify that an error screenshot was not taken")]
        public void ScreenshotWasNotTaken()
        {
            _homePage.Navigate();
            _homePage.ConfirmErrorScreenshot();
        }

        // Ensure WebDriver quits properly
        public void Dispose()
        {
            _driver.Quit();
            _driver.Dispose();
        }
    }
}