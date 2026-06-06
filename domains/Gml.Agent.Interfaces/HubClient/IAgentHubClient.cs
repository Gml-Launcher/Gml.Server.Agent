namespace Gml.Agent.Interfaces.HubClient;

public interface IAgentHubClient
{
    event Func<string, string, string, Task>? OnJobReceived;
    Task ConnectWithRetryAsync(CancellationToken cancellationToken = default);
    Task SendDataAsync(string jobId, string type, string data, CancellationToken cancellationToken = default);
}