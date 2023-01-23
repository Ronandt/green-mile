using Web.Data;
using Web.Utils;
using Web.Models;
namespace Web.Services
{
    public interface IGroceryFoodService
    {


        public Task Add(GroceryFoodItem groceryItem);

        public Task ChangeQuantity(GroceryFoodItem groceryItem, int quantity);




        public Task Delete(string id);



        public  Task<Result<ICollection<GroceryFoodItem>>> RetrieveFoodByHousehold(int householdId);
     
    }
}
