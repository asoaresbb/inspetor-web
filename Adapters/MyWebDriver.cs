using OpenQA.Selenium.Chrome;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace selenium_web.Adapters;

public class MyWebDriver() : ChromeDriver(CreateDriverService(), CreateDefaultOptions()), IDisposable
{
    private bool _disposed;

    public new void Dispose()
    {
        if (!_disposed)
        {
            Quit();
            base.Dispose();
            _disposed = true;
        }
    }
    
    private static ChromeDriverService CreateDriverService()
    {
        // Automatically download the correct driver version
        new DriverManager().SetUpDriver(new ChromeConfig());
        return ChromeDriverService.CreateDefaultService();
    }

    private static ChromeOptions CreateDefaultOptions()
    {
        var options = new ChromeOptions();
        options.AddArgument("--headless=new");
        options.AddArgument("--no-sandbox");
        options.AddArgument("--disable-dev-shm-usage");
        return options;
    }
}