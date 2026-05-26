using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers;

[Authorize(Policy = "MarketingManagerPolicy")]
public class VoucherController : Controller
{
    // URL: /Voucher/Create
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(string voucherCode, decimal discountAmount)
    {
        // Logic tạo mã giảm giá lưu vào Database...
        TempData["Success"] = $"Đã tạo thành công voucher {voucherCode}!";
        return RedirectToAction("Create");
    }
}
