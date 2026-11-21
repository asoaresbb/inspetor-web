using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using selenium_web.Adapters.Interfaces;
using selenium_web.Models;

namespace selenium_web.Controllers;

public class HomeController(
    ScreenshotModel screenshotModel,
    ReadingModeModel readingModeModel,
    HeadingsStructureModel headingsStructureModel,
    LinksModel linksModel,
    ILogger<HomeController> logger
)
    : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult AboutUs()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpPost]
    public ActionResult ProcessAction(string url, string actionButton)
    {
        // TODO: dar erro se URL inválido
        ViewData["url"] = url;
        switch (actionButton)
        {
            case "screenshot":
                try
                {
                    ViewData["ImagemBase64"] = screenshotModel.CaptureScreenshot(url);
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "Screenshot failed for {Url}", url);
                    ViewData["Erro"] = "Error capturing screenshot, probably the URL does not exist.";
                }

                break;

            case "readingmode":
                try
                {
                    ViewData["ReadData"] = readingModeModel.GetContentReadingMode(url);
                }
                catch (NoMainContentException)
                {
                    ViewData["Erro"] = "No article, main or #content found on this page.";
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "Reading mode failed for {Url}", url);
                    ViewData["Erro"] = "Error loading reading mode, probably the URL does not exist.";
                }

                break;
            case "headingsStructure":
                try
                {
                    ViewData["Headings"] = headingsStructureModel.GetHeadings(url);
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "Headings failed for {Url}", url);
                    ViewData["Erro"] = "Error finding headings, probably the URL does not exist.";
                }

                break;
            case "links":
                try
                {
                    ViewData["Links"] = linksModel.GetLinks(url);
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "Links failed for {Url}", url);
                    ViewData["Erro"] = "Error getting links, probably the URL does not exist.";
                }

                break;
        }

        return View("Index");
    }

    public void NotifyLinksReady(Task<List<Link>> getLinksAsync)
    {
        // TODO: notificar o browser que os links estão prontos. desta forma cumprir o MVC corretamente

        throw new NotImplementedException();
    }
}