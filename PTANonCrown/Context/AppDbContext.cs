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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Stand → Plots
        modelBuilder.Entity<Stand>()
            .HasMany(s => s.Plots)
            .WithOne(p => p.Stand)
            .HasForeignKey(p => p.StandID)
            .OnDelete(DeleteBehavior.Cascade);  // Optional: cascade delete plots when stand is deleted

        // Plot → PlotTreeLive
        modelBuilder.Entity<Plot>()
            .HasMany(p => p.PlotTreeLive)
            .WithOne(t => t.Plot)
            .HasForeignKey(t => t.PlotID)
            .OnDelete(DeleteBehavior.Cascade);  // Optional: cascade delete trees when plot is deleted

        // Plot → PlotCoarseWoody
        modelBuilder.Entity<Plot>()
            .HasMany(p => p.PlotCoarseWoody)
            .WithOne(t => t.Plot)
            .HasForeignKey(t => t.PlotID)
            .OnDelete(DeleteBehavior.Cascade);  // Optional: cascade delete trees when plot is deleted

        // Plot → PlotTreeDead
        modelBuilder.Entity<Plot>()
            .HasMany(p => p.PlotTreeDead)
            .WithOne(t => t.Plot)
            .HasForeignKey(t => t.PlotID)
            .OnDelete(DeleteBehavior.Cascade);  // Optional: cascade delete trees when plot is deleted
    }
}