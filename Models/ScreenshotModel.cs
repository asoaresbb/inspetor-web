using selenium_web.Adapters.Interfaces;

namespace selenium_web.Models
{
    public class ScreenshotModel(IScreenshotService screenshotService)
    {
        public string CaptureScreenshot(string url)
        {
            return screenshotService.CaptureScreenshot(url);
        }
    }
}