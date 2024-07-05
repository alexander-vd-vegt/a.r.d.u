using Microsoft.AspNet.SignalR;

namespace Ardu.Bot;

[Command(Name = "Greeting", Description = "This command says hello")]
public class Greeting : CommandBase
{
    public Greeting(IHubContext context)
    {
        _context = context;
    }

    public override async Task ExecuteAsync(CommandAgruments arguments, CancellationToken cancellationToken)
    {
        await _context.Clients.Client(arguments.ClientId).SendAsync("Well hello there!!");
    }
}
