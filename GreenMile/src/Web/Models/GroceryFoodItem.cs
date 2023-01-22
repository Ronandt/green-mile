namespace Web.Models
{
    public class GroceryFoodItem
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public int Quantity { get; set; }
        public string? ExtraNote { get; set; }
        public bool InBasket { get; set; }
   

        public Household? Household { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
    
        public double? CarbonFootprint { get; set; }
   
        public string? ImageFilePath { get; set; } = string.Empty;
        public Category? Category { get; set; }
    }
}
