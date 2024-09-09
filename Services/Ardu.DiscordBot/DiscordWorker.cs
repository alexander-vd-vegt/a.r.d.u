
using Discord.WebSocket;

namespace Ardu.DiscordBot;

public class DiscordWorker : BackgroundService
{
    private readonly DiscordSocketClient _discordClient;
    private readonly ILogger _log;

    public void Init()
    {
        _log.LogInformation("Init() DiscordWorker");
        _discordClient.MessageReceived += OnMessageRecieved;
        _discordClient.Connected += OnConnected;
        _discordClient.Disconnected += OnDiscconect;
        _discordClient.UserJoined += OnUserJoined;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        this.Init();
        while(!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(100);
        }
    }
    public override void Dispose()
    {
        _log.LogInformation("Disposing DiscordWorker...");
        _discordClient.MessageReceived -= OnMessageRecieved;
        base.Dispose();
    }

    private async Task OnMessageRecieved(SocketMessage message)
    {
        throw new NotImplementedException();
    }
    private async Task OnConnected()
    {
        throw new NotImplementedException();
    }

    private async Task OnUserJoined(SocketGuildUser user)
    {
        throw new NotImplementedException();
    }

    private async Task OnDiscconect(Exception exception)
    {
        throw new NotImplementedException();
    }

}
