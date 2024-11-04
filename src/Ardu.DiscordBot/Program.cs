using Ardu.DiscordBot;
using Discord.WebSocket;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//discord config
builder.Services.AddHostedService<DiscordWorker>();
var socketConfig = new DiscordSocketConfig{
 
};
builder.Services.AddSingleton<DiscordSocketConfig>(socketConfig);
builder.Services.AddSingleton<DiscordSocketClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.Run();