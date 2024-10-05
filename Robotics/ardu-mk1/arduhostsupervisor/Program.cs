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
    var listParam = new ContainersListParameters(); 
    var runningCon =  await _dockerClient.Containers.ListContainersAsync(listParam);
    var info = runningCon
        .Where(container => container.Labels.ContainsKey("Ardu"))
        .Select(con => new ArduComponentContainer{
            Name = con.Names.FirstOrDefault(),
            Image = con.Image
        });
    return Results.Ok(info.AsEnumerable());
});

app.MapPost("/", async ([FromBody] ArduComponentContainer container, 
[FromServices] DockerClient _dockerClient) =>
{
    try
    {
        var param = new CreateContainerParameters()
        {
            Image = container.Image,
            Name = container.Name,
            Labels = new Dictionary<string, string(){
                new KeyValuePair<string, string>(key: "Ardu", value: "")
            }
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
