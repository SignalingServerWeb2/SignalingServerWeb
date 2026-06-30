using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR(options =>
{
    // Allow larger image frames (10 MB)
    options.MaximumReceiveMessageSize = 10 * 1024 * 1024;
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin();
    });
});

var app = builder.Build();

app.UseRouting();

app.UseCors();

app.MapGet("/", () => "SignalR Server Running");

app.MapHub<SignalHub>("/signal");

app.Run();
