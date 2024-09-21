using arduhostsupervisor.Workers;
using Docker.DotNet;

namespace arduhostsupervisor;

public static class ServiceExtensions
{
    public static WebApplicationBuilder AddArduHostSupervisior(this WebApplicationBuilder builder){

        var dockerClient = new DockerClientConfiguration(new Uri("unix:///var/run/docker.sock"))
            .CreateClient();

        builder.Services.AddSingleton<DockerClient>(dockerClient);
        builder.Services.AddHostedService<Dockerworker>();
        return builder;
    }
}
