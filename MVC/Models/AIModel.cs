using System.ComponentModel.DataAnnotations;

namespace MVC.Models;

public class AIModel
{
    [Key]
    public int Id { get; set; }
    // Properties
    public string? ModelName { get; set; }
    public string? EmailRegisted { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public int TotalQuantity { get; set; }
    public int RemainingQuantity { get; set; }

    // List to manage associated users
    public virtual ICollection<User> Users { get; set; } = new List<User>();

}