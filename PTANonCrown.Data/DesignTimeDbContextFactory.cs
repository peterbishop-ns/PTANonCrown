using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using PTANonCrown.Data.Context;
using PTANonCrown.Data.Services;

namespace PTANonCrown.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        string dbPath;


            // Fallback for EF CLI tools (no MAUI FileSystem available)
            dbPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "app.db"
            );
  
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlite($"Filename={dbPath}");

        return new AppDbContext(optionsBuilder.Options);
    }
}