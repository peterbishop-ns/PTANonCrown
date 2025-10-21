using Microsoft.EntityFrameworkCore;
using PTANonCrown.Data.Context;

namespace PTANonCrown.Services
{
    public class DatabaseService
    {
        public string CurrentDbPath { get; private set; } =
      Path.Combine(FileSystem.AppDataDirectory, "app.db");

        public AppDbContext GetContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite($"Data Source={CurrentDbPath}")
                .Options;

            return new AppDbContext(options);
        }
        public void SetDatabasePath(string newPath)
        {
            CurrentDbPath = newPath;
        }
    }
}