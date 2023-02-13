using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Web.Lib;
using Web.Models;
using Web.Services;
using Web.UiState;
using Web.Utils;

namespace Web.Pages;

public class RegisterModel : PageModel
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IHouseholdService _householdService;
    private readonly ICaptchaService _captchaService;
    private readonly INotificationService _notificationService;
    private readonly IImageService _imageService;

    [BindProperty, Required]
    public string UserName { get; set; }

    [BindProperty, Required]
    public string FirstName { get; set; }
    [BindProperty]
    public string UploadString { get; set; }
    [BindProperty, Required]
    public string LastName { get; set; }
    [BindProperty, Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    [BindProperty, Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    [BindProperty, Required]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Passwords doesn't match.")]
    public string ConfirmPassword { get; set; }

    [BindProperty, Required]
    public HouseholdUiState HouseholdUiState { get; set; }

    public RegisterModel(UserManager<User> userManager, SignInManager<User> signInManager, IHttpContextAccessor contextAccessor, IHouseholdService householdService, INotificationService notificationService, ICaptchaService captchaService, IImageService imageService)
    {
        _householdService = householdService;
        _userManager = userManager;
        _signInManager = signInManager;
        _contextAccessor = contextAccessor;
        _notificationService = notificationService;
        _captchaService = captchaService;
        _imageService = imageService;

    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if ((bool)HouseholdUiState.JoinHousehold && HouseholdUiState.InviteLink is null)
        {
            ModelState.AddModelError("HouseholdUiState.JoinHouseholdName", "Please fill in the household's invite code you want to join!");
            return Page();
        }
        else if ((bool)!HouseholdUiState.JoinHousehold && HouseholdUiState.CreateHouseholdName is null)
        {
            ModelState.AddModelError("HouseholdUiState.CreateHouseholdName", "Please fill in the household name you want to create!");
            if (HouseholdUiState.Address is null) ModelState.AddModelError("HouseholdUiState.Address", "Please fill in the address!");

            return Page();
        }


        if (ModelState.IsValid)
        {
            if (!(await _captchaService.CaptchaPassed(Request.Form["token"])).Value)
            {
                ModelState.AddModelError("", "You have failed the CAPTCHA. Try again.");
                return Page();

            }
            if ((bool)HouseholdUiState.JoinHousehold)
            {

                Result<Household> householdResult = await _householdService.VerifyLink(HouseholdUiState.InviteLink);
                if (householdResult.Status == Status.FAILURE)
                {
                    ModelState.AddModelError("HouseholdUiState.JoinHouseholdName", householdResult.Message);
                    householdResult.Print();
                    return Page();
                }
            }
            else if ((bool)!HouseholdUiState.JoinHousehold)
            {
                Result<Household> createHouseholdResult = await _householdService.RetrieveHouseholdDetailsByName(HouseholdUiState.CreateHouseholdName);
                if (createHouseholdResult.Status == Status.SUCCESS)
                {
                    ModelState.AddModelError("HouseholdUiState.CreateHouseholdName", createHouseholdResult.Message);
                    createHouseholdResult.Print();
                    return Page();
                }
            }

            var newUser = new User()
            {
                UserName = UserName,
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,


            };

            var result = await _userManager.CreateAsync(newUser, Password);

            if (result.Succeeded)
            {

                await _signInManager.SignInAsync(newUser, false);

                var user = await _userManager.FindByNameAsync(UserName);
                var userId = user.Id;
                await _imageService.StoreImageFromUrl($"https://api.dicebear.com/5.x/pixel-art/png?seed={UploadString}", await _userManager.FindByIdAsync(userId));



                if ((bool)!HouseholdUiState.JoinHousehold)
                {
                    await _householdService.CreateHousehold(HouseholdUiState.CreateHouseholdName, HouseholdUiState.Address, userId);
                    await _householdService.AddUserToHousehold(userId, (await _householdService.RetrieveHouseholdDetailsByName(HouseholdUiState.CreateHouseholdName)).Value.HouseholdId);
                    TempData["success"] = "Created household!";

                }
                else
                {

                    await _householdService.AddUserToHousehold(userId, (await _householdService.VerifyLink(HouseholdUiState.InviteLink)).Value.HouseholdId);
                    TempData["success"] = "Added to household!";
                }

                //_contextAccessor.HttpContext.Session.SetString(SessionVariable.UserName, UserName);
                //_contextAccessor.HttpContext.Session.SetString(SessionVariable.UserId, userId);
                //_contextAccessor.HttpContext.Session.SetString(SessionVariable.HousholdName, user.Household.Name);

                var notif = _notificationService.Create(
                    "Green Mile",
                    "Welcome to GreenMile! Thank you for registering with us."
                );

                await _notificationService.SendNotification(notif, user);

                return RedirectToPage("/Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
        return Page();
    }
}