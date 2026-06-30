using Microsoft.AspNetCore.SignalR;

public class SignalHub : Hub
{
    public async Task RegisterUser(string userId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, userId);
    }

    public async Task WatchUser(string userId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, userId);
    }

    public async Task SendFrame(string userId, byte[] imageBytes)
    {
        await Clients.Group(userId)
            .SendAsync("ReceiveFrame", userId, imageBytes);
    }
}
