using Ardu.Common;
using Ardu.Common.Services;
using arduhostsupervisor;
using Docker.DotNet;
using Docker.DotNet.Models;
using Microsoft.AspNetCore.Mvc;

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
app.Run();