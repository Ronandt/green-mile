using System.ComponentModel.DataAnnotations;
using System.Text;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Newtonsoft.Json;

using Web.Models;
using Web.Services;

namespace Web.Pages.FoodTracker
{
    public class ExternalAddModel : PageModel
    {
        private readonly FoodItemService _fooditemService;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _http;
        private readonly IHouseholdService _householdService;
        private readonly FoodCategoryService _foodcategoryService;
        private IWebHostEnvironment _environment;

        public ExternalAddModel(IHttpContextAccessor http, IHouseholdService householdService, FoodItemService fooditemService, IWebHostEnvironment environment, FoodCategoryService foodCategoryService, UserManager<User> userManager)
        {
            _fooditemService = fooditemService;
            _environment = environment;
            _foodcategoryService = foodCategoryService;
            _http = http;
            _householdService = householdService;
            _userManager = userManager;
        }
        public static List<Category> Categories { get; set; } = new();

        [BindProperty, Required]
        public string Category { get; set; }

        [BindProperty, Required]
        public string Name { get; set; }

        [BindProperty, Required, MinLength(0), MaxLength(100)]
        public string Description { get; set; }

        [BindProperty, Required, Range(1, 100, ErrorMessage = " Choose between 1 - 100")]
        public int Quantity { get; set; }

        [BindProperty, Required]
        public DateTime ExpiryDate { get; set; }

        [BindProperty, Required]
        public string ImageFilePath { get; set; }

        [BindProperty, Required]
        public double Weight { get; set; }

        [BindProperty, Required]
        public double CarbonFoodprint { get; set; }


        //public string householdName { get; set; }


        public async Task OnGetAsync(string name,string image,string weight)
        {
            //Categories = _foodcategoryService.GetAll();
            Categories.Add( new Category { Id = 1, Name = "Fruit", Description = "healthy" });
            Categories.Add(new Category { Id = 2, Name = "Meat", Description = "yummy" });
            Categories.Add(new Category { Id = 3, Name = "Vegetable", Description = "healthy" });
            Categories.Add(new Category { Id = 4, Name = "Dairy", Description = "tasty" });
                 
                 
            try
            {
                //string text = name;
                //string apiKey = "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";
               
                //Console.WriteLine("done");
                //Name = TranslateTextToEnglish(text,apiKey);
                Console.WriteLine(Name);

            }
            catch
            {
                Name = name;
            }
           
            ImageFilePath = image;
            ExpiryDate = DateTime.Now;
            try
            {
                Weight = double.Parse(weight);
            }
            catch
            {
                Weight = 0.5;
            }
            
           
            //var user = await _userManager.GetUserAsync(HttpContext.User);
            //var household = ( await _householdService.RetrieveHouseholdDetails(user.HouseholdId ?? -1)).Value;
            //householdName = household.Name;

        }
        //public string TranslateTextToEnglish(string text,string apiKey)
        //{
        //    using (var client = new HttpClient())
        //    {
        //        Console.WriteLine("done");
        //        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + apiKey);
        //        var request = new
        //        {
        //            prompt = text,
        //            max_tokens = 1024,
        //            temperature = 0.5
        //        };
                
        //        var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
        //        var response = client.PostAsync("https://api.openai.com/v1/engines/text-davinci-002/jobs", content).Result;
        //        Console.WriteLine(response);
        //        response.EnsureSuccessStatusCode();
                
        //        var responseString = response.Content.ReadAsStringAsync().Result;
        //        dynamic responseJson = JsonConvert.DeserializeObject(responseString);
        //        Console.WriteLine("done");
        //        return responseJson.choices[0].text;
        //    }
        //}


        public async Task<IActionResult> OnPostAsync()
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }



            if (ModelState.IsValid)
            {
                if (ImageFilePath != null)
                {
                    if (ImageFilePath.Length > 2 * 1024 * 1024)
                    {
                        ModelState.AddModelError("Upload", "File size cannot exceed 2MB.");
                        return Page();
                    }
                    //byte[] bytes = Encoding.UTF8.GetBytes(ImageFilePath);
                    //var memoryStream = new MemoryStream(bytes);

                    //var formFile = new FormFile(memoryStream, 0, bytes.Length, ImageFilePath, ImageFilePath);

                    //var uploadsFolder = "uploads";
                    //var imageFile = Guid.NewGuid() + Path.GetFileName(ImageFilePath);
                    //var imagePath = Path.Combine(_environment.ContentRootPath, "wwwroot", uploadsFolder, imageFile);
                    //using var fileStream = new FileStream(imagePath, FileMode.Create);
                    //await formFile.CopyToAsync(fileStream);
                    //var ImageURL = string.Format("/{0}/{1}", uploadsFolder, imageFile);

                    var user = await _userManager.GetUserAsync(HttpContext.User);
                    var household = (await _householdService.RetrieveHouseholdDetails(user.HouseholdId ?? -1)).Value;
                    //var householdName = _http.HttpContext?.Session.GetString(SessionVariable.HousholdName);
                    //var household = (await _householdService.RetrieveHouseholdDetailsByName(householdName)).Value;
                    //var category = _foodcategoryService.GetCategoryByName(Category);
                    var newfood = new FoodItem()
                    {
                        Household = household,
                        Name = Name,
                        Description = Description,
                        Quantity = Quantity,
                        ExpiryDate = ExpiryDate,
                        ImageFilePath = ImageFilePath,
                        Category = Category,
                        CarbonFootprint = 1.1,
                        Weight = 1.1,
                        IsCustom = false,
                        Status = true
                    };

                    var newcat = new Category()
                    {
                        Name = "Fruit",
                        Description = "Healthy for the body"
                    };

                    _foodcategoryService.AddCategory(newcat);
                    _fooditemService.AddFoodItem(newfood);
                    TempData["success"] = "Food added successgfully!";
                    return Redirect("/FoodTracker");
                }



            }
            return Page();



        }
    
}
}
