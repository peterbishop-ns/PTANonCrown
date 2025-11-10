using Microsoft.EntityFrameworkCore;
using PTANonCrown.Data.Context;

namespace PTANonCrown.Data.Services
{
    public class DatabaseService
    {
        public string WorkingDBPath { get; private set; }
        public string SaveFilePath { get; private set; }
        public bool DbIsNew{ get; set; }
        private DbContextOptions<AppDbContext>? _options;
        private AppDbContext? _context;

        public DatabaseService(string? dbPath = null)
        {
            if (dbPath != null && dbPath != string.Empty)
            {
                AppLoggerData.Log($"Initiating DB path: {dbPath}", "DatabaseService");
                SetDatabasePath(dbPath);
            }
         
        }

        public void ResetContext()
        {
            // Dispose existing context if any
            _context?.Dispose();
            _context = null;

            // Clear any cached options if you want
            _options = null;
        }

        public void CreateNewDatabase(string templatePath)
        {
            if (!File.Exists(templatePath))
                throw new FileNotFoundException("Template database not found.", templatePath);

            // Get the directory of the template
            var baseDir = Path.GetDirectoryName(templatePath);
            if (string.IsNullOrEmpty(baseDir))
                throw new InvalidOperationException("Cannot determine template directory.");


            ResetContext();

            WorkingDBPath = Path.Combine(baseDir, $"working_{Guid.NewGuid()}.pta");
            SetDatabasePath(WorkingDBPath);
            
            // Overwrite the working databse with the template
            File.Copy(templatePath, WorkingDBPath, overwrite: true);

            // Set in DatabaseService
            DbIsNew = true;
        }

        

        public void SetSaveFilePath(string filePath)
        {
            SaveFilePath = filePath;
        }

         public void SetDatabasePath(string newPath)
        {
            AppLoggerData.Log($"Changing DB path: {newPath}", "DatabaseService");

            WorkingDBPath = newPath;

            // Dispose old context and rebuild
            _context?.Dispose();
            _context = null;
            _options = null;
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
            if (string.IsNullOrEmpty(WorkingDBPath))
                throw new InvalidOperationException("Database path not set.");

            var options = BuildOptions(WorkingDBPath);
            return new AppDbContext(options);
        }


    }
}
