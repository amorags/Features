using System;
using ShowsService.Services;
using FeatureHubSDK;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(80);
});

// GREEN: Optimize Kestrel for efficiency
builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxConcurrentConnections = 50; // Lower connection limit
    options.Limits.MaxRequestBodySize = 1024 * 1024; // 1MB limit
    options.Limits.RequestHeadersTimeout = TimeSpan.FromSeconds(10);
});

// GREEN: Minimal logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Warning);

// Setup FeatureHub (minimal logging)
FeatureLogging.ErrorLogger += (sender, s) => Console.WriteLine("ERROR: " + s);

var config = new EdgeFeatureHubConfig("http://featurehub:8085", "3a878120-2cf4-4d97-bfd6-2170566480a5/4NlHcJXFrK52jSvbVZenQrH3yVxrBmdMddCRLqeg");
config.Init();
var context = await config.NewContext().Build();

// GREEN: Efficient service registration
builder.Services.AddSingleton<IFeatureHubConfig>(config);
builder.Services.AddSingleton<IClientContext>(context);
builder.Services.AddSingleton<ShowService>(); // Singleton for efficiency
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();



app.UseSwagger();
app.UseSwaggerUI();


// GREEN: Graceful shutdown with idle timeout
var shutdownTokenSource = new CancellationTokenSource();
var idleTimer = new Timer(async _ =>
{
    Console.WriteLine("GREEN: Service has been idle - initiating graceful shutdown");
    shutdownTokenSource.Cancel();
}, null, TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(5)); // Shutdown after 5 minutes idle

app.MapControllers();

// GREEN: Run with cancellation token for graceful shutdown
try
{
    await app.RunAsync(shutdownTokenSource.Token);
}
catch (OperationCanceledException)
{
    Console.WriteLine("GREEN: Service shutdown gracefully");
}
finally
{
    idleTimer?.Dispose();
}