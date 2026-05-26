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
            var OrdreDetailsViewModel = new Models.OrderDetailsViewModel
            {
                OrderId = 123,
                OrderCode = "ORD20240601",
                OrderDate = DateTime.Now,
                Status = "Chờ duyệt",
                ShippingFee = 15000,
                TotalAmount = 500000,
                CustomerName = "Nguyễn Văn A",
                Phone = "0123456789",
                ShippingAddress = "123 Đường ABC, Phường XYZ, Quận 1, TP.HCM",
                PaymentMethod = "Thanh toán khi nhận hàng",
                OrderItems = new List<Models.OrderItemViewModel>
                {
                    new Models.OrderItemViewModel
                    {
                        ProductId = 1,
                        ProductName = "Sản phẩm 1",
                        ProductImage = "/images/product1.jpg",
                        Quantity = 2,
                        Price = 100000
                    },
                    new Models.OrderItemViewModel
                    {
                        ProductId = 2,
                        ProductName = "Sản phẩm 2",
                        ProductImage = "/images/product2.jpg",
                        Quantity = 1,
                        Price = 300000
                    }
                }
            };
            return View(OrdreDetailsViewModel);
        }
    }
}
