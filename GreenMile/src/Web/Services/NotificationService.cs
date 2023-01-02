using Web.Data;
using Web.Models;

namespace Web.Services;

/// <summary>
/// Service for sending notifications to users.
/// </summary>
public class NotificationService : INotificationService
{
    private readonly AuthDbContext _context;

    public NotificationService(AuthDbContext context)
    {
        _context = context;
    }

    public async Task<bool> SendNotification(Notification notification)
    {
        if (notification is null)
        {
            throw new ArgumentNullException(nameof(notification));
        }
        var users = _context.Users.AsEnumerable();
        foreach (var user in users)
        {
            var notificationClone = new Notification()
            {
                Message = notification.Message,
                System = notification.System,
                Date = notification.Date,
                Read = notification.Read,
            };
            notificationClone.SetUser(user);
            await _context.Notifications.AddAsync(notificationClone);
        }
        return true;
    }

    public Task<bool> SendNotification(Notification notification, User user)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SendNotification(Notification notification, IEnumerable<User> users)
    {
        throw new NotImplementedException();
    }
}