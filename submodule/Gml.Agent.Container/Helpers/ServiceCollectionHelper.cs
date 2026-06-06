using Gml.Agent.Container.Factories;
using Gml.Agent.Container.Procedures;
using Microsoft.Extensions.DependencyInjection;

namespace Gml.Agent.Container.Helpers;

public static class ServiceCollectionHelper
{
    public static IServiceCollection ConfigureAgentContainer(this IServiceCollection services)
    {
        services
            .AddSingleton<DockerClientFactory>()
            .AddSingleton<IContainerProcedures, ContainerProcedures>()
            .AddSingleton<IAgentContainer, AgentContainer>();
        
        return services;
    }   
}