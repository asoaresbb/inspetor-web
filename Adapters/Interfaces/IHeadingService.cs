namespace selenium_web.Adapters.Interfaces;

public interface IHeadingsService
{
    List<Heading> GetHeadings(string url);
}

public class Heading
{
    public required int Level { get; set; }    // e.g. 1 for <h1>, 2 for <h2>
    public required string Text { get; set; }  // empty when the heading has no text at all
    public required HeadingVisibility Visibility { get; set; }
}

public enum HeadingVisibility
{
    Visible,
    Hidden,   // has text, but it isn't shown (screen-reader-only, closed menu, etc.)
    Empty     // no text in the HTML at all
}
