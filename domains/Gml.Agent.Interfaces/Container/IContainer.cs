namespace Gml.Agent.Interfaces.Container;

public interface IContainer
{
    public Guid Id { get; set; }
    public string ServerName { get; set; }
    public string ContainerId { get; set; }
    public bool IsRunning { get; set; }
    public int AssignedPort { get; set; }
    public string LastKnownStatus { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}