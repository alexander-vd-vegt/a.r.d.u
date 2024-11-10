using System;

namespace Ardu.Common;

public static class ArduEnvConstants
{
    public static string EventBusEnvVariableName => "Ardu_EventBus";
    public static string EventBusUrl => Environment.GetEnvironmentVariable(ArduEnvConstants.EventBusEnvVariableName); 
}
