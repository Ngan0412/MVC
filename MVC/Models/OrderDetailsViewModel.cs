namespace MVC.Models;

public class OrderDetailsViewModel
{
    public int OrderId { get; set; }
    public string OrderCode { get; set; }
    public DateTime OrderDate { get; set; }
    public string Status { get; set; } // Chờ duyệt, Đang giao, Đã giao, Hủy
    public decimal ShippingFee { get; set; }
    public decimal TotalAmount { get; set; }

    // Thông tin khách hàng & Giao hàng
    public string CustomerName { get; set; }
    public string Phone { get; set; }
    public string ShippingAddress { get; set; }
    public string PaymentMethod { get; set; }

    // Danh sách sản phẩm trong đơn hàng
    public List<OrderItemViewModel> OrderItems { get; set; }
}

public class OrderItemViewModel
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public string ProductImage { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Total => Quantity * Price;
}