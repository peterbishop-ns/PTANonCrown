using Microsoft.EntityFrameworkCore;
using PTANonCrown.Models;

namespace PTANonCrown.Context;

public class AppDbContext : DbContext
{
    public AppDbContext()
    {

    }

    public DbSet<CoarseWoody> CoarseWoodys { get; set; }
    public DbSet<Plot> Plots { get; set; }
    public DbSet<Stand> Stands { get; set; }
    public DbSet<TreeDead> TreesDead { get; set; }
    public DbSet<TreeLive> TreesLive { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "app.db");
        optionsBuilder.UseSqlite($"Data Source={dbPath}");
    }
}