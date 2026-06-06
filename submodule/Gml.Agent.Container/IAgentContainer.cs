using Gml.Agent.Container.Procedures;

namespace Gml.Agent.Container;

public interface IAgentContainer
{
    IContainerProcedures Containers { get; }
}