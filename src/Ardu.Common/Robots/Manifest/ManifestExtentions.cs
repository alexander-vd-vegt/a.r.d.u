using System;

namespace Ardu.Common.Robots.Manifest;

public static class ManifestExtentions
{
    public static ActuatorInfo HeadTurn(this IEnumerable<ActuatorInfo> actuators){
        return actuators.First(a => a.Name == ManiFestConstants.HeadTurn);
    }

    public static ActuatorInfo? HeadTilt(this IEnumerable<ActuatorInfo> actuators){
        return actuators.FirstOrDefault(a => a.Name == ManiFestConstants.HeadTilt);
    }
}
