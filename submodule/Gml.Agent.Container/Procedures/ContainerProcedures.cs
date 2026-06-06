using System.Runtime.InteropServices;
using Docker.DotNet;
using Docker.DotNet.Models;
using Gml.Agent.Container.Factories;

namespace Gml.Agent.Container.Procedures;

public class ContainerProcedures : IContainerProcedures
{
    private readonly IDockerClient _dockerClient;
    private const string MinecraftImage = "itzg/minecraft-server:latest";

    public ContainerProcedures(DockerClientFactory dockerClientFactory)
    {
        _dockerClient = dockerClientFactory.GetClient();
    }

    public async Task<bool> IsContainerRunningAsync(string containerId, CancellationToken cancellationToken = default)
    {
        try
        {
            var inspect = await _dockerClient.Containers.InspectContainerAsync(containerId, cancellationToken);
            return inspect.State.Running;
        }
        catch (DockerContainerNotFoundException)
        {
            return false;
        }
    }

    public async Task<string> CreateAndStartMinecraftContainerAsync(
        string serverId, 
        int port, 
        Dictionary<string, string> envVariables, 
        CancellationToken cancellationToken = default)
    {
        string technicalName = $"gml-server-{serverId}";

        var containers = await _dockerClient.Containers.ListContainersAsync(new ContainersListParameters { All = true }, cancellationToken);
        var existing = containers.FirstOrDefault(c => c.Names.Contains($"/{technicalName}"));

        if (existing != null)
        {
            if (existing.State != "running")
            {
                await _dockerClient.Containers.StartContainerAsync(existing.ID, new ContainerStartParameters(), cancellationToken);
            }
            return existing.ID;
        }

        string hostDataPath;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            string baseWinPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "GmlAgent", "servers");
            hostDataPath = Path.Combine(baseWinPath, serverId);
            
            hostDataPath = Path.GetFullPath(hostDataPath).Replace("\\", "/");
        }
        else
        {
            hostDataPath = $"/var/gml/servers/{serverId}";
        }

        if (!Directory.Exists(hostDataPath))
        {
            Directory.CreateDirectory(hostDataPath);
        }

        var defaultEnv = new Dictionary<string, string>
        {
            { "EULA", "TRUE" },
            { "TYPE", "PAPER" },
            { "MEMORY", "2G" }
        };

        foreach (var (key, value) in envVariables)
        {
            defaultEnv[key] = value;
        }

        var createParams = new CreateContainerParameters
        {
            Image = MinecraftImage,
            Name = technicalName,
            Env = defaultEnv.Select(kvp => $"{kvp.Key}={kvp.Value}").ToList(),
            HostConfig = new HostConfig
            {
                PortBindings = new Dictionary<string, IList<PortBinding>>
                {
                    { "25565/tcp", new List<PortBinding> { new() { HostPort = port.ToString() } } }
                },
                Binds = new List<string>
                {
                    $"{hostDataPath}:/data"
                },
                RestartPolicy = new RestartPolicy { Name = RestartPolicyKind.UnlessStopped }
            }
        };

        var response = await _dockerClient.Containers.CreateContainerAsync(createParams, cancellationToken);
        await _dockerClient.Containers.StartContainerAsync(response.ID, new ContainerStartParameters(), cancellationToken);
        
        return response.ID;
    }

    public async Task StopContainerAsync(string containerId, int waitBeforeKillSeconds = 30, CancellationToken cancellationToken = default)
    {
        await _dockerClient.Containers.StopContainerAsync(containerId, new ContainerStopParameters
        {
            WaitBeforeKillSeconds = (uint)waitBeforeKillSeconds
        }, cancellationToken);
    }

    public async Task DeleteContainerAsync(string containerId, CancellationToken cancellationToken = default)
    {
        await _dockerClient.Containers.RemoveContainerAsync(containerId, new ContainerRemoveParameters { Force = true }, cancellationToken);
    }

    public async Task<MultiplexedStream> GetContainerLogsStreamAsync(string containerId, CancellationToken cancellationToken = default)
    {
        var logParameters = new ContainerLogsParameters
        {
            ShowStdout = true,
            ShowStderr = true,
            Follow = true,
            Tail = "50"
        };

        MultiplexedStream multiplexedStream = await _dockerClient.Containers.GetContainerLogsAsync(
            containerId, 
            false, 
            logParameters, 
            cancellationToken);

        return multiplexedStream;
    }
}