
using System;
using Discord;
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

    private Task OnMessageRecieved(SocketMessage message)
    {
        _log.LogInformation($"Message recieved from: {message.Author.GlobalName}, on Channel {message.Channel.Name}, with contents: {Environment.NewLine}{message.Content}");
        return Task.CompletedTask;
    }
    private Task OnConnected()
    {
        _log.LogInformation("Connected to Discord!");
        return Task.CompletedTask;
    }

    private async Task OnUserJoined(SocketGuildUser user)
    {
        _log.LogInformation($"user: {user.GlobalName}, has joined us!");
        await user.SendMessageAsync("Hello there!");
    }

    private Task OnDiscconect(Exception exception)
    {
        _log.LogCritical("disconnected from discord!");
        return Task.CompletedTask;
    }

}
