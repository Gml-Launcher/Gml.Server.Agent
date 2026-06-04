namespace Gml.Agent.Interfaces.HubClient;

public interface IAgentHubClient
{
    event Func<string, string, Task>? OnJobReceived;
    Task ConnectWithRetryAsync(CancellationToken cancellationToken = default);
}