using selenium_web.Adapters.Interfaces;

namespace selenium_web.Adapters;

public class SeleniumReadingModeAdapter(MyWebDriver driver) : IReadingModeService
{
    // Finds the main content: every <article> on the page (not counting articles nested in
    // another, such as comments), or else the first <main>, or else #content. Returns null
    // if there is none.
    //
    // For each one, returns the visible text of each text block, skipping blocks nested in
    // another block (e.g. a <p> inside an <li>) so the same text doesn't appear twice. If it
    // has no such blocks, returns its whole text as paragraphs. Articles are separated by
    // an "hr" block.
    private const string ExtractBlocksScript = """
        const blockTags = 'h1,h2,h3,h4,h5,h6,p,li,blockquote,pre';

        const articles = [...document.querySelectorAll('article')]
            .filter(article => !article.parentElement.closest('article'));
        const roots = articles.length > 0
            ? articles
            : [document.querySelector('main') ?? document.getElementById('content')].filter(Boolean);
        if (roots.length === 0) return null;

        const blocksOf = root => {
            const blocks = [...root.querySelectorAll(blockTags)]
                .filter(el => {
                    const outer = el.parentElement.closest(blockTags);
                    return !outer || !root.contains(outer);
                })
                .map(el => ({ tag: el.tagName.toLowerCase(), text: el.innerText.trim() }));
            const withText = blocks.length > 0
                ? blocks
                : root.innerText.split('\n').map(line => ({ tag: 'p', text: line.trim() }));
            return withText.filter(block => block.text !== '');
        };

        return roots
            .map(blocksOf)
            .filter(blocks => blocks.length > 0)
            .flatMap((blocks, index) => index === 0 ? blocks : [{ tag: 'hr', text: '' }, ...blocks]);
        """;

    public List<ReadingBlock> GetContentReadingMode(string url)
    {
        driver.NavigateAndWaitForPage(url);

        var blocks = driver.ExecuteScript(ExtractBlocksScript) as IEnumerable<object>
            ?? throw new NoMainContentException(url);

        return blocks
            .Cast<IDictionary<string, object>>()
            .Select(block => new ReadingBlock((string)block["tag"], (string)block["text"]))
            .ToList();
    }
}
