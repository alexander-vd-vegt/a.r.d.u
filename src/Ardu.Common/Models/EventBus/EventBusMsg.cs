using System.Text.Json;

namespace Ardu.Common.Models.EventBus;

public class EventBusMsg
{
    public string Sender {get; set;}
    public string Topic {get; set;}
    public string RawPayload {get; set;}

    public EventBusMsg Serialize()
    {
        this.RawPayload = JsonSerializer.Serialize(this);
        return this;
    }

    public T Deserialize<T>() where T : EventBusMsg
    {
        var obj = JsonSerializer.Deserialize<T>(this.RawPayload);
        return obj;
    }
}
