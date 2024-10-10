using System;

namespace Ardu.Common.Services;

public interface IComponentContainerService
{
    Task StartComponent(ArduComponent component);
    Task StopComponent (ArduComponent arduComponent);
    Task<IEnumerable<ArduComponentStatus>> GetComponentsWithStatus();
    Task<IEnumerable<ArduComponent>> GetComponents();
}