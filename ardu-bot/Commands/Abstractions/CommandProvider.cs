using System.Reflection;
using Microsoft.AspNet.SignalR;

namespace Ardu.Bot;

public class CommandProvider : ICommandProvider
{
    private IEnumerable<Type> _commands;
    private readonly IServiceProvider _serviceProvider;

    public CommandProvider(IServiceProvider services){
        _serviceProvider = services;
        LoadCommands();
    }
    public void LoadCommands(){
        var currentAssembly = Assembly.GetExecutingAssembly();

        var commandTypes = currentAssembly.GetTypes()
            .Where(t => t.IsPublic && t.BaseType == typeof(CommandBase) && t.GetCustomAttribute<CommandAttribute>() != null);

        _commands = commandTypes;
    }

    public IEnumerable<string> GetCommandList(){
        return _commands.Where(c=> c.GetCustomAttribute<CommandAttribute>() != null)
        .Select(c => c.GetCustomAttribute<CommandAttribute>()!.Name);
    }

    public IEnumerable<CommandAttribute> GetCommandsInfo(){
        return _commands.Where(c=> c.GetCustomAttribute<CommandAttribute>() != null)
        .Select(c => c.GetCustomAttribute<CommandAttribute>()!);
    }

    public ICommand GetCommandInstance(string commandName, IHubContext context){
        var commandType = _commands.FirstOrDefault(c => c.GetCustomAttribute<CommandAttribute>()!.Name == commandName);
        if(commandType == null){
            throw new KeyNotFoundException($"command with the name {commandName} is not found by the commandProvider");
        }
        var instance = (CommandBase)_serviceProvider.GetService(commandType);
        instance.Intialize(context);
        return instance;
    }
}
