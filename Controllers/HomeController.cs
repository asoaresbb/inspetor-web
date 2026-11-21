using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using selenium_web.Adapters.Interfaces;
using selenium_web.Models;

namespace selenium_web.Controllers;

public class HomeController(
    ScreenshotModel screenshotModel,
    ReadingModeModel readingModeModel,
    HeadingsStructureModel headingsStructureModel,
    LinksModel linksModel
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
                catch (Exception)
                {
                    ViewData["Erro"] = "Não foi possível obter screenshot";
                }

                break;

            case "readingmode":
                try
                {
                    ViewData["ReadData"] = readingModeModel.GetContentReadingMode(url);
                }
                catch (Exception)
                {
                    ViewData["Erro"] = "Não foi possível realizar modo de leitura";
                }

                break;
            case "headingsStructure":
                try
                {
                    ViewData["Headings"] = headingsStructureModel.GetHeadings(url);
                }
                catch (Exception)
                {
                    ViewData["Erro"] = "Não foi possível encontrar headings";
                }

                break;
            case "links":
                try
                {
                    ViewData["Links"] = linksModel.GetLinks(url);
                }
                catch (Exception)
                {
                    ViewData["Erro"] = "Não foi possível obter links";
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