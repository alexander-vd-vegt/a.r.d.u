using Microsoft.AspNet.SignalR;

namespace Ardu.Bot;

public interface ICommand{
    Task ExecuteAsync(CommandAgruments arguments);
}

public abstract class CommandBase : ICommand
{
    protected IHubContext _context;
    public void Intialize(IHubContext context){
        _context = context;
    }
    public abstract Task ExecuteAsync(CommandAgruments arguments);
}

public class ArgumentDefintion{
    public string Type {get; set;}
    public string Description {get; set;}
    public string Example {get; set;}
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class CommandAttribute : Attribute{
    public string Name {get; set;}
    public string Description {get; set;}
    public Dictionary<string, ArgumentDefintion> Arguments {get; set;}

}