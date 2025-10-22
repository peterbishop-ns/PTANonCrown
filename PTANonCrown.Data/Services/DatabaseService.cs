using Microsoft.EntityFrameworkCore;
using PTANonCrown.Data.Context;

namespace PTANonCrown.Data.Services
{
    public class DatabaseService
    {
        public string CurrentDbPath { get; private set; }

        public DatabaseService(string dbPath)
        {
            AppLoggerData.Log($"Initiating DB path: {dbPath}", "DatabaseService");

            CurrentDbPath = dbPath;
        }

        public void SetDatabasePath(string newPath)
        {
            AppLoggerData.Log($"Changing DB path: {newPath}", "DatabaseService");
            CurrentDbPath = newPath;
        }

        public AppDbContext GetContext()
        {
            AppLoggerData.Log($"Getting Context: {CurrentDbPath}", "DatabaseService");

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite($"Data Source={CurrentDbPath}")
                .Options;

            return new AppDbContext(options);
        }
    }
}
