using Microsoft.AspNet.SignalR;

namespace Ardu.Bot.Hubs;
public class CommandHub(ICommandProvider commandProvider) : Hub
{
    private readonly ICommandProvider _commandProvider = commandProvider;
    public async Task CommandExecute(string commandstring){
        throw new NotImplementedException();
    }

    public Task<IEnumerable<string>> GetCommandList(){
        return Task.FromResult(_commandProvider.GetCommandList());
    } 
}
