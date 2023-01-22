using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Web.Models;

using Web.Services;

namespace Web.Pages.FoodSharing
{
    [Authorize]
    public class EditDonationInfoModel : PageModel
    {
        private readonly CustomFoodService _customFoodService;
        private readonly DonationService _donationService;
        private readonly IWebHostEnvironment _environment;

        public EditDonationInfoModel(
            CustomFoodService customFoodService,
            DonationService donationService,
            IWebHostEnvironment environment
        )
        {
            _donationService = donationService;
            _customFoodService = customFoodService;
            _environment = environment;
        }

        [BindProperty]
        public int CustomFoodId { get; set; }

        [BindProperty]
        public string? Name { get; set; }

        [BindProperty]
        public string? Description { get; set; }

        [BindProperty]
        public DateTime? ExpiryDate { get; set; }

        [BindProperty]
        public IFormFile? Upload { get; set; }

        public string Image { get; set; }

        public IActionResult OnGet(int id)
        {
            var donation = _donationService.GetDonationById(id);

            if (donation is null)
                return NotFound();

            CustomFoodId = donation.CustomFood.Id;

            Image = donation.CustomFood.Image;

            return Page();

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var customFood = await _customFoodService.GetCustomFoodById(CustomFoodId);

            customFood!.Name = Name ?? customFood.Name;
            customFood.Description = Description ?? customFood.Description;
            customFood.ExpiryDate = ExpiryDate ?? customFood.ExpiryDate;
            
            if (Upload is not null)
            {
                var uploadFolder = "Uploads";
                var imageFile = Guid.NewGuid() + Path.GetExtension(Upload.FileName);
                var imagePath = Path.Combine(_environment.ContentRootPath, "wwwroot", uploadFolder, imageFile);
                using var fileStream = new FileStream(imagePath, FileMode.Create);
                await Upload.CopyToAsync(fileStream);
                customFood.Image = string.Format("/{0}/{1}", uploadFolder, imageFile);
            }

            await _customFoodService.Update(customFood);

            return Redirect("/FoodSharing");
        }
    }
}
