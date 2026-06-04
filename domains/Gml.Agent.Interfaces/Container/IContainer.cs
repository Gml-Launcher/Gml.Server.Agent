namespace Gml.Agent.Interfaces.Container;

public interface IContainer
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string ContainerId { get; set; }
    public bool IsRunning { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}