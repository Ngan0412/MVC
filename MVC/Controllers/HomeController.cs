using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Data;
using MVC.Models;
using System.Diagnostics;

namespace MVC.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly MVCContext _context;

    public HomeController(ILogger<HomeController> logger, MVCContext context)
    {
        _logger = logger;
        _context = context;
    }


    public IActionResult Index()
    {
        var categories = _context.Category.ToList();
        var featuredProducts = _context.Product.Where(p => p.IsFeatured).ToList();
        HomeViewModel viewModel = new HomeViewModel
        {
            Categories = categories,
            FeaturedProducts = featuredProducts
        };
        return View(viewModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
