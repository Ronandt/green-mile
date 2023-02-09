using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

using Web.Data;
using Web.Models;

namespace Web.Hubs
{
    public class ChatHub : Hub
    {
        private readonly DataContext _context;

        public ChatHub(DataContext context)
        {
            _context = context;
        }

        public override Task OnConnectedAsync()
        {
            Groups.AddToGroupAsync(Context.ConnectionId, Context.User.Identity.Name);
            return base.OnConnectedAsync();
        }
        public async Task SendMessage(string user, string message)
        {
            //message send to all users
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendMessageToGroup(string sender, string receiver, string message)
        {
            // Insert message into database
            _context.Messages.Add(new MessageHistory
            {
                Sender = Context.User.Identity.Name,
                Receiver = receiver,
                Text = message,
                Timestamp = DateTime.Now
            });

            await _context.SaveChangesAsync();

            //message send to receiver only
            await Clients.Group(receiver).SendAsync("ReceiveMessage", sender, message);
        }
    }
}
