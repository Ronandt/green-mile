using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Web.Models;

using Web.Services;
using Microsoft.EntityFrameworkCore;

namespace Web.Pages.FoodSharing
{
    [Authorize]
    public class AddDonationModel : PageModel
    {
        private readonly DonationService _donationService;
        private readonly CustomFoodService _customFoodService;
        private readonly UserManager<User> _userManager;
        private IWebHostEnvironment _environment;
        public AddDonationModel(DonationService donationService,
            CustomFoodService customFoodService,
            UserManager<User> userManager,
            IWebHostEnvironment environment
            )
        {
            _donationService = donationService;
            _customFoodService = customFoodService;
            _userManager = userManager;
            _environment = environment;
        }

        [BindProperty, Required, MinLength(1), MaxLength(20)]
        public string Name { get; set; }
        [BindProperty, Required, MinLength(0), MaxLength(100)]
        public string Description { get; set; }
        
        [BindProperty, Required]
        public DateTime ExpiryDate { get; set; }

        [BindProperty, Required]
        public IFormFile? Upload { get; set; }

        public Donation MyDonation { get; set; } = new();

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                if (Upload != null)
                {
                    if (Upload.Length > 2 * 1024 * 1024)
                    {
                        ModelState.AddModelError("Upload", "File size cannot exceed 2MB.");
                        return Page();
                    }

                    // Get User
                    var userId = (await _userManager.GetUserAsync(HttpContext.User)).Id;
                    var user = await _userManager.Users.FirstAsync(u => u.Id == userId);

                    // Food Item Input
                    var newFood = new CustomFood()
                    {
                      
                        Name = Name,
                        Description = Description,
                        ExpiryDate = ExpiryDate,
                        
                    };

                    // Uploads Destination
                    var uploadsFolder = "uploads";
                    var imageFile = Guid.NewGuid() + Path.GetExtension(Upload.FileName);
                    var imagePath = Path.Combine(_environment.ContentRootPath, "wwwroot", uploadsFolder, imageFile);
                    using var fileStream = new FileStream(imagePath, FileMode.Create);
                    await Upload.CopyToAsync(fileStream);

                    newFood.Image = string.Format("/{0}/{1}", uploadsFolder, imageFile);

                    // Add food item to DB
                    _customFoodService.AddCustomFood(newFood);

                    // Donation 
                    var donation = new Donation()
                    {
                        User = user,
                        CustomFood = newFood,
                        Status = DonationStatus.ACTIVE,
                    };

                    _donationService.AddDonation(donation);

                    TempData["FlashMessage.Type"] = "success";
                    TempData["FlashMessage.Text"] = string.Format("Donation Offer {0} is created Of Food", Name);
                    return Redirect("/FoodSharing/Index");
                }
            }
            return Page();
        }
    }
}