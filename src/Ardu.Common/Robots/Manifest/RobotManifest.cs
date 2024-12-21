using System;
using Ardu.Common.Models.EventBus;

namespace Ardu.Common.Robots;

public interface IRobotManifest
{
    public static IEnumerable<ActuatorInfo> Actuators {get;}
    public static IEnumerable<SensorInfo> Sensors {get;}
}


public class ActuatorInfo{
    public string Topic {get; set;}
    public string Name{ get; set;}
    public int MinimalPosition {get; set;}
    public int MaximumPosition {get; set;}

    public ActuatorEvent GenerateEvent(int position, int transitionTime = 0){
        return new ActuatorEvent{
            Topic = this.Topic,
            Sender = ThisAssembly.AssemblyName,
            Position = position,
            TransitionTime = transitionTime 
        };
    }

    public EventBusMsg GenerateGenericEvent(int position, int transitionTime = 0){
        var ae = this.GenerateEvent(position, transitionTime);
        return ae.Serialize();
    }
}

public class SensorInfo {
    public string Topic {get; set;}
    public string Name {get; set;}
    public string Type {get; set;}
    public string Unit {get; set;}
    public Type OutputType {get; set;} 
}

public static class ManiFestConstants
{
    public const string HeadTurn = "HeadTurn";
    public const string HeadTilt = "HeadTilt";
}