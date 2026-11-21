namespace selenium_web.Adapters.Interfaces;

public interface IHeadingsService
{
    List<Heading> GetHeadings(string url);
}

public class Heading
{
    public required int Level { get; set; }    // Ex: 1 para <h1>, 2 para <h2>, etc.
    public required string Text { get; set; }  // texto do heading
}