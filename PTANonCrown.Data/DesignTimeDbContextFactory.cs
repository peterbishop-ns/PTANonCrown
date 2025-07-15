using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using PTANonCrown.Data.Context;
using PTANonCrown.Data.Services;

namespace PTANonCrown.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        // Use the same connection string your app uses
        var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "app.db");
        optionsBuilder.UseSqlite($"Filename={dbPath}");
        AppLogger.Log($"App DB Location", $"{dbPath}");

        return new AppDbContext(optionsBuilder.Options);
    }
}
