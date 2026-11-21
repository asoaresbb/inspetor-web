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
            driver.FindElement(By.Id("urlInput")).SendKeys(Url + "test-pages/headings.html");
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
            // Nothing listens on this port, so the page can't be opened. (Low ports such as 1
            // are blocked by Chrome as unsafe and hang instead of failing.)
            driver.FindElement(By.Id("urlInput")).SendKeys("http://localhost:59999/");
            driver.FindElement(By.Id("btnAction1")).Click();
            
            var expectedErrorMessage = new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(d => d.FindElement(By.Id("errorMessage")));

            Assert.Equal("Error capturing screenshot, probably the URL does not exist.", expectedErrorMessage.Text);
            
            Thread.Sleep(1500);
        }

        public void ConfirmOnlyExternalLinks()
        {
            driver.FindElement(By.Id("urlInput")).SendKeys(Url + "test-pages/links.html");
            driver.FindElement(By.CssSelector("button[value='links']")).Click();

            var links = WaitForResult("externalLinks").FindElements(By.TagName("a"));

            // Internal, mailto: and javascript: links are left out. The icon-only link and the
            // hidden one get their text from aria-label and textContent instead of coming back empty
            Assert.Equal(["Example site", "GitHub", "Wikipedia"], links.Select(link => link.Text));
            Assert.Equal(
                ["https://example.com/", "https://github.com/", "https://www.wikipedia.org/"],
                links.Select(link => link.GetAttribute("href")));
        }

        public void ConfirmReadingModeShowsOnlyArticle()
        {
            var result = OpenReadingMode("test-pages/reading-mode.html");

            Assert.Contains("Article title", result.Text);
            Assert.Contains("Article body text with a link.", result.Text);
            Assert.Contains("Second item", result.Text);

            // Every article on the page, separated, with nested ones (e.g. comments) kept in their parent
            Assert.Contains("Second article text.", result.Text);
            Assert.Contains("Nested comment text.", result.Text);
            Assert.Single(result.FindElements(By.CssSelector(".reading-separator")));
            Assert.DoesNotContain("We use cookies", result.Text);
            Assert.DoesNotContain("Footer text", result.Text);
        }

        public void ConfirmReadingModeShowsTextOnly()
        {
            var result = OpenReadingMode("test-pages/reading-mode.html");

            Assert.Empty(result.FindElements(By.CssSelector(".reading-mode a")));
            Assert.Empty(result.FindElements(By.CssSelector(".reading-mode img")));
        }

        public void ConfirmReadingModeIsSanitised()
        {
            var result = OpenReadingMode("test-pages/reading-mode.html");

            // onerror only fires once the broken image has finished loading
            new WebDriverWait(driver, TimeSpan.FromSeconds(10)).Until(d =>
                (bool)((IJavaScriptExecutor)d).ExecuteScript(
                    "return [...document.images].every(image => image.complete)"));

            Assert.Empty(result.FindElements(By.TagName("script")));
            Assert.Empty(result.FindElements(By.CssSelector("[onerror]")));
            Assert.Null(driver.FindElement(By.TagName("body")).GetAttribute("data-xss"));
        }

        public void ConfirmReadingModeErrorWithoutContent()
        {
            driver.FindElement(By.Id("urlInput")).SendKeys(Url + "test-pages/no-content.html");
            driver.FindElement(By.CssSelector("button[value='readingmode']")).Click();

            var errorMessage = new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(d => d.FindElement(By.Id("errorMessage")));

            Assert.Equal("No article, main or #content found on this page.", errorMessage.Text);
        }

        public void ConfirmHeadingsStructure()
        {
            driver.FindElement(By.Id("urlInput")).SendKeys(Url + "test-pages/headings.html");
            driver.FindElement(By.CssSelector("button[value='headingsStructure']")).Click();

            var items = WaitForResult("headingsResult").FindElements(By.TagName("li"));

            Assert.Equal(
                ["H1 Main title", "H2 Section", "H3 Subsection", "H2 Menu title (hidden)", "H3 (empty)"],
                items.Select(item => item.Text));

            // Each level is indented further than the one above it
            var indents = items.Take(3).Select(item => item.Location.X).ToList();
            Assert.True(indents[0] < indents[1] && indents[1] < indents[2],
                $"Expected increasing indentation, got {string.Join(", ", indents)}");
        }

        public void ConfirmReadingModeWaitsForJavaScriptContent()
        {
            var result = OpenReadingMode("test-pages/delayed.html");

            Assert.Contains("Content rendered by JavaScript.", result.Text);
        }

        public void ConfirmMessageWhenNoExternalLinks()
        {
            // The headings page has no links at all
            driver.FindElement(By.Id("urlInput")).SendKeys(Url + "test-pages/headings.html");
            driver.FindElement(By.CssSelector("button[value='links']")).Click();

            var message = new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(d => d.FindElement(By.Id("noResults")));

            Assert.Equal("No external links found on this page.", message.Text);
        }

        public void ConfirmLoadingMessageWhileWorking()
        {
            // ChromeDriver waits for the next page to load after a click that submits, so by
            // then the loading message is gone. Cancel the submit instead: the page's own
            // submit handler still runs and shows the message.
            ((IJavaScriptExecutor)driver).ExecuteScript(
                "document.forms[0].addEventListener('submit', event => event.preventDefault())");

            driver.FindElement(By.Id("urlInput")).SendKeys(Url + "test-pages/headings.html");
            driver.FindElement(By.CssSelector("button[value='readingmode']")).Click();

            var loading = new WebDriverWait(driver, TimeSpan.FromSeconds(2))
                .Until(d => d.FindElement(By.Id("loadingMessage")) is { Displayed: true } element ? element : null);
            Assert.Contains("Extracting the main content...", loading.Text);
            Assert.False(driver.FindElement(By.Id("actionButtons")).Displayed);
        }

        private IWebElement OpenReadingMode(string path)
        {
            driver.FindElement(By.Id("urlInput")).SendKeys(Url + path);
            driver.FindElement(By.CssSelector("button[value='readingmode']")).Click();

            return WaitForResult("readingModeResult");
        }

        // Waits for the result element. If the app shows something else instead, such as an
        // error message or an exception page, fails with what it showed.
        private IWebElement WaitForResult(string resultId)
        {
            try
            {
                return new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                    .Until(d => d.FindElement(By.Id(resultId)));
            }
            catch (WebDriverTimeoutException)
            {
                var errors = driver.FindElements(By.Id("errorMessage"));
                var shown = errors.Count > 0
                    ? $"error message \"{errors[0].Text}\""
                    : $"page \"{driver.Title}\"";
                Assert.Fail($"Expected #{resultId}, but the app showed {shown}");
                throw;
            }
        }
    }
}
