using Gml.Agent.Interfaces.Container;

namespace Gml.Agent.Domains.Container;

public class Container : IContainer
{
    public Guid Id { get; set; }
    public string ServerName { get; set; }
    public string ContainerId { get; set; }
    public bool IsRunning { get; set; }
    public int AssignedPort { get; set; }
    public string LastKnownStatus { get; set; } = "Created";
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}