using CommunityToolkit.Maui;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PTANonCrown.Data.Context;
using PTANonCrown.Data.Repository;
using PTANonCrown.Data.Services;
using PTANonCrown.Services;
using PTANonCrown.ViewModel;
using System.Diagnostics;

namespace PTANonCrown
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            Services.AppLogger.Log($"CreateMauiApp - begnning", "MauiProgram.cs");

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            Services.AppLogger.Log($"AddSingleton", "MauiProgram.cs");

            builder.Services.AddSingleton<DatabaseService>();

            builder.Services.AddSingleton<StandPage>();
            builder.Services.AddSingleton<PlotPage>();
            builder.Services.AddSingleton<LiveTreePage>();
            builder.Services.AddSingleton<DeadTreePage>();
            builder.Services.AddSingleton<CoarseWoodyMaterialPage>();
            builder.Services.AddSingleton<SummaryPage>();

            //Repository
            builder.Services.AddSingleton<StandRepository>();
            builder.Services.AddSingleton<LookupRepository>();

            builder.Services.AddSingleton<MainViewModel>();

            //Services
            builder.Services.AddSingleton<LookupRefreshService>();
           // builder.Services.AddTransient<CsvLoader>();

            builder.Logging.AddDebug();

            // Register DbContext
            Services.AppLogger.Log($"DbContext - start", "MauiProgram.cs");


            //builder.Services.AddDbContext<AppDbContext>();

            Services.AppLogger.Log($"{FileSystem.AppDataDirectory}", "AddDbContext");
            Services.AppLogger.Log("AddDbContext - app", "MauiProgram");

            // builder.Services.AddDbContext<AppDbContext>(options =>
            // {
            //     var dbPath = Path.Combine(FileSystem.AppDataDirectory, "app.db");
            //      options.UseSqlite($"Filename={dbPath}");
            //  });


            // STEP 1: Get platform-specific path
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "app.db");

            builder.Services.AddSingleton(new DatabaseService(dbPath));

            Services.AppLogger.Log($"dbPath", dbPath);


            // STEP 2: Register DbContext with dependency injection
            builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) =>
            {
                var dbService = serviceProvider.GetRequiredService<DatabaseService>();
                options.UseSqlite($"Data Source={dbService.CurrentDbPath}");


            });


            var app = builder.Build();
            Services.AppLogger.Log($"DBContext end ", "MauiProgram.cs");


            // Apply pending EF Core migrations at runtime
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                //db.Database.EnsureDeleted();  // <-- add this temporarily
                try
                {

                    db.Database.Migrate();
                }

                catch (Exception ex)
                {
                    // catch startup issues
                    Debug.WriteLine("Unexpected EF error: " + ex);
                    Services.AppLogger.Log($"DB Exception", ex.ToString());

                }
            }

            Services.AppLogger.Log($"Services start", "MauiProgram.cs");
            /*
            // Ensure database is created
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.EnsureCreated();  // Creates the database if it doesn't exist
            }
            */

            Services.AppLogger.Log($"Services done", "MauiProgram.cs");

            return app;

        }

        public static async Task<string> CopyDatabaseAsync(string dbFileName)
        {
            Services.AppLogger.Log($"CopyDatabaseAsync - start", "MauiProgram.cs");

            string dbPath = Path.Combine(FileSystem.AppDataDirectory, dbFileName);

            if (!File.Exists(dbPath))
            {
                // Copy database from Resources/Raw folder to app data directory
                using var stream = await FileSystem.OpenAppPackageFileAsync(dbFileName);
                using var fileStream = new FileStream(dbPath, FileMode.Create, FileAccess.Write);
                await stream.CopyToAsync(fileStream);
            }
            Services.AppLogger.Log($"CopyDatabaseAsync - end", "MauiProgram.cs");

            return dbPath;
        }

    }


}