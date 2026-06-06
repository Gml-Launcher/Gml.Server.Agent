using System.Text;
using Gml.Agent.Container.Tests.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Gml.Agent.Container.Tests;

[TestFixture]
public class ContainerIntegrationTests
{
    private IAgentContainer _agentContainer;
    private ServiceProvider _serviceProvider;
    private IHost _host;
    
    private readonly string _testServerId = $"test-mc-{Guid.NewGuid().ToString()[..8]}";
    private readonly int _testPort = 25565;

    [OneTimeSetUp]
    public async Task Setup()
    {
        (_host, _serviceProvider, _agentContainer) = await GmlAgentHelper.Configure();
        
        await _agentContainer.InitializeAsync();
    }
    
    [OneTimeTearDown]
    public async Task TearDown()
    {
        try
        {
            await _agentContainer.Containers.DeleteContainerAsync(_testServerId);
        }
        catch
        {
        }

        _serviceProvider.Dispose();
        _host.Dispose();
    }

    [Test]
    [Order(1)]
    [Category("0. Системный")]
    public void Initialize_ShouldSetFlagToTrue()
    {
        Assert.That(_agentContainer.IsInitialize, Is.True);
    }

    [Test]
    [Order(2)]
    [Category("1. Контейнеры")]
    public async Task CreateAndStartMinecraftContainer_ShouldCreateAndRunContainer()
    {
        // Arrange
        var env = new Dictionary<string, string>
        {
            { "TYPE", "PAPER" },
            { "VERSION", "1.20.4" },
            { "ONLINE_MODE", "FALSE" }
        };

        // Act
        string dockerId = await _agentContainer.Containers.CreateAndStartMinecraftContainerAsync(_testServerId, _testPort, env);

        // Assert
        Assert.That(dockerId, Is.Not.Null.Or.Empty);
        
        bool isRunning = await _agentContainer.Containers.IsContainerRunningAsync(dockerId);
        Assert.That(isRunning, Is.True);
    }

    [Test]
    [Order(3)]
    [Category("1. Контейнеры")]
    public async Task GetContainerLogsStream_ShouldReturnValidLogStream()
    {
        // Arrange
        string dockerId = await _agentContainer.Containers.CreateAndStartMinecraftContainerAsync(_testServerId, _testPort, new());

        // Act
        using var multiplexedStream = await _agentContainer.Containers.GetContainerLogsStreamAsync(dockerId);
        
        var buffer = new byte[1024];
        var readResult = await multiplexedStream.ReadOutputAsync(buffer, 0, buffer.Length, new CancellationTokenSource(TimeSpan.FromSeconds(5)).Token);

        // Assert
        Assert.That(readResult.EOF, Is.False);
        Assert.That(readResult.Count, Is.GreaterThan(0));

        string logText = Encoding.UTF8.GetString(buffer, 0, readResult.Count);
        Console.WriteLine($"[Test Log Output]: {logText}");
    }

    [Test]
    [Order(4)]
    [Category("1. Контейнеры")]
    public async Task StopContainer_ShouldStopRunningContainer()
    {
        // Arrange
        string dockerId = await _agentContainer.Containers.CreateAndStartMinecraftContainerAsync(_testServerId, _testPort, new());

        // Act
        await _agentContainer.Containers.StopContainerAsync(dockerId, waitBeforeKillSeconds: 15);

        // Assert
        bool isRunning = await _agentContainer.Containers.IsContainerRunningAsync(dockerId);
        Assert.That(isRunning, Is.False, "Контейнер должен был остановиться.");
    }

    [Test]
    [Order(5)]
    [Category("1. Контейнеры")]
    public async Task DeleteContainer_ShouldRemoveContainerFromDocker()
    {
        // Arrange
        string dockerId = await _agentContainer.Containers.CreateAndStartMinecraftContainerAsync(_testServerId, _testPort, new());

        // Act
        await _agentContainer.Containers.DeleteContainerAsync(dockerId);

        // Assert
        bool isRunning = await _agentContainer.Containers.IsContainerRunningAsync(dockerId);
        Assert.That(isRunning, Is.False);
    }
}