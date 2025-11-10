using Microsoft.EntityFrameworkCore;
using PTANonCrown.Data.Context;

namespace PTANonCrown.Data.Services
{
    public class DatabaseService
    {
        public string CurrentDbPath { get; private set; }
        public bool DbIsNew{ get; set; }
        private DbContextOptions<AppDbContext>? _options;

        public DatabaseService(string? dbPath = null)
        {
            if (dbPath != null && dbPath != string.Empty)
            {
                AppLoggerData.Log($"Initiating DB path: {dbPath}", "DatabaseService");
                SetDatabasePath(dbPath);
            }
         
        }

        public void CreateNewDatabase(string templatePath)
        {
            if (!File.Exists(templatePath))
                throw new FileNotFoundException("Template database not found.", templatePath);

            // Get the directory of the template
            var baseDir = Path.GetDirectoryName(templatePath);
            if (string.IsNullOrEmpty(baseDir))
                throw new InvalidOperationException("Cannot determine template directory.");

            // Build a new working file path in the same directory
            var newFileName = $"working_{Guid.NewGuid()}.db";
            var newWorkingPath = Path.Combine(baseDir, newFileName);

            // Copy the template to the new working DB
            File.Copy(templatePath, newWorkingPath, overwrite: true);

            // Set in DatabaseService
            SetDatabasePath(newWorkingPath);
            DbIsNew = true;


        }

        public void SetDatabasePath(string newPath)
        {
            AppLoggerData.Log($"Changing DB path: {newPath}", "DatabaseService");
            CurrentDbPath = newPath; 

            _options = BuildOptions(newPath);


        }

        private DbContextOptions<AppDbContext> BuildOptions(string dbPath)
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite($"Data Source={dbPath}")
                 .EnableSensitiveDataLogging()
                .Options;
        }



        public AppDbContext GetContext()
        {
            if (string.IsNullOrEmpty(CurrentDbPath))
                throw new InvalidOperationException("Database path not set.");

            _options ??= BuildOptions(CurrentDbPath);
            return new AppDbContext(_options);
        }
    }
}
