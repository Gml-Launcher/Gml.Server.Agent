using Docker.DotNet;
using Docker.DotNet.Models;
using Gml.Agent.Container.Factories;
using Gml.Agent.Container.Procedures;

namespace Gml.Agent.Container;

public class AgentContainer : IAgentContainer
{
    private readonly IDockerClient _dockerClient;
    private const string MinecraftImage = "itzg/minecraft-server";
    private const string ImageTag = "latest";
    
    public AgentContainer(IContainerProcedures containers, DockerClientFactory dockerClientFactory)
    {
        Containers = containers;
        
        _dockerClient = dockerClientFactory.GetClient();
    }
    
    public IContainerProcedures Containers { get; }
    
    public bool IsInitialize { get; private set; }
    
    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            Console.WriteLine("Проверка наличия Docker-образа Minecraft...");
            bool imageExists = await CheckImageExistsAsync($"{MinecraftImage}:{ImageTag}", cancellationToken);

            if (!imageExists)
            {
                Console.WriteLine($"Образ {MinecraftImage}:{ImageTag} не найден. Начинаю загрузку (Pull)...");
                await PullMinecraftImageAsync(cancellationToken);
                Console.WriteLine("Образ успешно загружен.");
            }
            else
            {
                Console.WriteLine("Образ Minecraft уже присутствует локально.");
            }

            IsInitialize = true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при инициализации Docker-окружения: {ex.Message}");
            throw;
        }
    }

    private async Task<bool> CheckImageExistsAsync(string fullImageName, CancellationToken cancellationToken)
    {
        var images = await _dockerClient.Images.ListImagesAsync(new ImagesListParameters
        {
            All = false
        }, cancellationToken);

        foreach (var img in images)
        {
            if (img.RepoTags != null && img.RepoTags.Contains(fullImageName))
            {
                return true;
            }
        }

        return false;
    }

    private async Task PullMinecraftImageAsync(CancellationToken cancellationToken)
    {
        var pullParameters = new ImagesCreateParameters
        {
            FromImage = MinecraftImage,
            Tag = ImageTag
        };

        var progress = new Progress<JSONMessage>(message =>
        {
            if (!string.IsNullOrEmpty(message.Status))
            {
                Console.WriteLine($"[Docker Pull] {message.ID} -> {message.Status} {message.ProgressMessage}");
            }
        });

        await _dockerClient.Images.CreateImageAsync(
            pullParameters, 
            new AuthConfig(),
            progress, 
            cancellationToken
        );
    }
}