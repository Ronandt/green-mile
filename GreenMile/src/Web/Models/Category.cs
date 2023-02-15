using System.ComponentModel.DataAnnotations;

namespace Web.Models;

public class Category
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<FoodItem>? FoodItems { get; set; }
    public override string ToString()
    {
        return $"{Id} - {Name}";
    }


}