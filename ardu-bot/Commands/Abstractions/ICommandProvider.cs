using Microsoft.AspNet.SignalR;

namespace Ardu.Bot;

public interface ICommandProvider
{
    IEnumerable<string> GetCommandList();
    ICommand GetCommandInstance(string commandName, IHubContext context);
    IEnumerable<CommandAttribute> GetCommandsInfo();
}
