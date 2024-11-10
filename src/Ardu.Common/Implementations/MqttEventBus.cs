using System;
using Ardu.Common.Interfaces;
using Ardu.Common.Models.EventBus;
using MQTTnet.Client;
using MQTTnet.Client.Internal;
using MQTTnet;

namespace Ardu.Common.Implementations;

public class MqttEventBus : IEventbus, IDisposable
{
    protected IMqttClient _mqttClient;
    private List<EventBusSubscription> _subscriptions;
    public MqttEventBus(){
        InitMqttClient();
        var mqttOptions = GetMqTTOptions();
        _mqttClient.ConnectAsync(mqttOptions).Wait();
        _mqttClient.ApplicationMessageReceivedAsync += HandleIncomingMessage;
    }

    public void Dispose()
    {
        _mqttClient.ApplicationMessageReceivedAsync -= HandleIncomingMessage;
    }

    public async Task Publish<T>(T payload) where T : EventBusMsg
    {
        throw new NotImplementedException();
    }

    public string Subscribe<T>(string topic, Action<T> callback) where T : EventBusMsg
        {
        var subscription = new EventBusSubscription{
            Topic = topic,
            Callback = (Action<EventBusMsg>)callback
        };

        _subscriptions.Add(subscription);
        return subscription.Id;
    }

    public void UnSubscribe(string subscriptionId)
    {
        var sub = _subscriptions.FirstOrDefault(s => s.Id == subscriptionId);
        if(sub != null)
            _subscriptions.Remove(sub);
    }

    private Task HandleIncomingMessage(MqttApplicationMessageReceivedEventArgs msg)
    {
        var topic = msg.ApplicationMessage.Topic;
        var payload = msg.ApplicationMessage.Payload.ToString();
        var callbacks = _subscriptions.Where(s => s.Topic == topic).Select(s => s.Callback);

        var evmsg = new EventBusMsg{
            Topic = topic,
            RawPayload = payload,
            Sender = msg.ClientId
        };

        foreach(var callback in callbacks){
            callback?.Invoke(evmsg);
        }

        return Task.CompletedTask;
    }

    protected void InitMqttClient()
    {
        var factory = new MqttFactory();
        this._mqttClient = factory.CreateMqttClient();
    }

    protected virtual MqttClientOptions GetMqTTOptions(string url = "")
    {
        if(String.IsNullOrEmpty(url))
            url = ArduEnvConstants.EventBusUrl;

        var builder = new MqttClientOptionsBuilder();
        builder.WithConnectionUri(url);

        return builder.Build();
    }
}
