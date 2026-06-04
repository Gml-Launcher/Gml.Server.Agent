namespace Gml.Agent.Interfaces.HubClient;

public interface IAgentHubClient
{
    event Func<string, Task>? OnJobReceived;
    Task ConnectWithRetryAsync(CancellationToken cancellationToken = default);
    Task SendStatusUpdateAsync(string status, object data, CancellationToken cancellationToken = default);
}