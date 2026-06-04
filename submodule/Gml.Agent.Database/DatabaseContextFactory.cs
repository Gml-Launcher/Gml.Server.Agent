using Microsoft.EntityFrameworkCore;

namespace Gml.Agent.Database;

public class DatabaseContextFactory : IDbContextFactory<DatabaseContext>
{
    public DatabaseContext CreateDbContext()
    {
        var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING") ?? "";

        var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
        
        optionsBuilder.UseSqlite(connectionString);
        
        return new DatabaseContext(new DbContextOptions<DatabaseContext>());
    }
}