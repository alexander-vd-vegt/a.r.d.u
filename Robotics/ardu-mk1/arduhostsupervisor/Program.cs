using Ardu.Common;
using Ardu.Common.Services;
using arduhostsupervisor;
using arduhostsupervisor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
builder.AddArduHostSupervisior();

//Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//swagger
app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/components", async ([FromServices] IComponentContainerService _componentService) =>
{
    try{
        var components = await _componentService.GetComponentsWithStatus();
        return Results.Ok(components);
    }
    catch{
        return Results.Problem();
    }
});

app.MapPost("/components", async ([FromBody] ArduComponent container, 
[FromServices] IComponentContainerService componentService, 
[FromServices] ILogger<Program> log) =>
{
    try
    {
        await componentService.StartComponent(container);
        return Results.Ok();

    }
    catch (Exception ex)
    {
        log.LogError(ex, $"Failed to start component from endpoint");
        return Results.BadRequest();
    }
});

//todo: maybe change this just to the component name
app.MapDelete("/components", async ([FromBody] ArduComponent container, 
[FromServices] IComponentContainerService componentService, 
[FromServices] ILogger<Program> log) =>
{
    try
    {
        await componentService.StopComponent(container);
        return Results.Ok();

    }
    catch (Exception ex)
    {
        log.LogError(ex, $"Failed to stop component from endpoint");
        return Results.BadRequest();
    }
});

app.MapPut("/components", async ([FromBody] ArduComponent container, 
[FromServices] IComponentContainerService componentService, 
[FromServices] ILogger<Program> log) =>
{
    try
    {
        await componentService.StopComponent(container);
        await componentService.StartComponent(container);
        return Results.Ok();

    }
    catch (Exception ex)
    {
        log.LogError(ex, $"Failed to stop component from endpoint");
        return Results.BadRequest();
    }
});

app.MapGet("/config", async ([FromServices] IOptions<SupervisorConfig> config) =>
{
    try{
        return Results.Ok(config.Value);
    }
    catch{
        return Results.Problem();
    }
});


app.Run();