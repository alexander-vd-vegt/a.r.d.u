using System;
using Ardu.Common;
using Docker.DotNet;
using Docker.DotNet.Models;

namespace arduhostsupervisor.Services;

public class ComponentContainerService
{
    private readonly DockerClient _dockerClient;
    private Dictionary<string, string> _containerComponentIndex;

    public ComponentContainerService(DockerClient dockerClient)
    {
        _dockerClient = dockerClient;
        _containerComponentIndex = new Dictionary<string, string>();
    }

    public async Task StartContainer(ArduComponent component){
        var tags = new Dictionary<string, string>();
        tags.Add("Ardu", component.Name);
        tags.Add("ArduRestartOnExis", component.KillOnExit.ToString());
        
        var param = new CreateContainerParameters(){
            Image = component.Image,
            Name =  component.Name,
            Labels = tags
        };
        var response = await _dockerClient.Containers.CreateContainerAsync(param);
        _containerComponentIndex.Add(component.Name, response.ID);
    }
    
    public async Task StopContainer (ArduComponent arduComponent){
        if(_containerComponentIndex.ContainsKey(arduComponent.Name))
        {
            var param = new ContainerStopParameters();
            await _dockerClient.Containers.StopContainerAsync(arduComponent.Name ,param);
        }
        else{
            throw new Exception("to do fix this");
        }
    }

}
