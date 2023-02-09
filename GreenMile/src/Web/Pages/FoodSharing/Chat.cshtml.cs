using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

using Web.Models;

namespace Web.Pages.FoodSharing
{
    [Authorize]
    public class ChatModel : PageModel
    {
        private readonly UserManager<User> _userManager;

        [BindProperty]
        public List<SelectListItem> Users { get; set; }

        [BindProperty]
        public string MyUser { get; set; }

        public ChatModel(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public void OnGet()
        {
            //get all the users from the database
            Users = _userManager.Users.ToList()
                .Select(a => new SelectListItem { Text = a.UserName, Value = a.UserName })
                .OrderBy(s => s.Text).ToList();

            //get logged in user name
            MyUser = User.Identity.Name;
        }
    }
}
