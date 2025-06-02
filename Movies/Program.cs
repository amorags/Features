using System;
using MoviesService.Services;
using FeatureHubSDK;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(80);
});


// WASTEFUL: Maximum resource usage
builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxConcurrentConnections = 1000; // High connection limit
    options.Limits.MaxRequestBodySize = 100 * 1024 * 1024; // 100MB limit
    options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(5);
});

// WASTEFUL: Excessive logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Trace);

// WASTEFUL: All FeatureHub logging
FeatureLogging.DebugLogger += (sender, s) => Console.WriteLine("DEBUG: " + s);
FeatureLogging.TraceLogger += (sender, s) => Console.WriteLine("TRACE: " + s);
FeatureLogging.InfoLogger += (sender, s) => Console.WriteLine("INFO: " + s);
FeatureLogging.ErrorLogger += (sender, s) => Console.WriteLine("ERROR: " + s);

var config = new EdgeFeatureHubConfig("http://featurehub:8085", "5e57b377-5af8-4733-a332-28c14fe17c5c/GNUFChY4fxFmiSnAR2VXBzHIzAlbHo4m6sItvBeK");
config.Init();
var context = await config.NewContext().Build();

// WASTEFUL: Create multiple instances
builder.Services.AddSingleton<IFeatureHubConfig>(config);
builder.Services.AddSingleton<IClientContext>(context);
builder.Services.AddScoped<MovieService>(); // New instance per request// Additional wasteful service
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// WASTEFUL: Always enable Swagger
app.UseSwagger();
app.UseSwaggerUI();

// WASTEFUL: Background tasks that consume resources
var backgroundTimer = new Timer(async _ =>
{
    Console.WriteLine("WASTEFUL: Running unnecessary background processing...");
    // Simulate wasteful computation
    var random = new Random();
    var wasteArray = new int[10000];
    for (int i = 0; i < wasteArray.Length; i++)
    {
        wasteArray[i] = random.Next() * random.Next();
    }
    Array.Sort(wasteArray); // Unnecessary sorting
}, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));

app.MapControllers();
await app.RunAsync();