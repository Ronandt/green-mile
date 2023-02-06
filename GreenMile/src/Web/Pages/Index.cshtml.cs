using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Web.Models;
using Web.Services;

namespace Web.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly UserManager<User> _userManager;
    private readonly CategoryService _categoryService;

    public IndexModel(ILogger<IndexModel> logger, UserManager<User> userManager, CategoryService categoryService)
    {
        _logger = logger;
        _userManager = userManager;
        _categoryService = categoryService;
    }

    public async Task<IActionResult> OnGetAsync()
    {
       await _categoryService.Prepopulate();
        
        if (User != null && User.Identity.IsAuthenticated && !await _userManager.IsInRoleAsync(await _userManager.GetUserAsync(User), "Member"))
        {
            return Redirect("/account/transferhousehold");
        }
        return Page();
    }
}