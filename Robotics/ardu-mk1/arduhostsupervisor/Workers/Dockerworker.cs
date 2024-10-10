
using System.Diagnostics;
using Ardu.Common;
using Ardu.Common.Interfaces;
using Docker.DotNet;
using Docker.DotNet.Models;

namespace arduhostsupervisor.Workers;

public class Dockerworker(DockerClient dockerClient, ILogger<Dockerworker> logger) : BackgroundService
{
    private readonly DockerClient _dockerClient = dockerClient;
    private readonly ILogger _log = logger;

    private readonly IConfig _config;
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await ContainerStartup();
    }

    private async Task ContainerStartup(){
        
    }

    private async Task StartContainer(ArduComponent component){
        var param = new CreateContainerParameters(){
            Image = component.Image,
            Name =  component.Name
        };
        await _dockerClient.Containers.CreateContainerAsync(param);
    }
}
