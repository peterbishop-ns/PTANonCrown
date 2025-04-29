using Microsoft.EntityFrameworkCore;
using PTANonCrown.Data.Models;

namespace PTANonCrown.Data.Context;

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
    public DbSet<Treatment>Treatments { get; set; }
    public DbSet<PlotTreatment> PlotTreatments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //string dbPath = Path.Combine(FileSystem.AppDataDirectory, "app.db");
        string dbPath = Path.Combine("C://temp", "app.db");
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


        modelBuilder.Entity<Treatment>().HasData(
                  new Treatment { ID = 1, Name = "Planting" },
                  new Treatment { ID = 2, Name = "Pre-commercial thinning" },
                  new Treatment { ID = 3, Name = "Commercial thinning" },
                  new Treatment { ID = 4, Name = "Strip shelterwood" },
                  new Treatment { ID = 5, Name = "Patch shelterwood" },
                  new Treatment { ID = 6, Name = "Uniform shelterwood" },
                  new Treatment { ID = 7, Name = "Gap irregular shelterwood" },
                  new Treatment { ID = 8, Name = "Continuous cover irregular shelterwood" },
                  new Treatment { ID = 9, Name = "Single tree selection" },
                  new Treatment { ID = 10, Name = "Group selection" },
                  new Treatment { ID = 11, Name = "Partial harvest (unknown)" }
              ); 
        modelBuilder.Entity<PlotTreatment>()
            .HasKey(sc => new { sc.PlotId, sc.TreatmentId });

        modelBuilder.Entity<PlotTreatment>()
            .HasOne(pt => pt.Plot)
            .WithMany(p => p.PlotTreatments)
            .HasForeignKey(pt => pt.PlotId);

        modelBuilder.Entity<PlotTreatment>()
            .HasOne(pt => pt.Treatment)
            .WithMany(t => t.PlotTreatments)
            .HasForeignKey(pt => pt.TreatmentId);


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