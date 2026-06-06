using Gml.Agent.Container.Tests.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Gml.Agent.Container.Tests;

public class Tests
{
    private IAgentContainer _agentContainer;
    private ServiceProvider _serviceProvider;
    private IHost _host;
    
    [OneTimeSetUp]
    public async Task Setup()
    {
        (_host, _serviceProvider, _agentContainer) = await GmlAgentHelper.Configure();
    }
    
    [OneTimeTearDown]
    public void TearDown()
    {
        
    }

    [Test]
    public void Test1()
    {
        Assert.Pass();
    }
}