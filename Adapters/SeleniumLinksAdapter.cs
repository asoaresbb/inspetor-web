using OpenQA.Selenium;
using selenium_web.Adapters.Interfaces;

namespace selenium_web.Adapters;

public class SeleniumLinksAdapter(MyWebDriver driver) : ILinksService
{
    public List<Link> GetLinks(string url)
    {
        driver.NavigateAndWaitForPage(url);
        var pageHost = HostOf(new Uri(driver.Url));

        return driver
            .FindElements(By.TagName("a"))
            .Select(element => new { Element = element, Url = element.GetAttribute("href") })
            .Where(link => IsExternal(link.Url, pageHost))
            .Select(link => new Link
            {
                Url = link.Url,
                Text = GetText(link.Element, link.Url)
            })
            .ToList();
    }

    public async Task<List<Link>> GetLinksAsync(string url)
    {
        return await Task.Run(() => GetLinks(url));
    }

    // A web link to another site. Selenium returns href already resolved, so relative
    // links and in-page anchors come back with the page's own host and are left out.
    private static bool IsExternal(string? url, string pageHost) =>
        Uri.TryCreate(url, UriKind.Absolute, out var uri)
        && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps)
        && HostOf(uri) != pageHost;

    // example.com and www.example.com are the same site
    private static string HostOf(Uri uri) =>
        uri.Host.StartsWith("www.") ? uri.Host[4..] : uri.Host;

    // Element.Text only returns visible text, so icon-only links and links inside
    // closed menus come back empty. Fall back to other descriptions, then the URL.
    private static string GetText(IWebElement element, string url)
    {
        // Each call is a round trip to the browser, so only ask for the next one if needed
        Func<string?>[] candidates =
        [
            () => element.Text,
            () => element.GetAttribute("aria-label"),
            () => element.GetAttribute("title"),
            () => element.GetAttribute("textContent")
        ];

        return candidates
            .Select(getText => getText()?.Trim())
            .FirstOrDefault(text => !string.IsNullOrEmpty(text))
            ?? url;
    }
}
