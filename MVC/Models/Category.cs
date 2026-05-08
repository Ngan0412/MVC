using System.ComponentModel.DataAnnotations;

namespace MVC.Models;

public class Category
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    [Display(Name = "Category Name")]
    public string? Name { get; set; }

    [Display(Name = "Display Order")]
    [Range(1, 100, ErrorMessage = "Display order must be between 1 and 100")]
    public int DisplayOrder { get; set; }

    // Navigation property: One category can have many products
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}