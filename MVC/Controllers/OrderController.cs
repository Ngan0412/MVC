using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers;

public class OrderController : Controller
{
    public IActionResult DownloadInvoice(int orderId)
    {
        // 1. Giả lập tìm đường dẫn file hóa đơn vật lý lưu trên Server 
        // Thư mục thực tế thường nằm trong wwwroot hoặc một folder bảo mật riêng
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "invoices", $"invoice_{orderId}.pdf");

        // 2. Kiểm tra xem file có tồn tại thật không
        if (!System.IO.File.Exists(filePath))
        {
            return NotFound("Hóa đơn không tồn tại hoặc đang được xử lý.");
        }

        // 3. Đọc file thành mảng byte dữ liệu
        byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

        // 4. Tên file khi tải về máy khách hàng sẽ hiển thị
        string downloadName = $"HoaDon_DonHang_{orderId}.pdf";

        // 5. Trả về file, định dạng tệp là application/pdf
        return File(fileBytes, "application/pdf", downloadName);
    }
}
