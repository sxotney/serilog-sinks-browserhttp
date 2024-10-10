using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Serilog;
using Serilog.Core;
using Serilog.Extensions.Logging;

//take note of the setup below for dotnet 8

Log.Information("Hello, browser!");

var builder = WebAssemblyHostBuilder.CreateDefault(args);

var levelSwitch = new LoggingLevelSwitch();
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.ControlledBy(levelSwitch)
    .Enrich.WithProperty("InstanceId", Guid.NewGuid().ToString("n"))
    .WriteTo.BrowserConsole()
    .WriteTo.BrowserHttp(endpointUrl: $"{builder.HostEnvironment.BaseAddress}ingest", controlLevelSwitch: levelSwitch)
    .CreateLogger();


builder.Services.AddScoped(sp=>new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
var app = builder.Build();
    
await app.RunAsync();
