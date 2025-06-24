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
     new VegLookup{ID = 2, ShortCode = "CE1", Name = "Eastern white cedar / Speckled alder / Cinnamon fern / Sphagnum" },
    new VegLookup { ID = 3, ShortCode = "CE1a", Name = "Eastern white cedar / Speckled alder / Cinnamon fern / Sphagnum (Poison ivy variant)" },
    new VegLookup { ID = 4, ShortCode = "CE2", Name = "Eastern white cedar - Balsam fir / Stair-step moss" },
    new VegLookup { ID = 5, ShortCode = "CO1", Name = "Black spruce - Balsam fir / Foberry / Plume moss" },
    new VegLookup { ID = 6, ShortCode = "CO2", Name = "White spruce - Balsam fir / Foberry / Twinflower" },
    new VegLookup { ID = 7, ShortCode = "CO2a", Name = "White spruce - Balsam fir / Foberry / Twinflower (Black crowberry Headland variant)" },
    new VegLookup { ID = 8, ShortCode = "CO3", Name = "Red spruce / Mountain-ash / Foberry" },
    new VegLookup { ID = 9, ShortCode = "CO4", Name = "Balsam fir / Foberry - Twinflower" },
    new VegLookup { ID = 10, ShortCode = "CO5", Name = "White birch - Balsam fir / Foberry - Wood aster" },
    new VegLookup { ID = 11, ShortCode = "CO6", Name = "Red maple - Birch / Bunchberry - Sarsaparilla" },
    new VegLookup { ID = 12, ShortCode = "CO7", Name = "White spruce / Bayberry" },
    new VegLookup { ID = 13, ShortCode = "FP1", Name = "Sugar maple - White ash / Ostrich fern - Wood goldenrod" },
    new VegLookup { ID = 14, ShortCode = "FP2", Name = "Red maple - Red oak / Bellwort - Nodding trillium" },
    new VegLookup { ID = 15, ShortCode = "FP2a", Name = "Red maple - Red oak / Bellwort - Nodding trillium (Sugar maple variant)" },
    new VegLookup { ID = 16, ShortCode = "FP3", Name = "Red maple / Sensitive fern - Rough goldenrod" },
    new VegLookup { ID = 17, ShortCode = "FP4", Name = "Balsam poplar - White spruce / Ostrich fern - Cow-parsnip" },
    new VegLookup { ID = 18, ShortCode = "FP5", Name = "Black cherry - Red maple / Rough goldenrod - Jack-in-the‑pulpit" },
    new VegLookup { ID = 19, ShortCode = "FP6", Name = "White spruce / Wood goldenrod / Shaggy moss" },
    new VegLookup { ID = 20, ShortCode = "HL1", Name = "Balsam fir / Mountain-ash / Large‑leaved goldenrod" },
    new VegLookup { ID = 21, ShortCode = "HL1a", Name = "Balsam fir / Mountain-ash / Large‑leaved goldenrod (White birch / Wood sorrel variant)" },
    new VegLookup { ID = 22, ShortCode = "HL2", Name = "White spruce / Wood aster" },
    new VegLookup { ID = 23, ShortCode = "HL3", Name = "Yellow birch - Balsam fir / Eastern spreading wood fern - Wood sorrel" },
    new VegLookup { ID = 24, ShortCode = "HL4", Name = "Birch / Wood fern - Wood sorrel" },
    new VegLookup { ID = 25, ShortCode = "IH1", Name = "Large‑tooth aspen / Lambkill / Bracken" },
    new VegLookup { ID = 26, ShortCode = "IH1a", Name = "Large‑tooth aspen / Lambkill / Bracken (Red oak variant)" },
    new VegLookup { ID = 27, ShortCode = "IH2", Name = "Red oak - Red maple / Witch‑hazel" },
    new VegLookup { ID = 28, ShortCode = "IH2a", Name = "Red oak - Red maple / Witch‑hazel (Red oak variant)" },
    new VegLookup { ID = 29, ShortCode = "IH3", Name = "Large‑tooth aspen / Christmas fern - New York fern" },
    new VegLookup { ID = 30, ShortCode = "IH4", Name = "Trembling aspen / Wild raisin / Bunchberry" },
    new VegLookup { ID = 31, ShortCode = "IH5", Name = "Trembling aspen - White ash / Beaked hazelnut / Christmas fern" },
    new VegLookup { ID = 32, ShortCode = "IH6", Name = "White birch - Red maple / Sarsaparilla - Bracken" },
    new VegLookup { ID = 33, ShortCode = "IH6a", Name = "White birch - Red maple / Sarsaparilla - Bracken (Aspen variant)" },
    new VegLookup { ID = 34, ShortCode = "IH7", Name = "Red maple / Hay‑scented fern - Wood sorrel" },
    new VegLookup { ID = 35, ShortCode = "KA1", Name = "Hemlock / Christmas fern - White lettuce - Wood goldenrod" },
    new VegLookup { ID = 36, ShortCode = "KA2", Name = "Sugar maple / Christmas fern - Rattlesnake fern - Bulbet bladder fern" },
    new VegLookup { ID = 37, ShortCode = "MW1", Name = "Red spruce - Yellow birch / Evergreen wood fern" },
    new VegLookup { ID = 38, ShortCode = "MW2", Name = "Red spruce - Red maple - White birch / Goldthread" },
    new VegLookup { ID = 39, ShortCode = "MW2a", Name = "Red spruce - Red maple - White birch / Goldthread (Aspen variant)" },
    new VegLookup { ID = 40, ShortCode = "MW3", Name = "Hemlock - Yellow birch / Evergreen wood fern" },
    new VegLookup { ID = 41, ShortCode = "MW4", Name = "Balsam fir - Red maple / Wood sorrel - Goldthread" },
    new VegLookup { ID = 42, ShortCode = "MW5", Name = "White birch - Balsam fir / Starflower" },
    new VegLookup { ID = 43, ShortCode = "OF1", Name = "White spruce / Aster - Goldenrod / Shaggy moss" },
    new VegLookup { ID = 44, ShortCode = "OF2", Name = "Tamarack / Speckled alder / Rough goldenrod / Shaggy moss" },
    new VegLookup { ID = 45, ShortCode = "OF3", Name = "White pine - Balsam fir / Shinleaf - Pine‑sap" },
    new VegLookup { ID = 46, ShortCode = "OF4", Name = "Balsam fir - White spruce / Evergreen wood fern - Wood aster" },
    new VegLookup { ID = 47, ShortCode = "OF5", Name = "Trembling aspen - Grey birch / Rough goldenrod - Strawberry" },
    new VegLookup { ID = 48, ShortCode = "OW1", Name = "Jack pine / Huckleberry / Black crowberry / Reindeer lichen" },
    new VegLookup { ID = 49, ShortCode = "OW2", Name = "Black spruce / Lambkill / Reindeer lichen" },
    new VegLookup { ID = 50, ShortCode = "OW3", Name = "Red spruce / Red-berried elder / Rock polypody" },
    new VegLookup { ID = 51, ShortCode = "OW4", Name = "Red pine - White pine / Broom crowberry / Grey reindeer lichen" },
    new VegLookup { ID = 52, ShortCode = "OW5", Name = "Red oak / Huckleberry / Cow-wheat - Rice grass / Reindeer lichen" },
    new VegLookup { ID = 53, ShortCode = "OW6", Name = "White birch - Red oak - White ash / Marginal wood fern - Herb‑Robert" },
    new VegLookup { ID = 54, ShortCode = "SH1", Name = "Hemlock / Pin cushion moss / Needle carpet" },
    new VegLookup { ID = 55, ShortCode = "SH2", Name = "Hemlock - White pine / Sarsaparilla" },
    new VegLookup { ID = 56, ShortCode = "SH3", Name = "Red spruce - Hemlock / Wild lily‑of‑the‑valley" },
    new VegLookup { ID = 57, ShortCode = "SH4", Name = "Red spruce - White pine / Lambkill / Bracken" },
    new VegLookup { ID = 58, ShortCode = "SH4a", Name = "Red spruce - White pine / Lambkill / Bracken (Red spruce variant)" },
    new VegLookup { ID = 59, ShortCode = "SH5", Name = "Red spruce - Balsam fir / Schreber’s moss" },
    new VegLookup { ID = 60, ShortCode = "SH6", Name = "Red spruce - Balsam fir / Stair-step moss - Sphagnum" },
    new VegLookup { ID = 61, ShortCode = "SH7", Name = "White spruce - Red spruce / Blueberry / Schreber’s moss" },
    new VegLookup { ID = 62, ShortCode = "SH8", Name = "Balsam fir / Wood fern / Schreber’s moss" },
    new VegLookup { ID = 63, ShortCode = "SH9", Name = "Balsam fir - Black spruce / Blueberry" },
    new VegLookup { ID = 64, ShortCode = "SH10", Name = "White spruce - Balsam fir / Broom moss" },
    new VegLookup { ID = 65, ShortCode = "SP1", Name = "Jack pine / Bracken - Teaberry" },
    new VegLookup { ID = 66, ShortCode = "SP1a", Name = "Jack pine / Bracken - Teaberry (Black spruce variant)" },
    new VegLookup { ID = 67, ShortCode = "SP2", Name = "Red pine / Blueberry / Bracken" },
    new VegLookup { ID = 68, ShortCode = "SP2a", Name = "Red pine / Blueberry / Bracken (Black spruce variant)" },
    new VegLookup { ID = 69, ShortCode = "SP3", Name = "Red pine - White pine / Bracken - Mayflower" },
    new VegLookup { ID =70, ShortCode = "SP3a", Name = "Red pine - White pine / Bracken - Mayflower (Black spruce variant)" },
    new VegLookup { ID = 71, ShortCode = "SP4", Name = "White pine / Blueberry / Bracken" },
    new VegLookup { ID = 72, ShortCode = "SP4a", Name = "White pine / Blueberry / Bracken (Black spruce variant)" },
    new VegLookup { ID = 73, ShortCode = "SP4b", Name = "White pine / Blueberry / Bracken (Huckleberry variant)" },
    new VegLookup { ID = 74, ShortCode = "SP5", Name = "Black spruce / Lambkill / Bracken" },
    new VegLookup { ID = 75, ShortCode = "SP6", Name = "Black spruce - Red maple / Bracken - Sarsaparilla" },
    new VegLookup { ID = 76, ShortCode = "SP7", Name = "Black spruce / False holly / Ladies’ tresses sphagnum" },
    new VegLookup { ID = 77, ShortCode = "SP8", Name = "Black spruce - Aspen / Bracken - Sarsasparilla" },
    new VegLookup { ID = 78, ShortCode = "SP9", Name = "Red oak - White pine / Teaberry" },
    new VegLookup { ID = 79, ShortCode = "SP10", Name = "Tamarack / Wild raisin / Schreber’s moss" },
    new VegLookup { ID = 80, ShortCode = "TH1", Name = "Sugar maple / Hay‑scented fern" },
    new VegLookup { ID = 81, ShortCode = "TH1a", Name = "Sugar maple / Hay‑scented fern (Beech variant)" },
    new VegLookup { ID = 82, ShortCode = "TH1b", Name = "Sugar maple / Hay‑scented fern (Yellow birch variant)" },
    new VegLookup { ID = 83, ShortCode = "TH2", Name = "Sugar maple / New York fern - Northern beech fern" },
    new VegLookup { ID = 84, ShortCode = "TH2a", Name = "Sugar maple / New York fern - Northern beech fern (Yellow birch variant)" },
    new VegLookup { ID = 85, ShortCode = "TH3", Name = "Sugar maple - White ash / Christmas fern" },
    new VegLookup { ID = 86, ShortCode = "TH4", Name = "Sugar maple - White ash / Silvery spleenwort - Baneberry" },
    new VegLookup { ID = 87, ShortCode = "TH5", Name = "Beech / Sarsaparilla / Leaf litter" },
    new VegLookup { ID = 88, ShortCode = "TH6", Name = "Red oak - Yellow birch / Striped maple" },
    new VegLookup { ID = 89, ShortCode = "TH7", Name = "Yellow birch - White birch / Evergreen wood fern" },
    new VegLookup { ID = 90, ShortCode = "TH8", Name = "Red maple - Yellow birch / Striped maple" },
    new VegLookup { ID = 91, ShortCode = "TH8a", Name = "Red maple - Yellow birch / Striped maple (White ash variant)" },
    new VegLookup { ID = 92, ShortCode = "WC1", Name = "Black spruce / Cinnamon fern / Sphagnum" },
    new VegLookup { ID = 93, ShortCode = "WC2", Name = "Black spruce / Lambkill - Labrador tea / Sphagnum" },
    new VegLookup { ID = 94, ShortCode = "WC2a", Name = "Black spruce / Lambkill - Labrador tea / Sphagnum (Huckleberry - Inkberry variant)" },
    new VegLookup { ID = 95, ShortCode = "WC3", Name = "Jack pine - Black spruce / Rhodora / Sphagnum" },
    new VegLookup { ID = 96, ShortCode = "WC3a", Name = "Jack pine - Black spruce / Rhodora / Sphagnum (Black spruce variant)" },
    new VegLookup { ID = 97, ShortCode = "WC4", Name = "Red pine - Black spruce / Huckleberry - Rhodora / Sphagnum" },
    new VegLookup { ID = 98, ShortCode = "WC5", Name = "Red spruce - Balsam fir / Cinnamon fern / Sphagnum" },
    new VegLookup { ID = 99, ShortCode = "WC6", Name = "Balsam fir / Cinnamon fern - Three seeded sedge / Sphagnum" },
    new VegLookup { ID = 100, ShortCode = "WC7", Name = "Tamarack - Black spruce / Lambkill / Sphagnum" },
    new VegLookup { ID = 101, ShortCode = "WC7a", Name = "Tamarack - Black spruce / Lambkill / Sphagnum (Huckleberry - Inkberry variant)" },
    new VegLookup { ID = 102, ShortCode = "WC8", Name = "Hemlock / Cinnamon fern - Sensitive fern / Sphagnum" },
    new VegLookup { ID = 103, ShortCode = "WD1", Name = "White ash / Sensitive fern - Christmas fern" },
    new VegLookup { ID = 104, ShortCode = "WD2", Name = "Red maple / Cinnamon fern / Sphagnum" },
    new VegLookup { ID = 105, ShortCode = "WD3", Name = "Red maple / Sensitive fern - Lady fern / Sphagnum" },
    new VegLookup { ID = 106, ShortCode = "WD4", Name = "Red maple / Poison ivy / Sphagnum" },
    new VegLookup { ID = 107, ShortCode = "WD4a", Name = "Red maple / Poison ivy / Sphagnum (Huckleberry - Inkberry variant)" },
    new VegLookup { ID = 108, ShortCode = "WD5", Name = "Trembling aspen / Beaked hazelnut / Interrupted fern / Sphagnum" },
    new VegLookup { ID = 109, ShortCode = "WD6", Name = "Red maple - Balsam fir / Wood aster / Sphagnum" },
    new VegLookup { ID = 110, ShortCode = "WD7", Name = "Balsam fir - White ash / Cinnamon fern - New York fern / Sphagnum" },
    new VegLookup { ID = 111, ShortCode = "WD8", Name = "Red spruce - Red maple / Wood sorrel - Sensitive fern / Sphagnum" });

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
            new TreeSpecies { ID = 1, ShortCode = "n/a", Name = "Unknown", HardwoodSoftwood = HardwoodSoftwood.Unknown, LIT = false, LIT_planted = false, LT = false },
            new TreeSpecies { ID = 2, ShortCode = "rS", Name = "Red Spruce", HardwoodSoftwood = HardwoodSoftwood.Softwood, LIT = true, LIT_planted = true, LT = true },
            new TreeSpecies { ID = 3, ShortCode = "eH", Name = "Eastern Hemlock", HardwoodSoftwood = HardwoodSoftwood.Softwood, LIT = true, LIT_planted = true, LT = true },
            new TreeSpecies { ID = 4, ShortCode = "wP", Name = "White Pine", HardwoodSoftwood = HardwoodSoftwood.Softwood, LIT = true, LIT_planted = true, LT = false },
            new TreeSpecies { ID = 5, ShortCode = "wS", Name = "White Spruce", HardwoodSoftwood = HardwoodSoftwood.Softwood, LIT = false, LIT_planted = false, LT = false },
            new TreeSpecies { ID = 6, ShortCode = "S", Name = "Black Spruce/Coastal", HardwoodSoftwood = HardwoodSoftwood.Softwood, LIT = true, LIT_planted = true, LT = false },
            new TreeSpecies { ID = 7, ShortCode = "bF", Name = "Balsam Fir", HardwoodSoftwood = HardwoodSoftwood.Softwood, LIT = false, LIT_planted = false, LT = false },
            new TreeSpecies { ID = 8, ShortCode = "rP", Name = "Red Pine", HardwoodSoftwood = HardwoodSoftwood.Softwood, LIT = false, LIT_planted = false, LT = false },
            new TreeSpecies { ID = 9, ShortCode = "jP", Name = "Jack Pine", HardwoodSoftwood = HardwoodSoftwood.Softwood, LIT = false, LIT_planted = false, LT = false },
            new TreeSpecies { ID = 10, ShortCode = "eL", Name = "Eastern Larch", HardwoodSoftwood = HardwoodSoftwood.Softwood, LIT = false, LIT_planted = false, LT = false },
            new TreeSpecies { ID = 11, ShortCode = "sM", Name = "Sugar Maple", HardwoodSoftwood = HardwoodSoftwood.Hardwood, LIT = true, LIT_planted = true, LT = false },
            new TreeSpecies { ID = 12, ShortCode = "yB", Name = "Yellow Birch", HardwoodSoftwood = HardwoodSoftwood.Hardwood, LIT = true, LIT_planted = true, LT = false },
            new TreeSpecies { ID = 13, ShortCode = "wA", Name = "White Ash", HardwoodSoftwood = HardwoodSoftwood.Hardwood, LIT = true, LIT_planted = true, LT = false },
            new TreeSpecies { ID = 14, ShortCode = "rO", Name = "Red Oak", HardwoodSoftwood = HardwoodSoftwood.Hardwood, LIT = true, LIT_planted = true, LT = false },
            new TreeSpecies { ID = 15, ShortCode = "rM", Name = "Red Maple", HardwoodSoftwood = HardwoodSoftwood.Hardwood, LIT = true, LIT_planted = false, LT = true },
            new TreeSpecies { ID = 16, ShortCode = "wB", Name = "White Birch", HardwoodSoftwood = HardwoodSoftwood.Hardwood, LIT = false, LIT_planted = false, LT = false },
            new TreeSpecies { ID = 17, ShortCode = "tA", Name = "Trembling Aspen", HardwoodSoftwood = HardwoodSoftwood.Hardwood, LIT = false, LIT_planted = false, LT = false },
            new TreeSpecies { ID = 18, ShortCode = "lTA", Name = "Large-tooth Aspen", HardwoodSoftwood = HardwoodSoftwood.Hardwood, LIT = false, LIT_planted = false, LT = false }
        );

        modelBuilder.Entity<Treatment>().HasData(
                  new Treatment { ID = 1, Name = "Pre-commercial thinning" },
                  new Treatment { ID = 2, Name = "Commercial thinning" },
                  new Treatment { ID = 3, Name = "Strip shelterwood" },
                  new Treatment { ID = 4, Name = "Patch shelterwood" },
                  new Treatment { ID = 5, Name = "Uniform shelterwood" },
                  new Treatment { ID = 6, Name = "Gap irregular shelterwood" },
                  new Treatment { ID = 7, Name = "Continuous cover irregular shelterwood" },
                  new Treatment { ID = 8, Name = "Single tree selection" },
                  new Treatment { ID = 9, Name = "Group selection" },
                  new Treatment { ID = 10, Name = "Partial harvest (unknown)" }
              );

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