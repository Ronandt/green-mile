using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using PexelsDotNetSDK.Api;

using Web.Models;
using Web.Services;
using Web.UiState;

namespace Web.Pages.GroceryList
{
    [Authorize]
    public class GroceriesModel : PageModel
    {
        private readonly IGroceryFoodService _groceryFoodService;
        private readonly UserManager<User> _userManager;
        private readonly GoogleAIService _googleAIService;
        [BindProperty]
        public GroceryListUiState GroceryListUiState { get; set; } = new GroceryListUiState();

        public GroceriesModel(IGroceryFoodService groceryFoodService, UserManager<User> userManager, GoogleAIService googleAIService)
        {
            _groceryFoodService = groceryFoodService;

            _userManager = userManager;
            _googleAIService = googleAIService;
            
        }

        public async Task OnGet()
        {
           int household =  (int)(await _userManager.GetUserAsync(User)).HouseholdId;
           await _groceryFoodService.RetrieveFoodByHousehold(household);

        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            await _groceryFoodService.Delete(GroceryListUiState.DeleteId);
            return Redirect("/grocerylist/groceries");
           

        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (GroceryListUiState.JsonFile is not null)
                {
                    using var streamReader = new StreamReader(GroceryListUiState.JsonFile.OpenReadStream());
                    string jsonString = await streamReader.ReadToEndAsync();
                    await _groceryFoodService.ImportGroceryList((int)(await _userManager.GetUserAsync(User)).HouseholdId, jsonString);

                }
                else if (GroceryListUiState.ImageFile is not null)
                {

                    List<string> items = await _googleAIService.DetectText(await _googleAIService.ConvertFileStreamToImage(GroceryListUiState.ImageFile));
                    await _groceryFoodService.ImportGroceryListFromPlainText((int)(await _userManager.GetUserAsync(User)).HouseholdId, items);

                }
                TempData["success"] = "The list has been successfully imported";
             

            } catch(Exception ex) { TempData["error"] = "The import went wrong, try again."; }

            return Redirect("/grocerylist/groceries");
        }

        
    }
}
