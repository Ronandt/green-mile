using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

using Web.Models;
using Web.Services;

namespace Web.Utils;

public class HouseholdHub : Hub
{
    private readonly UserManager<User> _userManager;
    private readonly MessageService _messageService;

    public HouseholdHub(UserManager<User> userManager, MessageService messageService)
    {
        _userManager = userManager;
        _messageService = messageService;
    }

    public async Task SendMessage(string userId, string text)
    {
        var user = await _userManager.FindByIdAsync(userId);
        var message = await _messageService.CreateMessage(user, text);
        await Clients.Group(user.HouseholdId.ToString()).SendAsync("ReceiveMessage", user.UserName, message.Key, text, message.CreatedAt.ToShortTimeString());
    }

    public async Task DeleteMessage(string messageId)
    {
        Console.WriteLine("Tried to delete message");
        Console.WriteLine(messageId);
        var success = int.TryParse(messageId, out var messageIdInt);
        if (!success)
        {
            return;
        }
        var message = await _messageService.FindById(messageIdInt);
        if (message is not null)
        {
            var householdId = await _messageService.GetHouseholdId(message);
            await _messageService.DeleteMessage(message);
            await Clients.Group(householdId.ToString()).SendAsync("ReceiveDeleteMessage", messageId);
        }
    }

    public async Task JoinGroup(string householdId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, householdId);
    }
}