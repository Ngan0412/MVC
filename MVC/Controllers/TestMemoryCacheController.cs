using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MVC.Models;
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

        // Dùng Stopwatch để đo tốc độ phản hồi hiển thị ra View
        var stopwatch = Stopwatch.StartNew();
        string dataSource;

        // BƯỚC CHÍNH: Thử lấy dữ liệu từ Cache ra trước
        // Nếu tìm thấy (true), dữ liệu sẽ nạp thẳng vào biến 'products'
        if (!_memoryCache.TryGetValue(cacheKey, out List<ProductMemory>? products))
        {
            // NẾU CACHE TRỐNG (CACHE MISS):
            dataSource = "Database gốc (Mất 3 giây xử lý)";

            // 1. Đi lấy dữ liệu gốc từ Service/DB
            products = await _productService.GetProductsFromDatabaseAsync();

            // 2. Thiết lập cấu hình (Thời gian sống) cho Cache này
            var cacheOptions = new MemoryCacheEntryOptions()
                // Hết hạn tuyệt đối sau 1 phút kể từ khi tạo (bắt buộc xóa để làm mới)
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(1))
                // Hết hạn trượt: Nếu trong vòng 20 giây không có ai xem trang, tự động xóa sớm
                .SetSlidingExpiration(TimeSpan.FromSeconds(20))
                // Độ ưu tiên giữ lại trong RAM nếu server bị nghẽn/thiếu RAM
                .SetPriority(CacheItemPriority.High);

            // 3. Nạp dữ liệu vào lại Cache để lần sau dùng
            _memoryCache.Set(cacheKey, products, cacheOptions);
        }
        else
        {
            // NẾU TÌM THẤY TRONG CACHE (CACHE HIT):
            dataSource = "RAM - In-Memory Cache (Tốc độ ánh sáng)";
        }

        stopwatch.Stop();

        // Truyền các thông tin đo đạc ra giao diện xem chơi
        ViewBag.ExecutionTime = stopwatch.ElapsedMilliseconds;
        ViewBag.DataSource = dataSource;

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