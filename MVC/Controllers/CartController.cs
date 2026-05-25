using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class CartController : Controller
    {
        [HttpPost]
        public IActionResult AddToCart(int productId)
        {
            // 1. Giả lập logic thêm sản phẩm vào database/session của giỏ hàng...
            // (Ví dụ sau khi thêm, tổng số sản phẩm trong giỏ hiện tại là 5 món)
            int currentCartCount = 5;

            // 2. Trả về trạng thái thành công kèm số lượng mới dưới dạng JSON
            return Json(new
            {
                success = true,
                message = "Đã thêm sản phẩm vào giỏ hàng thành công!",
                cartCount = currentCartCount
            });
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
