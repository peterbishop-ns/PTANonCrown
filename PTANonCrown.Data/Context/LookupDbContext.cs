using Microsoft.EntityFrameworkCore;
using PTANonCrown.Data.Models;
using Microsoft.Maui.Storage;

namespace PTANonCrown.Data.Context
{
    public class LookupDbContext : DbContext
    {
        public LookupDbContext()
        {

        }

        public DbSet<SoilLookup> SoilLookup { get; set; }
        public DbSet<VegLookup> VegLookup { get; set; }
        public DbSet<EcodistrictLookup> EcodistrictLookup { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Define the path to the database in the Resources/Raw folder
            string dbFileName = "lookup.db";
            string dbPath = Path.Combine("C://temp", dbFileName);

            // Use SQLite from the app package (Resources/Raw) if the file exists
            if (!File.Exists(dbPath))
            {
                // Copy the database from app package (Resources/Raw) to AppDataDirectory (if not already done)
                CopyDatabaseFromAssets(dbFileName);
            }

            // Use SQLite from the AppDataDirectory for read-only access
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }

        private static async Task CopyDatabaseFromAssetsAsync(string dbFileName)
        {
            string targetPath = Path.Combine(FileSystem.AppDataDirectory, dbFileName);

            if (!File.Exists(targetPath))
            {
                using var sourceStream = await FileSystem.OpenAppPackageFileAsync(dbFileName);
                using var targetStream = File.Create(targetPath);
                await sourceStream.CopyToAsync(targetStream);
            }
        }

    }
}