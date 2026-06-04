using System.ComponentModel;
using Microsoft.EntityFrameworkCore;

namespace Gml.Agent.Database;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    public DbSet<Container> Containers { get; set; }
}