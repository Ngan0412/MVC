using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
