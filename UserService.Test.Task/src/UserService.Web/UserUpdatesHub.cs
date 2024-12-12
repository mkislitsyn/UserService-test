using Microsoft.AspNetCore.SignalR;

namespace UserService.Web
{
    public class UserUpdatesHub : Hub
    {
        public async Task NotifyUserUpdated(int userId, string name, string email, string role)
        {
            // Broadcast user info updates to all connected clients
            await Clients.All.SendAsync("UserUpdated", new { userId, name, email, role });
        }
    }
}
