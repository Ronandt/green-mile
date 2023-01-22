using System;
using System.ComponentModel.DataAnnotations;

namespace Web.Models;

public class CustomFood
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string? Image { get; set; }

    public DateTime ExpiryDate { get; set; } 

    // public ICollection<Category> Categories { get; set; } = new List<Category>();

}
