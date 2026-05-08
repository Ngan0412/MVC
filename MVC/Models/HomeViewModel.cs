namespace MVC.Models;

public class HomeViewModel
{
    public List<Category> Categories { get; set; } = new List<Category>();
    public List<Product> FeaturedProducts { get; set; } = new List<Product>();
}
