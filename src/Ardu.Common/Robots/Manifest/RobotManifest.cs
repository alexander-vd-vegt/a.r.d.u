using System;

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