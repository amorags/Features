// See https://aka.ms/new-console-template for more information

using FeatureHubSDK;

FeatureLogging.DebugLogger += (sender, s) => Console.WriteLine("DEBUG: " + s); 
FeatureLogging.TraceLogger += (sender, s) => Console.WriteLine("TRACE: " + s); 
FeatureLogging.InfoLogger += (sender, s) => Console.WriteLine("INFO: " + s); 
FeatureLogging.ErrorLogger += (sender, s) => Console.WriteLine("ERROR: " + s);

var config = new EdgeFeatureHubConfig("http://featurehub:8085", "<Configure you SDK key here>");

await Task.Run(() =>
{
    Console.WriteLine("Hello World");
});

