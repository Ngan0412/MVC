using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MVC.Models;
using MVC.Services;
using System.Diagnostics;

namespace MVC.Controllers;

public class TestMemoryCacheController : Controller
{
    private readonly IMemoryCache _memoryCache;
    private  ProductService _productService = new();

    public TestMemoryCacheController(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public async Task<IActionResult> Index()
    {
        string cacheKey = "product_list_key";

        // Controller chỉ việc gọi hàm, truyền Key, thời gian và Hàm lấy DB gốc
        var products = await _memoryCache.GetOrCreateAsync(
            cacheKey,
            async entry => {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30); // Cache sẽ tự động hết hạn sau 30 giây
                entry.Priority = CacheItemPriority.Normal; // Đặt mức độ ưu tiên cho cache (tùy chọn)
                return await _productService.GetProductsFromDatabaseAsync(); // Hàm này sẽ chỉ được gọi khi cache hết hạn hoặc chưa tồn tại, giúp giảm tải cho Database
            }
        );

        return View(products);
    }

    // Viết thêm 1 hàm để Admin chủ động XÓA CACHE khi cần (ví dụ khi sửa giá sản phẩm)
    public IActionResult ClearCache()
    {
        _memoryCache.Remove("product_list_key");
        return RedirectToAction("Index");
    }
}

public class ProductMemory
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
public class ProductService
{
    public async Task<List<ProductMemory>> GetProductsFromDatabaseAsync()
    {
        // Giả lập Database xử lý nặng/chậm mất 3 giây mới xong
        await Task.Delay(3000);

        return new List<ProductMemory>
        {
            new ProductMemory { Id = 1, Name = "iPhone 15 Pro Max", Price = 1200 },
            new ProductMemory { Id = 2, Name = "Samsung Galaxy S24 Ultra", Price = 1100 },
            new ProductMemory { Id = 3, Name = "MacBook Pro M3", Price = 2000 }
        };
    }
}