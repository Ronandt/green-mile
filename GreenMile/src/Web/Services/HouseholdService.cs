using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Web.Data;
using Web.Models;
using Web.Utils;

namespace Web.Services
{
    public class HouseholdService : IHouseholdService
    {
        private readonly UserManager<User> _userManager;
        private readonly DataContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly INotificationService _notificationService;
        public HouseholdService(UserManager<User> userManager, DataContext authDbContext, RoleManager<IdentityRole> roleManager, INotificationService notificationService)
        {
            _userManager = userManager;
            _dbContext = authDbContext;


            _roleManager = roleManager;
            _notificationService = notificationService;
        }


        async Task<Result<Tuple<User, Household>>> IHouseholdService.AddUserToHousehold(string userId, int householdId)
        {
            Household? household = await _dbContext.Household.FindAsync(householdId);
            User? user = await _userManager.FindByIdAsync(userId.ToString());
            if (household is null)
            {
                return Result<Tuple<User, Household>>.Failure("Household was not found");
            }
            if (user is null)
            {
                return Result<Tuple<User, Household>>.Failure("User was not found");
            }


            IdentityRole role = await _roleManager.FindByNameAsync("Member");
            if (role is null)
            {
                IdentityResult result = await _roleManager.CreateAsync(new IdentityRole("Member"));
                if (!result.Succeeded)
                {
                    return Result<Tuple<User, Household>>.Failure("Creation of Member failed");
                }
            }

            if (!await _userManager.IsInRoleAsync(user, "Member")) await _userManager.AddToRoleAsync(user, "Member");
            if (await _userManager.IsInRoleAsync(user, "HouseOwner") && household.OwnerId != userId) await _userManager.RemoveFromRoleAsync(user, "HouseOwner");



            user.HouseholdId = householdId;
            //household.Users.Add(user);
            //household.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            var household2 = await _dbContext.Household.Include(h => h.Users).FirstAsync(h => h.HouseholdId == householdId);
            var notif = _notificationService.Create("Green Mile", $"{user.UserName} joined the household!");
            await _notificationService.SendNotification(notif, household2);

            return Result<Tuple<User, Household>>.Success("User has been added to the household!", new Tuple<User, Household>(user, household));
        }

        async Task<Result<Household>> IHouseholdService.CreateHousehold(string householdName, string address, string ownerId)
        {


            if ((await _dbContext.Household.FirstOrDefaultAsync(x => x.Name == householdName)) is null)
            {
                Household householdObj = new Household()
                {
                    Name = householdName,
                    Address = address,
                    OwnerId = ownerId
                };

                IdentityRole role = await _roleManager.FindByNameAsync("HouseOwner");
                if (role is null)
                {
                    IdentityResult result = await _roleManager.CreateAsync(new IdentityRole("HouseOwner"));
                    if (!result.Succeeded)
                    {
                        return Result<Household>.Failure("Creation of Household owner failed");
                    }
                }
                IdentityRole roleM = await _roleManager.FindByNameAsync("Member");
                if (roleM is null)
                {
                    IdentityResult result = await _roleManager.CreateAsync(new IdentityRole("Member"));
                    if (!result.Succeeded)
                    {
                        return Result<Household>.Failure("Creation of Member failed");
                    }
                }
                User user = await _userManager.FindByIdAsync(ownerId);

                if (!await _userManager.IsInRoleAsync(user, "HouseOwner")) await _userManager.AddToRoleAsync(user, "HouseOwner");
                if (!await _userManager.IsInRoleAsync(user, "Member")) await _userManager.AddToRoleAsync(user, "Member");
                await _dbContext.Household.AddAsync(householdObj);
                await _dbContext.SaveChangesAsync();
                return Result<Household>.Success("Household has been created!", householdObj);
            }
            return Result<Household>.Failure("Household exists!");

        }



        async Task<Result<User>> IHouseholdService.RemoveUserFromHousehold(string userId)
        {
            User? user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return Result<User>.Failure("User was not found");
            }
            user.HouseholdId = null;
            if (await _userManager.IsInRoleAsync(user, "HouseOwner")) await _userManager.RemoveFromRoleAsync(user, "HouseOwner");
            if (await _userManager.IsInRoleAsync(user, "Member")) await _userManager.RemoveFromRoleAsync(user, "Member");
            if (user.OwnerOf is not null)
            {
                user.OwnerOf = null;
            }

            return Result<User>.Success("User was found and household as been removed!", user);

        }

        public async Task<Result<Household>> RetrieveHouseholdDetails(int householdId)
        {
            Household? household = await _dbContext.Household
                .Include(h => h.Users)
                .FirstOrDefaultAsync(h => h.HouseholdId == householdId);
            return household is null
                ? Result<Household>.Failure("Household was not found")
                : Result<Household>.Success("Household exists!", household);
        }

        public async Task<Result<Household>> RetrieveHouseholdDetailsByName(string householdName)
        {

            Household? household = await _dbContext.Household
                .Include(h => h.Users)
                .FirstOrDefaultAsync(x => x.Name == householdName);

            return household is null
                ? Result<Household>.Failure("Household was not found")
                : Result<Household>.Success("Household exists!", household);
        }

        public async Task<Result<Household>> VerifyLink(string code)
        {
            Household household = await _dbContext.Household.FirstOrDefaultAsync(h => h.InviteLink == code);
            if (household is null)
            {
                return Result<Household>.Failure("No household found!");
            }
            return Result<Household>.Success("Household found!", household);


        }

        public async Task<Result<User>> TransferHouseholdOwnership(string currentOwnerId, string nextOwnerId)
        {
            User currentOwner = await _userManager.FindByIdAsync(currentOwnerId);
            User nextOwner = await _userManager.FindByIdAsync(nextOwnerId);
            if (currentOwner.HouseholdId != nextOwner.HouseholdId)
            {
                return Result<User>.Failure("These two live in different households!");
            }
            if (!await _userManager.IsInRoleAsync(currentOwner, "HouseOwner"))
            {
                return Result<User>.Failure("Id inserted is not the owner!");
            }
            if (await _userManager.IsInRoleAsync(nextOwner, "HouseOwner"))
            {
                return Result<User>.Failure("Next owner id is an owner!");
            }
            Household? household = await _dbContext.Household.FindAsync(currentOwner.HouseholdId);
            if (household is null)
            {
                return Result<User>.Failure("Invalid household");
            }
            await _userManager.RemoveFromRoleAsync(currentOwner, "HouseOwner");
            await _userManager.AddToRoleAsync(nextOwner, "HouseOwner");

            household.Owner = nextOwner;
            household.OwnerId = nextOwnerId;
            _dbContext.SaveChanges();
            var notif = _notificationService.Create("Green Mile", $"Household owner is now {nextOwner.UserName}");
            await _notificationService.SendNotification(notif, household);
            return Result<User>.Success("Transfer of ownership successful!", nextOwner);

        }
    }
}
