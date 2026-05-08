using Microsoft.AspNetCore.Mvc;
using MVC.Data;

namespace MVC.ViewComponents;

public class CategoryMenuViewComponent : ViewComponent
{
    private readonly MVCContext _context;
    public CategoryMenuViewComponent(MVCContext context)
    {
        _context = context;
    }
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var categories = _context.Category.OrderBy(c => c.Name).ToList();
        return View(categories);
    }
}
