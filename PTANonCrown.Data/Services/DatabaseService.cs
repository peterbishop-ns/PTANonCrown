using Microsoft.EntityFrameworkCore;
using PTANonCrown.Data.Context;

namespace PTANonCrown.Data.Services
{
    public class DatabaseService
    {
        public string CurrentDbPath { get; private set; }
        public bool DbIsNew{ get; set; }

        public DatabaseService(string? dbPath = null)
        {
            if (dbPath != null && dbPath != string.Empty)
            {
                AppLoggerData.Log($"Initiating DB path: {dbPath}", "DatabaseService");
                SetDatabasePath(dbPath);
            }
         
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
