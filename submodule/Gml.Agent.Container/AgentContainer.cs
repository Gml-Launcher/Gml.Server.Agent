using Gml.Agent.Container.Procedures;

namespace Gml.Agent.Container;

public class AgentContainer : IAgentContainer
{
    public AgentContainer(IContainerProcedures containers)
    {
        Containers = containers;
    }
    
    public IContainerProcedures Containers { get; }
}