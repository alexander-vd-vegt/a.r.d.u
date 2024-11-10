using Ardu.Common;
using Ardu.Common.Implementations;
using arduhostsupervisor.Models;
using Microsoft.Extensions.Options;

namespace arduhostsupervisor.Extensions;

public class SupervisorEventBus : MqttEventBus
{
    private IOptions<SupervisorConfig> _config;
    public SupervisorEventBus(IOptions<SupervisorConfig> cconfig){
        _config = cconfig;
        InitMqttClient();
        var url = _config.Value.Services[ArduEnvConstants.EventBusEnvVariableName];
        var options = GetMqTTOptions(url);
        _mqttClient.ConnectAsync(options).Wait();
    }
}
