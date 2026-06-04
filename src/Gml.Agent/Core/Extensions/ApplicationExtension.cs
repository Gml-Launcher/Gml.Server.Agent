namespace Gml.Agent.Core.Extensions;

public static class ApplicationExtension
{
    public static HostApplicationBuilder RegisterServices(this HostApplicationBuilder builder)
    {
        return builder;
    }

    public static IHost RegisterServices(this IHost app)
    {
        return app;
    }
}