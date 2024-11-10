using System;

namespace Ardu.Common.Interfaces;

public interface IConfig
{
    public IEnumerable<ArduComponent> RequiredComponents {get; set;}
    public Dictionary<string,string> Services {get;set;}
}
