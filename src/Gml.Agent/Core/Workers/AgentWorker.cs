using System.Threading.Channels;
using Gml.Agent.Interfaces;
using Gml.Agent.Interfaces.HubClient;

namespace Gml.Agent.Core.Workers;

public class AgentWorker : BackgroundService
{
    private readonly ILogger<AgentWorker> _logger;
    private readonly IAgentHubClient _hubClient;
    private readonly IAgentSdk _sdk;
    
    private readonly Channel<string> _taskQueue = Channel.CreateUnbounded<string>();

    public AgentWorker(IAgentSdk sdk, IAgentHubClient hubClient, ILogger<AgentWorker> logger)
    {
        _sdk = sdk;
        _hubClient = hubClient;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        
    }
}