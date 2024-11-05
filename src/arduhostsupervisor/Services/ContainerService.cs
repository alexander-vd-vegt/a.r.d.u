using System;
using Ardu.Common;
using Ardu.Common.Services;
using arduhostsupervisor.Extensions;
using arduhostsupervisor.Models;
using Docker.DotNet;
using Docker.DotNet.Models;
using Microsoft.Extensions.Options;

namespace arduhostsupervisor.Services;

public class ComponentDockerContainerService : IComponentContainerService
{
    private readonly DockerClient _dockerClient;
    private ILogger _log;
    private IOptions<SupervisorConfig> _config;

    public ComponentDockerContainerService(DockerClient dockerClient, 
        ILogger<ComponentDockerContainerService> logger,
        IOptions<SupervisorConfig> config)
    {
        _dockerClient = dockerClient;
        _log = logger;
        _config = config;
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
                var envVars = new List<string>();
                var param = new CreateContainerParameters(){
                    Image = component.Image,
                    Name =  component.Name,
                    Labels = component.AddArduComponentLabels(),
                    Hostname = component.Name,
                    Env = CompileEnvVars()
                };
                param.HostConfig = component.GetHostConfig();    
                param.ExposedPorts = component.GetExposedPorts();
                var response = await _dockerClient.Containers.CreateContainerAsync(param);
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
        try
        {
            _log.LogInformation($"shutting down and removing container: {arduComponent.Name}");
            var param = new ContainerStopParameters();
            await _dockerClient.Containers.StopContainerAsync(arduComponent.Name ,param);
            await _dockerClient.Containers.RemoveContainerAsync(arduComponent.Name, new ContainerRemoveParameters());
        }
        catch(Exception ex)
        {
            _log.LogError(ex,$"Unexpected exception on shutting down and removing container: {arduComponent.Name}");
            throw ex;
        }
    }

    public async Task<IEnumerable<ArduComponentStatus>> GetComponentsWithStatus()
    {
        var list = await _dockerClient.Containers.ListContainersAsync( new ContainersListParameters{
           All = true            
        });
        
        var components = list.Where(c => c.Labels.ContainsKey("Ardu"))
            .Select(c => new ArduComponentStatus{
                    Name = c.Names.First().TrimStart('/'),
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

    private List<string> CompileEnvVars(){
        return _config.Value.Services
            .Select( s => $"{s.Key}={s.Value}")
            .ToList();
    }

}

