using Microsoft.AspNetCore.Identity;
using System.Xml.Linq;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Web.Models;
using Web.Services;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Authorization;

namespace Web.Pages.FoodSharing
{
    [Authorize]
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

        public async Task OnGetAsync()
        {

            var userId = (await _userManager.GetUserAsync(HttpContext.User)).Id;
            DonationList = _donationService.GetAll(userId);
            
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Checking for has the recipient requested the item before
            var donation = _donationService.GetDonationById(DonationId);
            var userId = (await _userManager.GetUserAsync(HttpContext.User)).Id;
            User user = await _userManager.Users.FirstAsync(u => u.Id == userId);

            var exists = await _donationRequestService.DonationRequestExists(DonationId, userId);

            // Donor cannot request for his/her own donation
            User donor = donation.User;

            if (donor == user)
            {
                TempData["FlashMessage.Type"] = "danger";
                TempData["FlashMessage.Text"] = "You are not allowed to request your own donation";
                return Redirect("/FoodSharing/Index");
            }
                
            if (exists == false)
            {
                var request = new DonationRequest()
                {
                    Donation = donation,
                    Donor = donor,
                    Recipient = user,
                    Status = RequestStatus.PENDING
                };

                _donationRequestService.AddRequest(request);

                MailMessage message = new MailMessage();
                message.To.Add("liujiajun2003@gmail.com");
                message.Subject = "Test Email";
                message.Body = "Your received a donation request from user GUOLIHENG";
                message.IsBodyHtml = false;
                message.From = new MailAddress("greenmile2024bycrazyapi@gmail.com");

                SmtpClient client = new SmtpClient();
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.UseDefaultCredentials = false;
                client.EnableSsl = true;
                // Green Mile Email Account Password: greenmile2023!
                client.Credentials = new NetworkCredential("greenmile2024bycrazyapi@gmail.com", "arcpfpypntqmhnxf");
                await client.SendMailAsync(message);
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
