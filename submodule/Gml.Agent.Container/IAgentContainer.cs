using Gml.Agent.Container.Procedures;

namespace Gml.Agent.Container;

public interface IAgentContainer
{
    IContainerProcedures Containers { get; }
    
    Task InitializeAsync(CancellationToken cancellationToken = default);
    bool IsInitialize { get; }
}