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

        [Fact]
        [Trait("Category", "Links")]
        [Trait("Description", "Verify that only external links with text are listed")]
        public void OnlyExternalLinksAreListed()
        {
            _homePage.Navigate();
            _homePage.ConfirmOnlyExternalLinks();
        }

        [Fact]
        [Trait("Category", "ReadingMode")]
        [Trait("Description", "Verify that reading mode shows only the article content")]
        public void ReadingModeShowsOnlyArticle()
        {
            _homePage.Navigate();
            _homePage.ConfirmReadingModeShowsOnlyArticle();
        }

        [Fact]
        [Trait("Category", "ReadingMode")]
        [Trait("Description", "Verify that reading mode shows text only, without links or images")]
        public void ReadingModeShowsTextOnly()
        {
            _homePage.Navigate();
            _homePage.ConfirmReadingModeShowsTextOnly();
        }

        [Fact]
        [Trait("Category", "ReadingMode")]
        [Trait("Description", "Verify that scripts from the inspected page do not run in the app")]
        public void ReadingModeIsSanitised()
        {
            _homePage.Navigate();
            _homePage.ConfirmReadingModeIsSanitised();
        }

        [Fact]
        [Trait("Category", "ReadingMode")]
        [Trait("Description", "Verify that an error is shown when the page has no main content")]
        public void ReadingModeErrorWithoutContent()
        {
            _homePage.Navigate();
            _homePage.ConfirmReadingModeErrorWithoutContent();
        }

        [Fact]
        [Trait("Category", "Headings")]
        [Trait("Description", "Verify that headings are listed with their level, text and hierarchy, flagging hidden and empty ones")]
        public void HeadingsStructureIsListed()
        {
            _homePage.Navigate();
            _homePage.ConfirmHeadingsStructure();
        }

        [Fact]
        [Trait("Category", "ReadingMode")]
        [Trait("Description", "Verify that reading mode waits for content rendered by JavaScript after the page loads")]
        public void ReadingModeWaitsForJavaScriptContent()
        {
            _homePage.Navigate();
            _homePage.ConfirmReadingModeWaitsForJavaScriptContent();
        }

        [Fact]
        [Trait("Category", "Links")]
        [Trait("Description", "Verify that a message is shown when the page has no external links")]
        public void MessageWhenNoExternalLinks()
        {
            _homePage.Navigate();
            _homePage.ConfirmMessageWhenNoExternalLinks();
        }

        [Fact]
        [Trait("Category", "Loading")]
        [Trait("Description", "Verify that a loading message replaces the buttons while the app is working")]
        public void LoadingMessageIsShown()
        {
            _homePage.Navigate();
            _homePage.ConfirmLoadingMessageWhileWorking();
        }

        // Ensure WebDriver quits properly
        public void Dispose()
        {
            _driver.Quit();
            _driver.Dispose();
        }
    }
}