using Gml.Agent.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Gml.Agent.Core.Helpers;

public static class ServiceCollectionHelper
{
    public static IServiceCollection ConfigureSdk(this IServiceCollection services)
    {
        services
            .AddSingleton<IAgentSdk, AgentSdk>();
        
        return services;
    }

    public static IHost ConfigureSdk(this IHost app)
    {
        app.MigrateDatabase();

        return app;
    }
}