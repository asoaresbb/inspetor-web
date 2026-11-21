namespace selenium_web.Adapters.Interfaces;

public interface IReadingModeService
{
    /// <exception cref="NoMainContentException">The page has no article, main or #content element.</exception>
    List<ReadingBlock> GetContentReadingMode(string url);
}

/// <summary>A block of text from the main content, e.g. a heading or a paragraph.</summary>
/// <param name="Tag">The HTML tag it came from: h1-h6, p, li, blockquote or pre; or "hr" between two articles.</param>
public record ReadingBlock(string Tag, string Text);

public class NoMainContentException(string url)
    : Exception($"No article, main or #content element found on {url}");
