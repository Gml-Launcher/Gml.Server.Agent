using Gml.Agent.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Gml.Agent.Core.Helpers;

public static class DatabaseHelper
{
    public static IServiceCollection ConfigureDatabase(this IServiceCollection services)
    {
        services
            .AddDbContextFactory<DatabaseContext>();
        
        return services;
    }

    public static IHost MigrateDatabase(this IHost app)
    {
        var dbContextFactory = app.Services.GetRequiredService<IDbContextFactory<DatabaseContext>>();
        var db = dbContextFactory.CreateDbContext();

        if (db.Database.GetPendingMigrations().Any())
            db.Database.Migrate();
        
        return app;
    }
}