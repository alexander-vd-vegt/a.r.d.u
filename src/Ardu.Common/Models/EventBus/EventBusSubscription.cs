namespace Ardu.Common.Models.EventBus;

public record class EventBusSubscription
{
    public string Id => Guid.NewGuid().ToString();
    public string Topic {get; set;}
    public Action<EventBusMsg> Callback {get; set;}
}
