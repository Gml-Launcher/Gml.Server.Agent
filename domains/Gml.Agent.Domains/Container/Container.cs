using Gml.Agent.Interfaces.Container;

namespace Gml.Agent.Domains.Container;

public class Container : IContainer
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string ContainerId { get; set; }
    public bool IsRunning { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}