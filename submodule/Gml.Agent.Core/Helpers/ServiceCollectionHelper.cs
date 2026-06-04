using Gml.Agent.Core.Docker;
using Gml.Agent.Core.Services;
using Gml.Agent.Interfaces;
using Gml.Agent.Interfaces.HubClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Gml.Agent.Core.Helpers;

public static class ServiceCollectionHelper
{
    public static IServiceCollection ConfigureSdk(this IServiceCollection services)
    {
        services
            .ConfigureDatabase()
            .AddSingleton<DockerClientFactory>()
            .AddSingleton<IAgentSdk, AgentSdk>()
            .AddSingleton<IAgentHubClient, AgentHubClient>();
        
        return services;
    }

    public static IHost ConfigureSdk(this IHost app)
    {
        app.MigrateDatabase();

        return app;
    }
}