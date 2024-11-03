using System;
using Ardu.Common;
using Ardu.Common.Services;
using arduhostsupervisor.Extensions;
using Docker.DotNet;
using Docker.DotNet.Models;

namespace arduhostsupervisor.Services;

public class ComponentDockerContainerService : IComponentContainerService
{
    private readonly DockerClient _dockerClient;
    private Dictionary<string, string> _containerComponentIndex;
    private ILogger _log;

    public ComponentDockerContainerService(DockerClient dockerClient, ILogger<ComponentDockerContainerService> logger)
    {
        _dockerClient = dockerClient;
        _containerComponentIndex = new Dictionary<string, string>();
        _log = logger;
    }
    public async Task StartComponent(ArduComponent component){
        try
        {
            if(await ComponentExcists(component) == false)
            {
                var pullState = this.PullImage(component.Image);
                await pullState.WaitAsync(CancellationToken.None);
                while(!pullState.IsCompleted)
                {
                    _log.LogInformation($"busy pulling: {component.Image}");
                    await Task.Delay(1000);
                }
                _log.LogInformation($"done pulling: {component.Image}");

                var param = new CreateContainerParameters(){
                    Image = component.Image,
                    Name =  component.Name,
                    Labels = component.AddArduComponentLabels(),
                    Hostname = component.Name
                };
                param.HostConfig = component.GetHostConfig();    
                param.ExposedPorts = component.GetExposedPorts();
                var response = await _dockerClient.Containers.CreateContainerAsync(param);
                _containerComponentIndex.Add(component.Name, response.ID);
                await _dockerClient.Containers.StartContainerAsync(response.ID,null);   
            }
            else{
                _log.LogWarning($"Instance of compontent {component.Name} with image {component.Image} already present. Skipping Component");
            } 
        } 
        catch(Exception ex)
        {
            _log.LogError(ex, $"Error while starting Component {component.Name}:{component.Image} : {ex.Message}");
        }   
    }
    
    public async Task StopComponent (ArduComponent arduComponent){
        if(_containerComponentIndex.ContainsKey(arduComponent.Name))
        {
            var param = new ContainerStopParameters();
            await _dockerClient.Containers.StopContainerAsync(arduComponent.Name ,param);
        }
        else{
            throw new Exception("to do fix this");
        }
    }

    public async Task<IEnumerable<ArduComponentStatus>> GetComponentsWithStatus()
    {
        var list = await _dockerClient.Containers.ListContainersAsync( new ContainersListParameters{
           All = true            
        });
        
        var components = list.Where(c => c.Labels.ContainsKey("Ardu"))
            .Select(c => new ArduComponentStatus{
                    Name = c.Names.First(),
                    Image = c.Image,
                    Status = c.Status,
                    KillOnExit = Boolean.Parse(c.Labels["ArduRestartOnExit"]),
                    State = c.State
                })
            .ToList();

        return components;
    }

    public async Task<IEnumerable<ArduComponent>> GetComponents(){
        return await this.GetComponentsWithStatus();
    }

    private async Task<Progress<JSONMessage>> PullImage(string image){
        var img = image.Contains(':') ? image.Split(':')[0] : image;
        var tag = image.Contains(':') ? image.Split(':')[1] : "latest";
        var progress = new Progress<JSONMessage>();
        await _dockerClient.Images.CreateImageAsync(new ImagesCreateParameters
            {
                FromImage = img,
                Tag = tag
            },
            new AuthConfig(),
            progress);
        return progress;
    }

    private async Task<bool> ComponentExcists(ArduComponent component){
        var containers = await _dockerClient.Containers.ListContainersAsync(new ContainersListParameters{
             All=true
        });
        var excisting = containers.FirstOrDefault(c => c.Names.First() == $"/{component.Name}" && c.Image == component.Image);
        if(excisting != null){
            return true;
        }
        else if(containers.FirstOrDefault(c => c.Names.Contains(component.Name) && c.Image != component.Image) != null){
            throw new Exception("Container name already in use with an other image");
        }
        else
        {
            return false;
        }
    }

}

