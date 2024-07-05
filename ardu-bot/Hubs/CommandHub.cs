using Microsoft.AspNet.SignalR;

namespace Ardu.Bot.Hubs;
public class CommandHub(ICommandProvider commandProvider) : Hub
{
    private readonly ICommandProvider _commandProvider = commandProvider;
    public async Task CommandExecute(string command, Dictionary<string, object> arguments){
        var args = new CommandAgruments{
            ClientId = Context.ConnectionId,
            Arguments = arguments
        };
        var commandInstance = _commandProvider.GetCommandInstance(command, Clients.Caller);
        await commandInstance.ExecuteAsync(args);
    }

    public Task<IEnumerable<string>> GetCommandList(){
        return Task.FromResult(_commandProvider.GetCommandList());
    }

    public Task<IEnumerable<CommandAttribute>> GetCommandsInfo(){
        return Task.FromResult(_commandProvider.GetCommandsInfo());
    } 
}
