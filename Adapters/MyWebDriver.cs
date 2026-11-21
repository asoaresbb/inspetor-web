using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace selenium_web.Adapters;

public class MyWebDriver : ChromeDriver, IDisposable
{
    // How long the page must go without DOM changes, after it has loaded, to count as
    // finished rendering, and the most we wait in total (animated spinners never go quiet)
    private const int QuietMilliseconds = 1_000;
    private const int MaxWaitMilliseconds = 10_000;

    private const string WaitForQuietDomScript = """
        const done = arguments[arguments.length - 1];
        const [quietMs, maxMs] = [arguments[0], arguments[1]];
        let finished = false;
        let quietTimer;
        let observer;
        const finish = () => {
            if (finished) return;
            finished = true;
            observer?.disconnect();
            clearTimeout(quietTimer);
            clearTimeout(maxTimer);
            done();
        };
        const maxTimer = setTimeout(finish, maxMs);
        const waitForQuiet = () => {
            observer = new MutationObserver(() => {
                clearTimeout(quietTimer);
                quietTimer = setTimeout(finish, quietMs);
            });
            observer.observe(document, { subtree: true, childList: true, attributes: true, characterData: true });
            quietTimer = setTimeout(finish, quietMs);
        };
        // Scripts that build the page may still be downloading, so start after the load event
        if (document.readyState === 'complete') waitForQuiet();
        else window.addEventListener('load', waitForQuiet, { once: true });
        """;

    private bool _disposed;

    public MyWebDriver() : base(CreateDriverService(), CreateDefaultOptions())
    {
        Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
        Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromMilliseconds(MaxWaitMilliseconds + 5_000);
    }

    /// <summary>
    /// Opens the URL and waits until the page has finished rendering. Single-page apps
    /// show a loading screen and build the content with JavaScript after the page loads,
    /// so waiting for the load event alone isn't enough.
    /// </summary>
    public void NavigateAndWaitForPage(string url)
    {
        Navigate().GoToUrl(url);
        ExecuteAsyncScript(WaitForQuietDomScript, QuietMilliseconds, MaxWaitMilliseconds);
    }

    public new void Dispose()
    {
        if (!_disposed)
        {
            Quit();
            base.Dispose();
            _disposed = true;
        }
    }

    // Downloads the driver for the installed Chrome version, once per app run instead of on
    // every request. PublicationOnly so a failed attempt (e.g. offline) is retried next time.
    private static readonly Lazy<string> DriverPath = new(
        () => new DriverManager().SetUpDriver(new ChromeConfig()),
        LazyThreadSafetyMode.PublicationOnly);

    private static ChromeDriverService CreateDriverService()
    {
        _ = DriverPath.Value;
        return ChromeDriverService.CreateDefaultService();
    }

    private static ChromeOptions CreateDefaultOptions()
    {
        var options = new ChromeOptions
        {
            // Return once the HTML is parsed instead of waiting for every image and script;
            // NavigateAndWaitForPage then waits for the page to finish rendering
            PageLoadStrategy = PageLoadStrategy.Eager
        };
        options.AddArgument("--headless=new");
        options.AddArgument("--no-sandbox");
        options.AddArgument("--disable-dev-shm-usage");
        return options;
    }
}
