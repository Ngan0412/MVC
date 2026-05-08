using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC.Models;

public class Product
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string? Name { get; set; }

    [Required]
    public string? Description { get; set; }

    [Required]
    [Range(0.01, 10000)]
    [Column(TypeName = "decimal(18,2)")] // Essential for SQL money precision
    public decimal Price { get; set; }

    public string? ImageUrl { get; set; }

    public bool IsFeatured { get; set; } = false;

    [Required]
    [Display(Name = "Category")]
    public int CategoryId { get; set; }

    // Navigation property to the Category object
    [ForeignKey("CategoryId")]
    public virtual Category? Category { get; set; }
}