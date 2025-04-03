using Microsoft.EntityFrameworkCore;
using PTANonCrown.Models;

namespace PTANonCrown.Context
{
    public class LookupDbContext : DbContext
    {
        public DbSet<TreeLookup> TreeLookup { get; set; }
        
        public LookupDbContext() {
          
        
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Define the path to the database in the Resources/Raw folder
            string dbFileName = "lookup.db";
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, dbFileName);

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
            // Open the SQLite database file embedded in the app package (Resources/Raw)
            using var stream = FileSystem.OpenAppPackageFileAsync(dbFileName).Result;

            // Define the path to copy the database to AppDataDirectory
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, dbFileName);

            // Copy the database to AppDataDirectory (this will only happen once)
            using var fileStream = new FileStream(dbPath, FileMode.Create);
            stream.CopyTo(fileStream);
        }
    }
}
