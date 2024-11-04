
using Ardu.Common.Services;
using arduhostsupervisor.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace arduhostsupervisor.Workers;

public class Dockerworker(IComponentContainerService componentService, ILogger<Dockerworker> logger,
IOptions<SupervisorConfig> config) : BackgroundService
{
    private readonly IComponentContainerService _componentService = componentService; 
    private readonly ILogger _log = logger;

    private readonly SupervisorConfig _config = config.Value;
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.Register(async () => await ContainerShutDown());
        await ContainerStartup();
        while(!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(500);
        }
    }

    private async Task ContainerStartup(){
        if(_config.RequiredComponents != null)
        {
            foreach(var component in _config.RequiredComponents!){
                try
                {
                    await _componentService.StartComponent(component);
                }
                catch(Exception ex){
                    _log.LogError(ex, $"Failed to start required container: {component.Name}");
                }
            }
        }
        else{
            _log.LogInformation("No required containers were specified in config");
        }
    }

    private async Task ContainerShutDown(){
        var runningContainer = await _componentService.GetComponentsWithStatus();
        foreach(var container in runningContainer){
            try
            {
                if(container.KillOnExit)
                    await _componentService.StopComponent(container);
            }
            catch(Exception ex){
                _log.LogError(ex, $"Failed to stop container on exit: {container.Name}");

            }
        }
    }

    private async Task MonitorContainers(){
        // todo write code to see if required containers are still running 
    }

}
