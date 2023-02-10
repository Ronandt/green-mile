using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Web.Data;
using Web.Models;

namespace Web.Services;

public class MessageService
{
    private readonly DataContext _context;
    private readonly IHouseholdService _householdService;

    public MessageService(DataContext context, IHouseholdService householdService)
    {
        _context = context;
        _householdService = householdService;
    }

    public DbSet<Message> Messages => _context.Messages;

    public async Task<Message> CreateMessage(User user, string text)
    {
        var household = (await _householdService.RetrieveHouseholdDetails((int)user.HouseholdId)).Value;
        var message = new Message
        {
            User = user,
            Household = household,
            Text = text
        };
        _context.Messages.Add(message);
        await _context.SaveChangesAsync();
        return message;
    }

    public async Task<List<Message>> GetMessagesByHouseholdId(int householdId)
    {
        var household = (await _householdService.RetrieveHouseholdDetails(householdId)).Value;

        return await _context.Messages
            .Where(m => m.Household == household)
            .ToListAsync();
    }

    public async Task<Message?> FindById(int id)
    {
        return await Messages.FindAsync(id);
    }

    public async Task DeleteMessage(Message message)
    {
        Messages.Remove(message);
        await _context.SaveChangesAsync();
    }

    public async Task<int?> GetHouseholdId(Message message)
    {
        return await Messages
            .Include(m => m.Household)
            .Where(m => m.Key == message.Key)
            .Select(m => m.Household.HouseholdId)
            .FirstOrDefaultAsync();
    }
}