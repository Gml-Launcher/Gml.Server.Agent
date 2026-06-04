using Gml.Agent.Core.Extensions;

Host
    .CreateApplicationBuilder(args)
    .RegisterServices()
    .Build()
    .RegisterServices()
    .Run();
