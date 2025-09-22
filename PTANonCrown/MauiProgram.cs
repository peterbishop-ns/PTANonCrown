using CommunityToolkit.Maui;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PTANonCrown.Data.Context;
using PTANonCrown.Data.Repository;
using PTANonCrown.Services;
using PTANonCrown.ViewModel;
using System.Diagnostics;

namespace PTANonCrown
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            AppLogger.Log($"CreateMauiApp - begnning", "MauiProgram.cs");

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            AppLogger.Log($"AddSingleton", "MauiProgram.cs");

            builder.Services.AddSingleton<MainService>();

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

            builder.Logging.AddDebug();

            // Register DbContext
            AppLogger.Log($"DbContext - start", "MauiProgram.cs");


            builder.Services.AddDbContext<AppDbContext>();

            AppLogger.Log($"{FileSystem.AppDataDirectory}", "AddDbContext");
            AppLogger.Log("AddDbContext - app", "MauiProgram");

           // builder.Services.AddDbContext<AppDbContext>(options =>
           // {
           //     var dbPath = Path.Combine(FileSystem.AppDataDirectory, "app.db");
          //      options.UseSqlite($"Filename={dbPath}");
          //  });


            // STEP 1: Get platform-specific path
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "app.db");

            AppLogger.Log($"dbPath", dbPath);


            // STEP 2: Register DbContext with dependency injection
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite($"Data Source={dbPath}"));


            var app = builder.Build();
            AppLogger.Log($"DBContext end ", "MauiProgram.cs");


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
                    AppLogger.Log($"DB Exception", ex.ToString());

                }
            }

            AppLogger.Log($"Services start", "MauiProgram.cs");
            /*
            // Ensure database is created
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.EnsureCreated();  // Creates the database if it doesn't exist
            }
            */
          
            AppLogger.Log($"Services done", "MauiProgram.cs");

            return app;

        }

        public static async Task<string> CopyDatabaseAsync(string dbFileName)
        {
            AppLogger.Log($"CopyDatabaseAsync - start", "MauiProgram.cs");

            string dbPath = Path.Combine(FileSystem.AppDataDirectory, dbFileName);

            if (!File.Exists(dbPath))
            {
                // Copy database from Resources/Raw folder to app data directory
                using var stream = await FileSystem.OpenAppPackageFileAsync(dbFileName);
                using var fileStream = new FileStream(dbPath, FileMode.Create, FileAccess.Write);
                await stream.CopyToAsync(fileStream);
            }
            AppLogger.Log($"CopyDatabaseAsync - end", "MauiProgram.cs");

            return dbPath;
        }

    }
}