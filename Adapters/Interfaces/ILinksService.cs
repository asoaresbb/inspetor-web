namespace selenium_web.Adapters.Interfaces;

public interface ILinksService
{
    Task<List<Link>> GetLinksAsync(string url);
    List<Link> GetLinks(string url);
}

public class Link
{
    public required string Url { get; init; }
    public required string Text { get; init; }
}