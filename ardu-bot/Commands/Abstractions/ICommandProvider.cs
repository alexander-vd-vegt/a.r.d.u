namespace Ardu.Bot;

public interface ICommandProvider
{
    IEnumerable<string> GetCommandList();
    ICommand GetCommandInstance(string commandName);
}
