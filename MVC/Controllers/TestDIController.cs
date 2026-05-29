using Microsoft.AspNetCore.Mvc;
using MVC.Services;

namespace MVC.Controllers;

public class TestDIController : Controller
{
    private readonly ITransientService _transient;
    private readonly IScopedService _scoped;
    private readonly ISingletonService _singleton;
    private readonly ITestDIService _mainService;

    public TestDIController(
        ITransientService transient,
        IScopedService scoped,
        ISingletonService singleton,
        ITestDIService mainService)
    {
        _transient = transient;
        _scoped = scoped;
        _singleton = singleton;
        _mainService = mainService;
    }

    public IActionResult Index()
    {
        // Lấy Id từ MainService (Lần gọi 1)
        var mainIds = _mainService.GetServiceIds();

        // Gửi toàn bộ dữ liệu qua View để hiển thị và so sánh với Lần gọi 2 tại Controller
        ViewBag.Transient_Call1 = mainIds.transientId;
        ViewBag.Transient_Call2 = _transient.Id;

        ViewBag.Scoped_Call1 = mainIds.scopedId;
        ViewBag.Scoped_Call2 = _scoped.Id;

        ViewBag.Singleton_Call1 = mainIds.singletonId;
        ViewBag.Singleton_Call2 = _singleton.Id;

        return View();
    }
}
