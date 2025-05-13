using Microsoft.EntityFrameworkCore;
using PTANonCrown.Data.Models;
using System.IO;
using System.Threading.Tasks;
namespace PTANonCrown.Data.Context
{
    public class LookupDbContext : DbContext
    {
        public LookupDbContext(DbContextOptions<LookupDbContext> options)
            : base(options)
        {
        }

    public DbSet<SoilLookup> SoilLookup { get; set; }
        public DbSet<VegLookup> VegLookup { get; set; }
        public DbSet<EcodistrictLookup> EcodistrictLookup { get; set; }



        /*private static void CopyDatabaseFromAssets(string dbFileName)
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
        }*/
    }
}