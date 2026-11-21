using selenium_web.Adapters.Interfaces;

namespace selenium_web.Models
{
    public class ReadingModeModel(IReadingModeService readingModeService)
    {
        public string GetContentReadingMode(string url)
        {
            return readingModeService.GetContentReadingMode(url);
        }
    }
}