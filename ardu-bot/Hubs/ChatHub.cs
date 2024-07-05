using Microsoft.AspNet.SignalR;

namespace Ardu.Bot.Hubs;

public class ChatHub : Hub
{
    public async override Task OnConnected()
    {
        await Clients.Caller.SendAsync("ReceiveMessage", "Hello there!");
        await base.OnConnected();
    }
    public async Task SendTextMessage(string text){
        await Clients.Caller.SendAsync("ReceiveMessage", text);

    }
}
