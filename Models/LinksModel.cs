using selenium_web.Adapters.Interfaces;

namespace selenium_web.Models;

public class LinksModel(ILinksService linksService)
{
    public List<Link> GetLinks(string url)
    {
        return linksService.GetLinks(url);
    }

    public void GetLinksAsync(string url)
    {
        //view.NotifyLinksReady(linksService.GetLinksAsync(url));
    }
}