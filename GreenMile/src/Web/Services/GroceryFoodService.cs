using Microsoft.EntityFrameworkCore;

using Web.Data;
using Web.Models;
using Web.Utils;

namespace Web.Services
{
    public class GroceryFoodService: IGroceryFoodService
    {
        private readonly DataContext _dataContext;
        public GroceryFoodService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Boolean> Add(GroceryFoodItem groceryItem)
        {
            if(groceryItem is null)
            {
                throw new ArgumentNullException(nameof(groceryItem));
            }

           await _dataContext.GroceryFood.AddAsync(groceryItem);
            await _dataContext.SaveChangesAsync();
            return true;
        }

        public async Task ChangeQuantity(GroceryFoodItem groceryItem, int quantity)
        {
            int resultingQuantity = groceryItem.Quantity + quantity;
            if (resultingQuantity <= 0)
            {
                resultingQuantity = groceryItem.Quantity;
            }
            groceryItem.Quantity = resultingQuantity;

           _dataContext.Update(groceryItem);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<GroceryFoodItem> RetrieveGroceryItem(string id)
        {
            return await _dataContext.GroceryFood.FindAsync(id);
        }

       


        public async Task Delete(string id)
        {
            if(id is null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            
            _dataContext.GroceryFood.Remove(await _dataContext.GroceryFood.FindAsync(id));
            await _dataContext.SaveChangesAsync();
        }

        public async Task<Result<ICollection<GroceryFoodItem>>> RetrieveFoodByHousehold(int householdId)
        {
            var household =await _dataContext.Household.Where(h => h.HouseholdId == householdId).Include(x => x.GroceryItems).FirstOrDefaultAsync();
            return Result<ICollection<GroceryFoodItem>>.Success("Retrieved successfully", household.GroceryItems);
        }
     

    }
}
