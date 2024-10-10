using Ardu.Common.Interfaces;
using Ardu.Common.Services;
using arduhostsupervisor.Models;
using arduhostsupervisor.Services;
using arduhostsupervisor.Workers;
using Docker.DotNet;

namespace arduhostsupervisor;

public static class ServiceExtensions
{
    public static WebApplicationBuilder AddArduHostSupervisior(this WebApplicationBuilder builder){

        builder.Services.AddOptions<SupervisorConfig>()
            .Bind(builder.Configuration.GetSection("Config"));
        var dockerClient = new DockerClientConfiguration(new Uri("unix:///var/run/docker.sock"))
            .CreateClient();

        builder.Services.AddSingleton<DockerClient>(dockerClient);
        builder.Services.AddHostedService<Dockerworker>();
        builder.Services.AddSingleton<IComponentContainerService, ComponentDockerContainerService>();
        return builder;
    }
}
