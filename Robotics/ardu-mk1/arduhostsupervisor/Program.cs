using Ardu.Common;
using arduhostsupervisor;
using arduhostsupervisor.Models;
using Docker.DotNet;
using Docker.DotNet.Models;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.AddArduHostSupervisior();
var app = builder.Build();


app.MapGet("/", async ([FromServices] DockerClient _dockerClient) =>
{
    try{
        //todo: check if running
        var listParam = new ContainersListParameters(); 
        var runningCon =  await _dockerClient.Containers.ListContainersAsync(listParam);
        var info = runningCon
            .Where(container => container.Labels.ContainsKey("Ardu"))
            .Select(con => new ArduComponentContainer{
                Name = con.Names.FirstOrDefault(),
                Image = con.Image
            });
        return Results.Ok(info.AsEnumerable());
    }
    catch{
        return Results.Problem();
    }
});

app.MapPost("/", async ([FromBody] ArduComponent container, 
[FromServices] DockerClient _dockerClient) =>
{
    try
    {
        var tags = new Dictionary<string, string>();
        tags.Add("Ardu", container.Name);
        tags.Add("ArduRestartOnExis", container.KillOnExit.ToString());
        var param = new CreateContainerParameters()
        {
            Image = container.Image,
            Name = container.Name,
            Labels = tags
        };
        await _dockerClient.Containers.CreateContainerAsync(param);
        return Results.Ok();

    }
    catch (Exception ex)
    {
        return Results.BadRequest();
    }
});

app.Run();
