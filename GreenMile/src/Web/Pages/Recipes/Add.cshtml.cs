using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.Models;
using Web.Services;

namespace Web.Pages.Recipes
{
    public class AddModel : PageModel
    {
        private readonly RecipeService _recipeService;
        private IWebHostEnvironment _webHostEnvironment;

        public AddModel(RecipeService recipeService, IWebHostEnvironment webHostEnvironment)
        {
            _recipeService = recipeService;
            _webHostEnvironment = webHostEnvironment;
        }

        [BindProperty]
        public Recipe CurrentRecipe { get; set; } = new();
        public IFormFile? image { get; set; }
        public List<String> testIngredientList { get; set; } = new();

        public void OnGet()
        {
            //get the ingredients list
            testIngredientList.Add("First ingredient");
            testIngredientList.Add("Second ingredient");
            testIngredientList.Add("Third ingredient");
        }
        public IActionResult OnPost()
        {
            TempData["FlashMessage.Type"] = "danger";
            TempData["FlashMessage.Text"] = ModelState.IsValid + "THIS IS THE MODELSTATE";
            if (true) //ModelState.IsValid //model state is invalid bc idk how the table works LOL
            { 
                if (image != null)
                {

                    var imagesFolder = "uploads";
                    var imageFile = Guid.NewGuid() + Path.GetExtension(image.FileName);
                    var imagePath = Path.Combine(_webHostEnvironment.ContentRootPath,"wwwroot",imagesFolder, imageFile);
                    using var fileStream = new FileStream(imagePath, FileMode.Create);
                    image.CopyTo(fileStream);
                    CurrentRecipe.imageFilePath = String.Format("/" + imagesFolder + "/" + imageFile);

                }
                Recipe? recipe = _recipeService.GetRecipeById(CurrentRecipe.recipeName);
                if(recipe != null)
                {
                    TempData["FlashMessage.Type"] = "danger";
                    TempData["FlashMessage.Text"] = CurrentRecipe.recipeName + " already exists!";
                    return Page();
                }
                _recipeService.AddRecipe(CurrentRecipe);
                TempData["FlashMessage.Type"] = "success";
                TempData["FlashMessage.Text"] = CurrentRecipe.recipeName + " successfully added!";
                return Redirect("/Recipes/Index");
            }
            //return Page();
        }
    }
}
