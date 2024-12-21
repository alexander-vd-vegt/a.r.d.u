using System;

namespace Ardu.Common.Models.EventBus;

public class ActuatorEvent : EventBusMsg
{
    public int Position {get; set;}
    public int TransitionTime {get; set;}
}
