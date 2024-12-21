using System;

namespace Ardu.Common.Robots;

public class Mk1 : IRobotManifest
{
    public static IEnumerable<ActuatorInfo> Actuators { get => compileActuators();  }
    public static IEnumerable<SensorInfo> Sensors { get =>compileSensors();  }

    private static IEnumerable<ActuatorInfo> compileActuators(){
        var actList = new List<ActuatorInfo>();

        return actList.AsEnumerable();
    }

    private static IEnumerable<SensorInfo> compileSensors(){
        var sensList = new List<SensorInfo>();

        return sensList.AsEnumerable();
    }
}

