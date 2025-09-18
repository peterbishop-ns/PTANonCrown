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
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlite("Data Source=app.db");  // or your actual connection string
        optionsBuilder.EnableSensitiveDataLogging();
        return new AppDbContext(optionsBuilder.Options);
    }

    public DbSet<CoarseWoody> CoarseWoodys { get; set; }
    public DbSet<EcodistrictLookup> EcodistrictLookup { get; set; }
    public DbSet<Plot> Plots { get; set; }
    public DbSet<PlotTreatment> PlotTreatments { get; set; }
    public DbSet<SoilLookup> SoilLookup { get; set; }
    public DbSet<Stand> Stands { get; set; }
    public DbSet<Treatment> Treatments { get; set; }
    public DbSet<TreeDead> TreesDead { get; set; }
    public DbSet<TreeLive> TreesLive { get; set; }
    public DbSet<TreeSpecies> TreeSpecies { get; set; }
    public DbSet<VegLookup> VegLookup { get; set; }
    public DbSet<ExposureLookup> ExposureLookup { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SoilLookup>().HasData(
     new SoilLookup { ID = 1, ShortCode = "n/a", Name = "Unknown" },
     new SoilLookup { ID = 2, ShortCode = "ST1", Name = "Dry - CT" },
    new SoilLookup { ID = 3, ShortCode = "ST2", Name = "Fresh - MT" },
    new SoilLookup { ID = 4, ShortCode = "ST3", Name = "Moist - MT" },
    new SoilLookup { ID = 5, ShortCode = "ST4", Name = "Wet - MT" },
    new SoilLookup { ID = 6, ShortCode = "ST5", Name = "Fresh - FT" },
    new SoilLookup { ID = 7, ShortCode = "ST6", Name = "Moist - FT" },
    new SoilLookup { ID = 8, ShortCode = "ST7", Name = "Wet - FT" },
    new SoilLookup { ID = 9, ShortCode = "ST8", Name = "Rich Fresh - MT" },
    new SoilLookup { ID = 10, ShortCode = "ST9", Name = "Rich Moist - MT" },
    new SoilLookup { ID = 11, ShortCode = "ST10", Name = "Rich Wet - MT" },
    new SoilLookup { ID = 12, ShortCode = "ST11", Name = "Rich Fresh - FT" },
    new SoilLookup { ID = 13, ShortCode = "ST12", Name = "Rich Moist - FT" },
    new SoilLookup { ID = 14, ShortCode = "ST13", Name = "Rich Wet - FT" },
    new SoilLookup { ID = 15, ShortCode = "ST14", Name = "Organic" },
    new SoilLookup { ID = 16, ShortCode = "ST15", Name = "Dry Shallow - MT" },
    new SoilLookup { ID = 17, ShortCode = "ST16", Name = "Moist Shallow - MT" },
    new SoilLookup { ID = 18, ShortCode = "ST17", Name = "Rich Dry Shallow - MT" },
    new SoilLookup { ID = 19, ShortCode = "ST18", Name = "Rich Moist Shallow - MT" });

        modelBuilder.Entity<ExposureLookup>().HasData(
            new ExposureLookup { ID = 1,  Name = "n/a - Not assessed" },
            new ExposureLookup { ID = 2, Name = "Exposed"},
            new ExposureLookup { ID = 3, Name = "Moderately exposed"},
            new ExposureLookup { ID = 4, Name = "Moderate/neutral"},
            new ExposureLookup { ID = 5, Name = "Moderately sheltered"},
            new ExposureLookup { ID = 6, Name = "Sheltered"}
            
            );
        
        modelBuilder.Entity<VegLookup>().HasData(
            new VegLookup {ID = 1, ShortCode = "n/a", Name = "Unknown" },
            new VegLookup{ID = 2, ShortCode = "CE1", Name = "Eastern white cedar / Speckled alder / Cinnamon fern / Sphagnum", ForestGroup = ForestGroup.SprucePine},
            new VegLookup { ID = 3, ShortCode = "CE1a", Name = "Eastern white cedar / Speckled alder / Cinnamon fern / Sphagnum (Poison ivy variant)", ForestGroup = ForestGroup.SpruceHemlock},
            new VegLookup { ID = 4, ShortCode = "CE2", Name = "Eastern white cedar - Balsam fir / Stair-step moss", ForestGroup = ForestGroup.KarstForest},
            new VegLookup { ID = 5, ShortCode = "TH1", Name = "Tolerant Hardwood TESTING", ForestGroup = ForestGroup.TolerantHardwood },
            new VegLookup { ID = 6, ShortCode = "CB1", Name = "Coastal Boreal TESTING", ForestGroup = ForestGroup.CoastalBoreal},
            new VegLookup { ID = 7, ShortCode = "HG1", Name = "Highlands TESTING", ForestGroup = ForestGroup.Highland },
            new VegLookup { ID = 8, ShortCode = "PL1", Name = "Planted TESTING", ForestGroup = ForestGroup.PlantedForest },
            new VegLookup { ID = 9, ShortCode = "OF1", Name = "Old Field TESTING", ForestGroup = ForestGroup.OldField })
       ;

        modelBuilder.Entity<EcodistrictLookup>().HasData(
                new EcodistrictLookup { ID = 1, ShortCode = "n/a", Name = "Unknown", EcositeGroup = "Unknown" },
                new EcodistrictLookup { ID = 2, ShortCode = "100", Name = "Northern Plateau", EcositeGroup = "Maritime Boreal" },
                new EcodistrictLookup { ID = 3, ShortCode = "210", Name = "Cape Breton Highlands", EcositeGroup = "Maritime Boreal" },
                new EcodistrictLookup { ID = 4, ShortCode = "220", Name = "Victoria Lowlands", EcositeGroup = "Acadian" },
                new EcodistrictLookup { ID = 5, ShortCode = "300", Name = "Nova Scotia Uplands", EcositeGroup = "Acadian" },
                new EcodistrictLookup { ID = 6, ShortCode = "400", Name = "Eastern", EcositeGroup = "Acadian" },
                new EcodistrictLookup { ID = 7, ShortCode = "500", Name = "Northumberland / Bras d’Or", EcositeGroup = "Acadian" },
                new EcodistrictLookup { ID = 8, ShortCode = "600", Name = "Valley and Central Lowlands", EcositeGroup = "Acadian" },
                new EcodistrictLookup { ID = 9, ShortCode = "700", Name = "Western", EcositeGroup = "Acadian" },
                new EcodistrictLookup { ID = 10, ShortCode = "800", Name = "Atlantic Coastal", EcositeGroup = "Maritime Boreal" },
                new EcodistrictLookup { ID = 11, ShortCode = "900", Name = "Fundy Shore", EcositeGroup = "Acadian" }
            );



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
);





        modelBuilder.Entity<Treatment>().HasData(
                  new Treatment { ID = 1, Name = "Single Tree Selection" },
                  new Treatment { ID = 2, Name = "Group Selection" },
                  new Treatment { ID = 3, Name = "Irregular Shelterwood (Continuous Cover)" },
                  new Treatment { ID = 4, Name = "Irregular Shelterwood (Gap)" },
                  new Treatment { ID = 5, Name = "Commercial Thinning" },
                  new Treatment { ID = 6, Name = "Uniform Shelterwood with Reserves (Establishment Cut)" },
                  new Treatment { ID = 7, Name = "Gap Shelterwood with Reserves" },
                  new Treatment { ID = 8, Name = "Strip Shelterwood with Reserves" },
                  new Treatment { ID = 9, Name = "Partial Overstory Removal (1/5 retention)" },
                  new Treatment { ID = 10, Name = "Partial Overstory Removal (1/3 retention)" },
                  new Treatment { ID = 11, Name = "Partial Overstory Removal and Plant (1/3 retention)" },
                  new Treatment { ID = 12, Name = "Salvage with Retention" }
              );

        // Stand → Plots
        modelBuilder.Entity<Stand>()
            .HasMany(s => s.Plots)
            .WithOne(p => p.Stand)
           // .HasForeignKey("StandID")  // shadow property, no need for it in Plot class
            .OnDelete(DeleteBehavior.Cascade); // optional: cascade delete plots when stand is deleted

        // Plot → PlotTreeLive
        modelBuilder.Entity<Plot>()
            .HasMany(p => p.PlotTreeLive)
            .WithOne(t => t.Plot)
           // .HasForeignKey("PlotID")
            .OnDelete(DeleteBehavior.Cascade);  // Optional: cascade delete trees when plot is deleted


        modelBuilder.Entity<PlotTreatment>()
            .HasKey(sc => new { sc.PlotId, sc.TreatmentId });

        modelBuilder.Entity<PlotTreatment>()
            .HasOne(pt => pt.Plot)
            .WithMany(p => p.PlotTreatments);
        // .HasForeignKey("PlotId");

        modelBuilder.Entity<PlotTreatment>()
            .HasOne(pt => pt.Treatment)
            .WithMany(t => t.PlotTreatments);
       //     .HasForeignKey("TreatmentId");

        // Plot → PlotCoarseWoody
        modelBuilder.Entity<Plot>()
                .HasMany(p => p.PlotCoarseWoody)
                .WithOne(t => t.Plot)
              //  .HasForeignKey("PlotID")
                .OnDelete(DeleteBehavior.Cascade);  // Optional: cascade delete trees when plot is deleted

        // Plot → PlotTreeDead
        modelBuilder.Entity<Plot>()
            .HasMany(p => p.PlotTreeDead)
            .WithOne(t => t.Plot)
          //  .HasForeignKey("PlotID")
            .OnDelete(DeleteBehavior.Cascade);  // Optional: cascade delete trees when plot is deleted
    }
}