using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Web.Models;
using Web.Services;
using Web.UiState;

namespace Web.Pages.GroceryList
{
    public class AddModel : PageModel
    {
        [BindProperty,Required]
        public AddGroceryUiState GroceryUiState { get; set; }

        public List<Category> Categories { get; set; }
        private readonly IImageService _imageService;
        private readonly IHouseholdService _householdService;
        private readonly IGroceryFoodService _foodService;
        private readonly UserManager<User> _userManager;
        private readonly CategoryService _categoryService;
        private readonly OpenAIApiService _openAIApiService;

        public AddModel(IGroceryFoodService foodService, IImageService imageService, IHouseholdService householdService, UserManager<User> userManager, CategoryService categoryService, OpenAIApiService openAIApiService)
        {
            _foodService = foodService;
            _imageService = imageService;
            _householdService = householdService;
            _userManager = userManager;
            _categoryService = categoryService;
            _openAIApiService= openAIApiService;        
        }

 

        public async Task OnGet()
        {
            Categories = await _categoryService.RetrieveCategories();
        }

        public async Task<IActionResult> OnPost()
        {
            var user = await _userManager.GetUserAsync(User);

            var response = await _openAIApiService.ClassifyText(new List<string>()
            {
                GroceryUiState?.Description,
                GroceryUiState?.Name
            });

            if(!response.Successful)
            {
                TempData["error"] = "Something went wrong, try again";
                return Redirect("/grocerylist/add");
            }


            // List<string> vals = response.Results.Select(x => x.Categories.GetType().GetProperties().ToDictionary(a => a.Name, a => (bool)(a.GetValue(x) ?? false))).SelectMany(dict => dict).Where(kvp => kvp.Value).ToDictionary(kvp => kvp.Key, kvp => kvp.Value, StringComparer.OrdinalIgnoreCase).Select(x => x.Key).ToList();
            List<string> vals = response.Results
             .Where(x => x.Flagged)
             .SelectMany(x => x.Categories.GetType().GetProperties(), (x, p) => (Property: p, Value: p.GetValue(x.Categories)))
             .Where(tuple => (bool)(tuple.Value ?? false))
             .Select(tuple => tuple.Property.Name)
             .Distinct(StringComparer.OrdinalIgnoreCase)
             .ToList();

            if (!(vals.DefaultIfEmpty() is null))
            {
                TempData["error"] = $"You can't add as your content contains the following: {string.Join(" ,", vals)}";
                return Redirect("/grocerylist/add");
            }


            
       





            if (!(user is null))
            {
                var item = new GroceryFoodItem()
                {
                    Id = Guid.NewGuid().ToString(),
                Household = user.Household,
                HouseholdId = user.HouseholdId?? 0,
                    Name = GroceryUiState.Name,
                    Quantity = GroceryUiState.Quantity,
                    ImageFilePath = await _imageService.StoreImage(GroceryUiState.Image),
                    CategoryId = GroceryUiState.Category,
                    Description = GroceryUiState.Description
                    
                };
                await _foodService.Add(item);
       

      
                TempData["success"] = "Added!";
                return Redirect("/grocerylist/groceries");


            }
            return Page();
        }
    }
}
