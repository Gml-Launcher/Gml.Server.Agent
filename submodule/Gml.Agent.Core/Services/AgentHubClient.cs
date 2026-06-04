using Gml.Agent.Interfaces.HubClient;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace Gml.Agent.Core.Services;

public class AgentHubClient : IAgentHubClient
{
    private readonly HubConnection _connection;
    private readonly ILogger<AgentHubClient> _logger;

    public event Func<string, string, Task>? OnJobReceived;
    
    public AgentHubClient(string hubUrl, ILogger<AgentHubClient> logger)
    {
        _logger = logger;

        _connection = new HubConnectionBuilder()
            .WithUrl($"{hubUrl}/ws/profiles/servers", options =>
            {
                var token = Environment.GetEnvironmentVariable("GML_AGENT_TOKEN");
                
                if (!string.IsNullOrEmpty(token))   
                    options.AccessTokenProvider = () => Task.FromResult(token)!;
            })
            .WithAutomaticReconnect([TimeSpan.Zero, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(30)])
            .Build();

        RegisterHubHandlers();
        RegisterLifecycleEvents();
    }
    
    private void RegisterHubHandlers()
    {
        _connection.On<string, string>("ExecuteTask", async (taskName, jsonData) =>
        {
            if (OnJobReceived != null)
                await OnJobReceived.Invoke(taskName, jsonData);
        });
    }

    private void RegisterLifecycleEvents()
    {
        _connection.Closed += async error =>
        {
            _logger.LogWarning($"Соединение закрыто: {error?.Message}. Ожидание переподключения...");
            await Task.CompletedTask;
        };

        _connection.Reconnecting += error =>
        {
            _logger.LogWarning($"Потеряно соединение. Переподключение: {error?.Message}");
            return Task.CompletedTask;
        };

        _connection.Reconnected += connectionId =>
        {
            _logger.LogInformation($"Соединение успешно восстановлено. ID: {connectionId}");
            return Task.CompletedTask;
        };
    }
    
    public async Task ConnectWithRetryAsync(CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("Попытка подключения к SignalR хабу...");
                await _connection.StartAsync(cancellationToken);
                _logger.LogInformation("Агент успешно подключен к API");
                return;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка подключения: {ex.Message}. Повтор через 5 секунд...");
                await Task.Delay(5000, cancellationToken);
            }
        }
    }
}