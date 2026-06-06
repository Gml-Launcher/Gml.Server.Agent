using Docker.DotNet;

namespace Gml.Agent.Container.Procedures;

public interface IContainerProcedures
{
    Task<bool> IsContainerRunningAsync(string containerId, CancellationToken cancellationToken = default);
    Task<string> CreateAndStartMinecraftContainerAsync(
        string serverId,
        int port, 
        Dictionary<string, string> envVariables, 
        CancellationToken cancellationToken = default);

    Task StopContainerAsync(string containerId, int waitBeforeKillSeconds = 30, CancellationToken cancellationToken = default);
    Task DeleteContainerAsync(string containerId, CancellationToken cancellationToken = default);
    Task<MultiplexedStream> GetContainerLogsStreamAsync(string containerId, CancellationToken cancellationToken = default);
}