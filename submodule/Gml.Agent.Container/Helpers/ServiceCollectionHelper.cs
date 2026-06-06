using Gml.Agent.Container.Factories;
using Microsoft.Extensions.DependencyInjection;

namespace Gml.Agent.Container.Helpers;

public static class ServiceCollectionHelper
{
    public static IServiceCollection ConfigureAgentContainer(this IServiceCollection services)
    {
        services.AddSingleton<DockerClientFactory>();
        
        return services;
    }   
}