using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC.Models;

public class User
{
    [Key]
    public int Id { get; set; }
    // Properties
    public string? Name { get; set; }
    public string? FacebookLink { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Zalo { get; set; }
    public string? Description { get; set; }
    public DateTime PurchaseDate { get; set; }
    public DateTime ExpiryDate { get; set; }

    [Required]
    [Display(Name = "AIModel")]
    public int AIModelId { get; set; }

    // Navigation property to the Category object
    [ForeignKey("AIModelId")]
    public virtual AIModel? AIModel { get; set; }


}