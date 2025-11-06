using Microsoft.EntityFrameworkCore;
using PTANonCrown.Data.Models;
using PTANonCrown.Data.Services;

namespace PTANonCrown.Data.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
    : base(options)
    {
        AppLoggerData.Log($"App DB Context", "App");
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

        modelBuilder.Entity<Plot>()
            .HasOne(p => p.Soil)
            .WithMany()
            .HasForeignKey(p => p.SoilCode)
            .IsRequired(false);  // optional

        modelBuilder.Entity<Plot>()
            .HasOne(p => p.Vegetation)
            .WithMany()
            .HasForeignKey(p => p.VegCode)
            .IsRequired(false);

        modelBuilder.Entity<Plot>()
            .HasOne(p => p.EcoDistrict)
            .WithMany()
            .HasForeignKey(p => p.EcodistrictCode)
            .IsRequired(false);



        modelBuilder.Entity<Soil>().HasKey(s => s.ShortCode);
        modelBuilder.Entity<Vegetation>().HasKey(v => v.ShortCode);
        modelBuilder.Entity<Ecodistrict>().HasKey(e => e.ShortCode);
        modelBuilder.Entity<TreeSpecies>().HasKey(e => e.ShortCode);

        modelBuilder.Entity<EcodistrictSoilVeg>()
            .HasKey(ev => new { ev.SoilCode, ev.VegCode, ev.EcositeGroup });
        /*
        modelBuilder.Entity<TreeSpecies>().HasData(
           new TreeSpecies { ID = 1, ShortCode = "n/a", Name = "Unknown", HardwoodSoftwood = HardwoodSoftwood.Unknown, LIT = false, LT = false, CustomOrder = 1 },
           new TreeSpecies { ID = 2, ShortCode = "rS", Name = "Red Spruce", HardwoodSoftwood = HardwoodSoftwood.Softwood, LIT = true, LT = true, CustomOrder = 2 },
           new TreeSpecies { ID = 3, ShortCode = "eH", Name = "Eastern Hemlock", HardwoodSoftwood = HardwoodSoftwood.Softwood, LIT = true, LT = true, CustomOrder = 3 },
           new TreeSpecies { ID = 4, ShortCode = "wP", Name = "White Pine", HardwoodSoftwood = HardwoodSoftwood.Softwood, LIT = true, LT = false, CustomOrder = 4 },
           new TreeSpecies { ID = 5, ShortCode = "wS", Name = "White Spruce", HardwoodSoftwood = HardwoodSoftwood.Softwood, LIT = false, LT = false, CustomOrder = 5 },
           new TreeSpecies { ID = 6, ShortCode = "bS", Name = "Black Spruce", HardwoodSoftwood = HardwoodSoftwood.Softwood, LIT = false, LT = false, CustomOrder = 6 },
           new TreeSpecies { ID = 7, ShortCode = "bF", Name = "Balsam Fir", HardwoodSoftwood = HardwoodSoftwood.Softwood, LIT = false, LT = false, CustomOrder = 7 },
           new TreeSpecies { ID = 8, ShortCode = "rP", Name = "Red Pine", HardwoodSoftwood = HardwoodSoftwood.Softwood, LIT = false, LT = false, CustomOrder = 8 },
           new TreeSpecies { ID = 9, ShortCode = "jP", Name = "Jack Pine", HardwoodSoftwood = HardwoodSoftwood.Softwood, LIT = false, LT = false, CustomOrder = 9 },
           new TreeSpecies { ID = 10, ShortCode = "eL", Name = "Eastern Larch", HardwoodSoftwood = HardwoodSoftwood.Softwood, LIT = false, LT = false, CustomOrder = 10 },
           new TreeSpecies { ID = 11, ShortCode = "nS", Name = "Norway Spruce", HardwoodSoftwood = HardwoodSoftwood.Softwood, LIT = false, LT = false, CustomOrder = 11 },
           new TreeSpecies { ID = 12, ShortCode = "sM", Name = "Sugar Maple", HardwoodSoftwood = HardwoodSoftwood.Hardwood, LIT = true, LT = true, CustomOrder = 12 },
           new TreeSpecies { ID = 13, ShortCode = "strM", Name = "Striped Maple", HardwoodSoftwood = HardwoodSoftwood.Hardwood, LIT = false, LT = false, CustomOrder = 13 },
           new TreeSpecies { ID = 14, ShortCode = "mM", Name = "Mountain Maple", HardwoodSoftwood = HardwoodSoftwood.Hardwood, LIT = false, LT = false, CustomOrder = 14 },
           new TreeSpecies { ID = 15, ShortCode = "yB", Name = "Yellow Birch", HardwoodSoftwood = HardwoodSoftwood.Hardwood, LIT = true, LT = false, CustomOrder = 15 },
           new TreeSpecies { ID = 16, ShortCode = "wA", Name = "White Ash", HardwoodSoftwood = HardwoodSoftwood.Hardwood, LIT = true, LT = false, CustomOrder = 16 },
           new TreeSpecies { ID = 17, ShortCode = "bA", Name = "Black Ash", HardwoodSoftwood = HardwoodSoftwood.Hardwood, LIT = false, LT = false, CustomOrder = 17 },
           new TreeSpecies { ID = 18, ShortCode = "mA", Name = "Mountain Ash", HardwoodSoftwood = HardwoodSoftwood.Hardwood, LIT = false, LT = false, CustomOrder = 18 },
           new TreeSpecies { ID = 19, ShortCode = "rO", Name = "Red Oak", HardwoodSoftwood = HardwoodSoftwood.Hardwood, LIT = true, LT = false, CustomOrder = 19 },
           new TreeSpecies { ID = 20, ShortCode = "rM", Name = "Red Maple", HardwoodSoftwood = HardwoodSoftwood.Hardwood, LIT = true, LT = false, CustomOrder = 20 },
           new TreeSpecies { ID = 21, ShortCode = "wB", Name = "White Birch", HardwoodSoftwood = HardwoodSoftwood.Hardwood, LIT = false, LT = false, CustomOrder = 21 },
           new TreeSpecies { ID = 22, ShortCode = "gB", Name = "Gray Birch", HardwoodSoftwood = HardwoodSoftwood.Hardwood, LIT = false, LT = false, CustomOrder = 22 },
           new TreeSpecies { ID = 23, ShortCode = "tA", Name = "Trembling Aspen", HardwoodSoftwood = HardwoodSoftwood.Hardwood, LIT = false, LT = false, CustomOrder = 23 },
           new TreeSpecies { ID = 24, ShortCode = "ltA", Name = "Large-tooth Aspen", HardwoodSoftwood = HardwoodSoftwood.Hardwood, LIT = false, LT = false, CustomOrder = 24 },
           new TreeSpecies { ID = 25, ShortCode = "bC", Name = "Black Cherry", HardwoodSoftwood = HardwoodSoftwood.Hardwood, LIT = false, LT = false, CustomOrder = 25 },
           new TreeSpecies { ID = 26, ShortCode = "iW", Name = "Ironwood", HardwoodSoftwood = HardwoodSoftwood.Hardwood, LIT = false, LT = false, CustomOrder = 26 },
           new TreeSpecies { ID = 27, ShortCode = "bP", Name = "Balsam Poplar", HardwoodSoftwood = HardwoodSoftwood.Hardwood, LIT = false, LT = false, CustomOrder = 27 },
           new TreeSpecies { ID = 28, ShortCode = "wE", Name = "White Elm", HardwoodSoftwood = HardwoodSoftwood.Hardwood, LIT = false, LT = false, CustomOrder = 28 },
           new TreeSpecies { ID = 29, ShortCode = "pC", Name = "Pin Cherry", HardwoodSoftwood = HardwoodSoftwood.Hardwood, LIT = false, LT = false, CustomOrder = 29 },
           new TreeSpecies { ID = 30, ShortCode = "sP", Name = "Scots pine", HardwoodSoftwood = HardwoodSoftwood.Softwood, LIT = false, LT = false, CustomOrder = 30 },
           new TreeSpecies { ID = 31, ShortCode = "jL", Name = "Japanese Larch", HardwoodSoftwood = HardwoodSoftwood.Softwood, LIT = false, LT = false, CustomOrder = 31 },
           new TreeSpecies { ID = 32, ShortCode = "sS", Name = "Sitka Spruce", HardwoodSoftwood = HardwoodSoftwood.Softwood, LIT = false, LT = false, CustomOrder = 32 },
           new TreeSpecies { ID = 33, ShortCode = "dF", Name = "Douglas Fir", HardwoodSoftwood = HardwoodSoftwood.Softwood, LIT = false, LT = false, CustomOrder = 33 },
           new TreeSpecies { ID = 34, ShortCode = "lP", Name = "Lodgepole Pine", HardwoodSoftwood = HardwoodSoftwood.Softwood, LIT = false, LT = false, CustomOrder = 34 }
       ); */
    }
}