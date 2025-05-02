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
    public DbSet<TreeSpecies> TreeSpecies { get; set; }
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


        modelBuilder.Entity<TreeSpecies>().HasData(
            new TreeSpecies { ID = 999, ShortCode = "n/a", Name = "Unknown", HardwoodSoftwood =  HardwoodSoftwood.Unknown, LIT = false, LIT_planted = false, LT = false},
            new TreeSpecies { ID = 1, ShortCode = "rS", Name = "Red Spruce", HardwoodSoftwood =  HardwoodSoftwood.Softwood, LIT = true, LIT_planted = true, LT = true },
            new TreeSpecies { ID = 2, ShortCode = "eH", Name = "Eastern Hemlock", HardwoodSoftwood = HardwoodSoftwood.Softwood, LIT = true, LIT_planted = true, LT = true },
            new TreeSpecies { ID = 3, ShortCode = "wP", Name = "White Pine", HardwoodSoftwood = HardwoodSoftwood.Softwood, LIT = true, LIT_planted = true, LT = false },
            new TreeSpecies { ID = 4, ShortCode = "wS", Name = "White Spruce", HardwoodSoftwood = HardwoodSoftwood.Softwood, LIT = false, LIT_planted = false, LT = false },
            new TreeSpecies { ID = 5, ShortCode = "S", Name = "Black Spruce/Coastal", HardwoodSoftwood = HardwoodSoftwood.Softwood, LIT = true, LIT_planted = true, LT = false },
            new TreeSpecies { ID = 6, ShortCode = "bF", Name = "Balsam Fir", HardwoodSoftwood = HardwoodSoftwood.Softwood, LIT = false, LIT_planted = false, LT = false },
            new TreeSpecies { ID = 7, ShortCode = "rP", Name = "Red Pine", HardwoodSoftwood = HardwoodSoftwood.Softwood, LIT = false, LIT_planted = false, LT = false },
            new TreeSpecies { ID = 8, ShortCode = "jP", Name = "Jack Pine", HardwoodSoftwood = HardwoodSoftwood.Softwood, LIT = false, LIT_planted = false, LT = false },
            new TreeSpecies { ID = 9, ShortCode = "eL", Name = "Eastern Larch", HardwoodSoftwood = HardwoodSoftwood.Softwood, LIT = false, LIT_planted = false, LT = false },
            new TreeSpecies { ID = 10, ShortCode = "sM", Name = "Sugar Maple", HardwoodSoftwood = HardwoodSoftwood.Hardwood, LIT = true, LIT_planted = true, LT = false },
            new TreeSpecies { ID = 11, ShortCode = "yB", Name = "Yellow Birch", HardwoodSoftwood = HardwoodSoftwood.Hardwood, LIT = true, LIT_planted = true, LT = false },
            new TreeSpecies { ID = 12, ShortCode = "wA", Name = "White Ash", HardwoodSoftwood = HardwoodSoftwood.Hardwood, LIT = true, LIT_planted = true, LT = false },
            new TreeSpecies { ID = 13, ShortCode = "rO", Name = "Red Oak", HardwoodSoftwood = HardwoodSoftwood.Hardwood, LIT = true, LIT_planted = true, LT = false },
            new TreeSpecies { ID = 14, ShortCode = "rM", Name = "Red Maple", HardwoodSoftwood = HardwoodSoftwood.Hardwood, LIT = true, LIT_planted = false, LT = true },
            new TreeSpecies { ID = 15, ShortCode = "wB", Name = "White Birch", HardwoodSoftwood = HardwoodSoftwood.Hardwood, LIT = false, LIT_planted = false, LT = false },
            new TreeSpecies { ID = 16, ShortCode = "tA", Name = "Trembling Aspen", HardwoodSoftwood = HardwoodSoftwood.Hardwood, LIT = false, LIT_planted = false, LT = false },
            new TreeSpecies { ID = 17, ShortCode = "lTA", Name = "Large-tooth Aspen", HardwoodSoftwood = HardwoodSoftwood.Hardwood, LIT = false, LIT_planted = false, LT = false }
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