using Azure_Connection_Sample.Architecture.Console;
using Azure_Connection_Sample.Architecture.Console.Extensions;
using Azure_Connection_Sample.Architecture.Domain_Layer.Aggregate;
using Azure_Connection_Sample.Architecture.Domain_Layer.Models;
using Azure_Connection_Sample.Architecture.Service_Layer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using System.Reflection;

var start = DateTime.UtcNow;
var end = DateTime.UtcNow.AddMinutes(60);
try
{
    var application = WebApplication.CreateBuilder(args);
    application.Host.RegisterLogger();

    Log.Information($"┌{new string('─', 100)}┐");
    Log.Information($"│{$"Starting Application {start:MMMM dd, yyyy hh:mm:ss}".Center()}│");
    Log.Information($"│{"Configuring Configuration Manager and IServiceCollection".Left()}│");

    application.Configuration.Build("application-settings.json");
    application.Services.RegisterDependencies();

    Log.Information($"│{"Serialize Azure Information...".Left()}│");
    application.Services.Configure<AzureModel>(application.Configuration.GetSection("Azure"));

    Log.Information($"│{"Building Services...".Left()}│");
    using var services = application.Services.BuildServiceProvider();

    var azure = services.GetService<IOptions<AzureModel>>();
    var factory = services.GetService<IApiFacadeFactory>();

    if (azure != null && factory != null)
    {
        Log.Information($"│{"Retrieve Required Services...".Left()}│");
        Log.Information($"│{"Create Simulated Data...".Left()}│");

        while (start <= end)
        {
            Log.Information($"│{"Build Batch Of Simulated Data Point...".Left()}│");

            var collection = new List<MockAggregate>();
            for (var index = 0; index < 1000; index++)
                collection.Add(new MockAggregate());

            using var api = factory.Connect();
            var token = api.GenerateToken(azure.Value.EventHub.Endpoint, azure.Value.EventHub.Authorization, azure.Value.EventHub.Key);

            Log.Information($"│{"Send Single Simulated Data Point...".Left()}│");
            api.Send<MockAggregate>(new Uri(azure.Value.EventHub.Endpoint), new MockAggregate(), token);

            Log.Information($"│{"Send Batch Of Simulated Data Points...".Left()}│");
            api.SendBatch<IList<MockAggregate>>(new Uri(azure.Value.EventHub.Endpoint), collection, token);

            Log.Information($"│{"Completed Simulated Data Transfer...".Left()}│");

            Thread.Sleep(600000);
            start = DateTime.UtcNow;
        }
    }

    Log.Information($"│{$"Stopping Application {DateTime.UtcNow:MMMM dd, yyyy hh:mm:ss}".Center()}│");
    Log.Information($"└{new string('─', 100)}┘");
}

catch (Exception exception)
{
    exception.Decorate(Log.Logger);

    Log.Information($"│{$"Application Stopped Abruptly {DateTime.UtcNow:MMMM dd, yyyy hh:mm:ss}".Center()}│");
    Log.Information($"└{new string('─', 100)}┘");
    Environment.Exit(1);
}