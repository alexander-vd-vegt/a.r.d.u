
using System.Diagnostics;
using Docker.DotNet;
using Docker.DotNet.Models;

namespace arduhostsupervisor.Workers;

public class Dockerworker(DockerClient dockerClient, ILogger<Dockerworker> logger) : BackgroundService
{
    private readonly DockerClient _dockerClient = dockerClient;
    private readonly ILogger _log = logger;
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await ContainerStartup();
    }

    private async Task ContainerStartup(){
        var listParam = new ContainersListParameters(); 
       var runningCon =  await _dockerClient.Containers.ListContainersAsync(listParam);
       foreach(var container in runningCon)
       {
            _log.LogInformation($"{container.Names.First()} : {container.State} : {container.Labels} : {container.Image}");
       }
    }

    private async Task StartContainer(){
        var param = new CreateContainerParameters(){
            Image = "",
            Name = ""
        };
        await _dockerClient.Containers.CreateContainerAsync(param);
    }
}
