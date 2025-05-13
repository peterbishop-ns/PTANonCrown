using CommunityToolkit.Maui;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PTANonCrown.Data.Context;
using PTANonCrown.Data.Repository;
using PTANonCrown.Services;
using PTANonCrown.ViewModel;

namespace PTANonCrown
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

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


            builder.Services.AddDbContext<AppDbContext>();

            CopyDatabaseAsync("lookup.db");

            builder.Services.AddDbContext<LookupDbContext>(options =>
            {
                var dbPath = Path.Combine(FileSystem.AppDataDirectory, "lookup.db");
                options.UseSqlite($"Filename={dbPath}");
            });

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                var dbPath = Path.Combine(FileSystem.AppDataDirectory, "app.db");
                options.UseSqlite($"Filename={dbPath}");
            });
            var app = builder.Build();

            // Ensure database is created
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.EnsureCreated();  // Creates the database if it doesn't exist
            }

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<LookupDbContext>();
                db.Database.EnsureCreated();  // Creates the database if it doesn't exist
            }

            return app;

        }

        public static async Task<string> CopyDatabaseAsync(string dbFileName)
        {
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, dbFileName);

            if (!File.Exists(dbPath))
            {
                // Copy database from Resources/Raw folder to app data directory
                using var stream = await FileSystem.OpenAppPackageFileAsync(dbFileName);
                using var fileStream = new FileStream(dbPath, FileMode.Create, FileAccess.Write);
                await stream.CopyToAsync(fileStream);
            }

            return dbPath;
        }

    }
}