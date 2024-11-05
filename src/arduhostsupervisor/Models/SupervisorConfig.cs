using System;
using Ardu.Common;
using Ardu.Common.Interfaces;

namespace arduhostsupervisor.Models;

public class SupervisorConfig : IConfig
{
    public IEnumerable<ArduComponent> RequiredComponents { get; set; }
    public Dictionary<string, string> Services { get; set;}
}
