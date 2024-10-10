using System;
using Ardu.Common;
using Docker.DotNet.Models;

namespace arduhostsupervisor.Extensions;

public static class ArduComponentExtensions
{
    public static HostConfig GetHostConfig(this ArduComponent component)
    {
        if(component.Ports == null || component.Ports?.Count == 0)
            return default;
        
        var hc = new HostConfig();
        hc.PortBindings = new Dictionary<string, IList<PortBinding>>();
        foreach(var port in component.Ports!)
        {
            hc.PortBindings.Add(port.Value,  new List<PortBinding> {new PortBinding {HostPort = port.Value}});
        }
        hc.PublishAllPorts = true;    
        return hc;
    }

    /// <summary>
    /// Extension method to generate a dictionary to tell what host ports need to be exposed 
    /// </summary>
    /// <param name="component">The ardu component to generate from</param>
    /// <returns></returns>
    public static Dictionary<string, EmptyStruct> GetExposedPorts(this ArduComponent component)
    {
        if(component.Ports == null || component.Ports?.Count == 0)
            return default;
        
        var exposedPorts = new Dictionary<string, EmptyStruct>();
        foreach(var port in component.Ports!){
            exposedPorts.Add(port.Key, default(EmptyStruct));
        }

        return exposedPorts;
    }

    public static Dictionary<string, string> AddArduComponentLabels(this ArduComponent component)
    {
        //todo: implement "tags" from component
        var tags = new Dictionary<string, string>();
        tags.Add("Ardu", component.Name);
        tags.Add("ArduRestartOnExit", component.KillOnExit.ToString());
        return tags;
    }

}
