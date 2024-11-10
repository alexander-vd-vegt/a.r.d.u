using System;

namespace Ardu.Common.Models.EventBus;

public class EventBusMsg
{
    public string Sender {get; set;}
    public string Topic {get; set;}
    public string RawPayload {get; set;}
}
