using System;
using ShowsService.Services;
using FeatureHubSDK;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(80);
});


// Setup FeatureHub logging
FeatureLogging.DebugLogger += (sender, s) => Console.WriteLine("DEBUG: " + s);
FeatureLogging.TraceLogger += (sender, s) => Console.WriteLine("TRACE: " + s);
FeatureLogging.InfoLogger += (sender, s) => Console.WriteLine("INFO: " + s);
FeatureLogging.ErrorLogger += (sender, s) => Console.WriteLine("ERROR: " + s);

// Create FeatureHub config
var config = new EdgeFeatureHubConfig("http://featurehub:8085", "3a878120-2cf4-4d97-bfd6-2170566480a5/4NlHcJXFrK52jSvbVZenQrH3yVxrBmdMddCRLqeg");

// Initialize the config
config.Init();

// Create context and wait for it to be ready
var context = await config.NewContext().Build();

// Register both config and context with DI
builder.Services.AddSingleton<IFeatureHubConfig>(config);
builder.Services.AddSingleton<IClientContext>(context);
builder.Services.AddScoped<ShowService>();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();
app.Run();