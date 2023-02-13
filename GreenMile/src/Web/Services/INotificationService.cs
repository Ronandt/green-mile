using Web.Models;
using Web.Utils;

namespace Web.Services;

/// <summary>
/// An interface for a notification service that allows users to events and other important information.
/// </summary>
public interface INotificationService
{
    /// <summary>
    /// Retrieves the notification for a given user.
    /// </summary>
    /// <param name="user">The user whose notifications are being retrieved.</param>
    /// <returns>
    /// An enumerable collection of notifications belonging to the user.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="user"/> is <c>null</c>.
    /// </exception>
    public Task<IEnumerable<Notification>> GetNotifications(User user);
    /// <summary>
    /// Sends a notification to all users.
    /// </summary>
    /// <param name="notification">The notification the users will receive.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="notification"/> is <c>null</c>.
    /// </exception>
    public Task SendNotification(Notification notification);
    /// <summary>
    /// Sends a notification to a specific user.
    /// </summary>
    /// <param name="notification">The notification the user will receive.</param>
    /// <param name="user">The user who will receive the notification.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="notification"/> or <paramref name="user"/> is <c>null</c>.
    /// </exception>
    public Task SendNotification(Notification notification, User user);
    /// <summary>
    /// Sends a notification to multiple users.
    /// </summary>
    /// <param name="notification">The notification the users will receive.</param>
    /// <param name="users">The users who will receive the notification.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="notification"/> or <paramref name="users"/> is <c>null</c>.
    /// </exception>
    public Task SendNotification(Notification notification, IEnumerable<User> users);
    /// <summary>
    /// Sends a notification to a household
    /// </summary>
    /// <param name="notification">The notification the users will receive.</param>
    /// <param name="household">The household whos members will receive the notification.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="notification"/> or <paramref name="household"/> is <c>null</c>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when <paramref name="household.Users"/> is <c>null</c>.
    /// </exception>
    public Task SendNotification(Notification notification, Household household);

    /// <summary>
    /// Returns a notification object.
    /// </summary>
    /// <param name="system">
    /// The sub-system/module that the notification came from. An example
    /// would be "Food Sharing"
    /// </param>
    /// <param name="message">The message of the notification.</param>
    /// <returns>
    /// The notification object.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="system"/> or
    /// <paramref name="message"/> is <c>null</c>.
    /// </exception>
    public Notification Create(string system, string message)
    {
        return system is null
            ? throw new ArgumentNullException(nameof(system))
            : message is null
            ? throw new ArgumentNullException(nameof(message))
            : (new()
            {
                System = system,
                Message = message,
                Date = DateTime.Now,
                Read = false
            });
    }

    Task<Notification?> FindById(int notificationId);
    Task Update(Notification notification);
    Task Delete(Notification notification);
    Task<int> GetUnreadCount(User user);
}