using System.Runtime.InteropServices;
using Docker.DotNet;

namespace Gml.Agent.Container.Factories;

public class DockerClientFactory : IDisposable
{
    private readonly IDockerClient _dockerClient;
    private bool _disposed;
    
    public DockerClientFactory()
    {
        var dockerUri = ResolveDockerUri();

        var configuration = new DockerClientConfiguration(new Uri(dockerUri));
        _dockerClient = configuration.CreateClient();
    }
    
    public IDockerClient GetClient() => _disposed ? throw new ObjectDisposedException(nameof(DockerClientFactory)) : _dockerClient;
    
    private string ResolveDockerUri()
    {
        var envHost = Environment.GetEnvironmentVariable("DOCKER_HOST");
        
        if (!string.IsNullOrEmpty(envHost))
            return envHost;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return "npipe://./pipe/docker_engine";

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            return "unix:///var/run/docker.sock";

        throw new PlatformNotSupportedException("Не удалось автоматически определить операционную систему для Docker.");
    }
    
    public void Dispose()
    {
        if (_disposed)
            return;
        
        _dockerClient.Dispose();
        _disposed = true;
    }
}