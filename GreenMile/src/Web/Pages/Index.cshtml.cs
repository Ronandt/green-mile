using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Web.Models;

namespace Web.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public IndexModel(ILogger<IndexModel> logger, UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            return User.Identity.IsAuthenticated && !await _userManager.IsInRoleAsync(await _userManager.GetUserAsync(User), "Member")
                ? Redirect("/account/transferhousehold")
                : Page();
        }
        catch (Exception)
        {
            await _signInManager.SignOutAsync();
            return Redirect("/Index");
        }
    }
}