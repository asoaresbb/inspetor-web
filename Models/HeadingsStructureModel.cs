using selenium_web.Adapters.Interfaces;

namespace selenium_web.Models
{
    public class HeadingsStructureModel(IHeadingsService headingsService)
    {
        public List<Heading> GetHeadings(string url)
        {
            return headingsService.GetHeadings(url);
        }
    }
}
