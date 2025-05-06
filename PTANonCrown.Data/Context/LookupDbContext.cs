using Microsoft.EntityFrameworkCore;
using PTANonCrown.Data.Models;

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

        private static void CopyDatabaseFromAssets(string dbFileName)
        {
            // Open the embedded database from Resources/Raw
            //  using var stream = await FileSystem.OpenAppPackageFileAsync(dbFileName);

            // Define the destination path inside the app's data directory
            // string dbPath = Path.Combine(FileSystem.AppDataDirectory, dbFileName);

            // Only copy if it doesn't already exist
            //  if (!File.Exists(dbPath))
            //  {
            //      using var fileStream = new FileStream(dbPath, FileMode.Create, FileAccess.Write);
            //   await stream.CopyToAsync(fileStream);
            // }
            string sourcePath = Path.Combine(@"C:\Code\MAUI\PTANonCrown\PTANonCrown\Resources\Raw", dbFileName);
            string targetPath = Path.Combine(@"C:\temp", dbFileName);

            if (!File.Exists(targetPath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(targetPath)!);
                File.Copy(sourcePath, targetPath);
            }
        }
    }
}