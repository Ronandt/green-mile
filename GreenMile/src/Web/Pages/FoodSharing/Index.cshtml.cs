using Microsoft.AspNetCore.Identity;
using System.Xml.Linq;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Web.Models;
using Web.Services;
using Microsoft.EntityFrameworkCore;

namespace Web.Pages.FoodSharing
{
    public class IndexModel : PageModel
    {
        private readonly DonationService _donationService;
        private readonly DonationRequestService _donationRequestService;
        private readonly UserManager<User> _userManager;

        public IndexModel(DonationService donationService, DonationRequestService donationRequestService, UserManager<User> userManager)
        {
            _donationService = donationService;
            _donationRequestService = donationRequestService;
            _userManager = userManager;
        }

        public List<Donation> DonationList { get; set; } = new();

        [BindProperty]
        public int DonationId { get; set; }

        public void OnGet()
        {
         
            DonationList = _donationService.GetAll();
            
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Checking for has the recipient requested the item before
            var donation = _donationService.GetDonationById(DonationId);
            var userId = (await _userManager.GetUserAsync(HttpContext.User)).Id;
            var user = await _userManager.Users.FirstAsync(u => u.Id == userId);

            var exists = await _donationRequestService.DonationRequestExists(DonationId, userId);

            // Donor cannot request for his/her own donation
            var donor = donation.User;
            if (donor == user)
            {
                TempData["FlashMessage.Type"] = "danger";
                TempData["FlashMessage.Text"] = string.Format("You are not allowed to request your own donation");
                return Redirect("/FoodSharing/Index");
            }
                
            if (exists == false)
            {
                var request = new DonationRequest()
                {
                    Donation = donation,
                    Donor = donation?.User,
                    Recipient = user,
                    Status = RequestStatus.PENDING
                };

                _donationRequestService.AddRequest(request);

                TempData["FlashMessage.Type"] = "success";
                TempData["FlashMessage.Text"] = string.Format("You have request for the items, Please wait for the donor to response");
                return Redirect("/FoodSharing/Index");
            }
            else
            {
                TempData["FlashMessage.Type"] = "danger";
                TempData["FlashMessage.Text"] = string.Format("You have requested for the items before");
                return Redirect("/FoodSharing/Index");

            }


            
        }
    }
}
