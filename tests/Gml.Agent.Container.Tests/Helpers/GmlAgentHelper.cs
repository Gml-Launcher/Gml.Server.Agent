using Gml.Agent.Container.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Gml.Agent.Container.Tests.Helpers;

public class GmlAgentHelper
{
    public static async Task<(IHost host, ServiceProvider serviceProvider, IAgentContainer agentContainer)> Configure()
    {
        var host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.ConfigureAgentContainer();
            })
            .ConfigureLogging(logging =>
            {
                logging.AddConsole();
            })
            .Build();
        
        await host.StartAsync();
        
        var serviceProvider = (ServiceProvider)host.Services;
        var agentContainer = serviceProvider.GetRequiredService<IAgentContainer>();
        
        return (host, serviceProvider, agentContainer);
    }
}