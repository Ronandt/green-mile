using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Web.Models;
using Web.Services;

namespace Web.Pages.Recipes
{
    public class DetailsModel : PageModel
    {
        private readonly RecipeService _recipeService;
        private readonly FoodItemService _foodItemService;

        public DetailsModel(RecipeService recipeService, FoodItemService foodItemService)
        {
            _recipeService = recipeService;
            _foodItemService = foodItemService;
        }
        [BindProperty]
        public Recipe currentRecipe { get; set; } = new();
        public List<string> ingredients { get; set; } = new();
        //public List<FoodItem> allFoodItems { get; set; } = new();
        public void OnGet(int id)
        {
            currentRecipe = _recipeService.GetRecipeById(id);
            
            //allFoodItems = _foodItemService.GetAllForRecipe();
            List<string> result = currentRecipe.ingredients?.Split(',').ToList();
            foreach(var ingredientID in result)
            {
                //int index = Int32.Parse(ingredientID);
                FoodItem foodItem = _foodItemService.GetFoodItemById(Int32.Parse(ingredientID)).Result;
                ingredients.Add(foodItem.Name);
            }

        }
    }
}
