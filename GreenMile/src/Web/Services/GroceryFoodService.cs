using System;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

using Newtonsoft.Json;

using PexelsDotNetSDK.Api;

using Web.Data;
using Web.Models;
using Web.Utils;

namespace Web.Services
{
    public class GroceryFoodService: IGroceryFoodService
    {
        private static JsonSerializerOptions serializerOptions = new JsonSerializerOptions
        {
            MaxDepth = 10000,
            WriteIndented= true
            
        };
        private readonly DataContext _dataContext;
        private readonly PexelsClient _pexelsClient;
        private readonly OpenAIApiService _openAIApiService;
        private readonly CategoryService _categoryService;
        public GroceryFoodService(DataContext dataContext, PexelsClient pexelsClient, CategoryService categoryService, OpenAIApiService openAIApiService)
        {
            _dataContext = dataContext;
            _pexelsClient = pexelsClient;
            _categoryService = categoryService;
            _openAIApiService = openAIApiService;
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


        public async Task<string> ExportGroceryList(int householdId)
        {
            List<GroceryFoodItem> r = (await RetrieveFoodByHousehold(householdId)).Value?.ToList();
            r.ForEach(x => x.InBasket = false);
            using var memoryStream = new MemoryStream();
   
            await System.Text.Json.JsonSerializer.SerializeAsync(memoryStream,r, serializerOptions);
            var e =  Encoding.UTF8.GetString(memoryStream.ToArray());
            return e;
       
     
      

        }

        public async Task ImportGroceryList(int householdId, string json)
        {
            List<GroceryFoodItem> foodItems;
            try
            {
                List<GroceryFoodItem> why = JsonConvert.DeserializeObject<List<GroceryFoodItem>>(json);
                why.ForEach(x => x.Id = Guid.NewGuid().ToString()
);


                why.ForEach(x =>
                {
                    x.HouseholdId = householdId;
                });

                await _dataContext.AddRangeAsync(why);
                await _dataContext.SaveChangesAsync();
            } catch(Exception ex)
            {

            }
  
      

        }

        public async Task ImportGroceryListFromPlainText(int householdId, List<string> text)
        {
          
                List<string> categories = (await _categoryService.RetrieveCategories()).Select(x => x.ToString()).ToList();

                List<string> categoryValues = (await _openAIApiService.GenerateDavinciPrompt($"Categorise these ingredients with the following categoryids: {String.Join(",", categories)}:　{String.Join(",", text)}. Only output in this format: 5,4,2,1")).Choices.FirstOrDefault().Text.Split(",").ToList();

                List<GroceryFoodItem> objects = (await Task.WhenAll(text[0].Split("\n").Zip(categoryValues, async (x, y) =>

                    new GroceryFoodItem()
                    {
                        HouseholdId = householdId,
                        CarbonFootprint = 0,
                        Description = "No description",
                        ImageFilePath = (await _pexelsClient.SearchPhotosAsync(x, pageSize: 2)).photos[0].source.original,
                        CategoryId = Int32.Parse(y),
                        Name = x,
                        Quantity = 1

                    }

                ))).ToList();

                await _dataContext.GroceryFood.AddRangeAsync(objects);
                await _dataContext.SaveChangesAsync();
      

        }


     

    }
}
