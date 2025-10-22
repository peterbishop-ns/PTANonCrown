using Microsoft.EntityFrameworkCore;
using PTANonCrown.Data.Models;
using PTANonCrown.Data.Services;

namespace PTANonCrown.Data.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
    : base(options)
    {
        AppLogger.Log($"App DB Context", "App");
    }


    /// Main Tables
    public DbSet<Stand> Stands { get; set; }
    public DbSet<Plot> Plots { get; set; }
    public DbSet<Treatment> Treatments { get; set; }
    public DbSet<CoarseWoody> CoarseWoodys { get; set; }
    public DbSet<PlotTreatment> PlotTreatments { get; set; }

    // Tree Tables
    public DbSet<TreeDead> TreesDead { get; set; }
    public DbSet<TreeLive> TreesLive { get; set; }
    public DbSet<TreeSpecies> TreeSpecies { get; set; }

    // Lookup Tables
    public DbSet<Vegetation> Vegetation { get; set; }
    public DbSet<Soil> Soil { get; set; }
    public DbSet<Ecodistrict> Ecodistrict { get; set; }

    //Junction table
    public DbSet<EcodistrictSoilVeg> EcodistrictSoilVeg { get; set; } = null!;


    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlite("Data Source=app.db");  // or your actual connection string
        optionsBuilder.EnableSensitiveDataLogging();
        return new AppDbContext(optionsBuilder.Options);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        // Stand → Plots
        modelBuilder.Entity<Stand>()
            .HasMany(s => s.Plots)
            .WithOne(p => p.Stand)
            .OnDelete(DeleteBehavior.Cascade); // optional: cascade delete plots when stand is deleted

        // Plot → PlotTreeLive
        modelBuilder.Entity<Plot>()
            .HasMany(p => p.PlotTreeLive)
            .WithOne(t => t.Plot)
            .OnDelete(DeleteBehavior.Cascade);  // Optional: cascade delete trees when plot is deleted
        
        // Plot → PlotCoarseWoody
        modelBuilder.Entity<Plot>()
                .HasMany(p => p.PlotCoarseWoody)
                .WithOne(t => t.Plot)
                .OnDelete(DeleteBehavior.Cascade);  // Optional: cascade delete trees when plot is deleted

        // Plot → PlotTreeDead
        modelBuilder.Entity<Plot>()
            .HasMany(p => p.PlotTreeDead)
            .WithOne(t => t.Plot)
            .OnDelete(DeleteBehavior.Cascade);  // Optional: cascade delete trees when plot is deleted

        // Treatments
        modelBuilder.Entity<PlotTreatment>()
            .HasKey(sc => new { sc.PlotId, sc.TreatmentId });

        modelBuilder.Entity<PlotTreatment>()
            .HasOne(pt => pt.Plot)
            .WithMany(p => p.PlotTreatments);

        modelBuilder.Entity<PlotTreatment>()
            .HasOne(pt => pt.Treatment)
            .WithMany(t => t.PlotTreatments);

        // Lookups
        // Use codes as keys instead of IDs
        modelBuilder.Entity<Soil>().HasKey(s => s.ShortCode);
        modelBuilder.Entity<Vegetation>().HasKey(v => v.ShortCode);
        modelBuilder.Entity<Ecodistrict>().HasKey(e => e.ShortCode);

        modelBuilder.Entity<EcodistrictSoilVeg>()
            .HasKey(ev => new { ev.SoilCode, ev.VegCode, ev.EcositeGroup });


    }
}