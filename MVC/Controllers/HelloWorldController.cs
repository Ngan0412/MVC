using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace MVC.Controllers;

public class HelloWorldController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
    public IActionResult Welcome(string name, int numTimes = 1)
    {
        ViewData["Message"] = "Hello " + HtmlEncoder.Default.Encode(name);
        ViewData["NumTimes"] = numTimes;
        return View();
    }
}
