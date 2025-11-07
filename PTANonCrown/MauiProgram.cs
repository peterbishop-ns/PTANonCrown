using CommunityToolkit.Maui;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PTANonCrown.Data.Context;
using PTANonCrown.Data.Repository;
using PTANonCrown.Data.Services;
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

            builder.Logging.AddDebug();

            // ------------------------
            // Pages
            // ------------------------
            builder.Services.AddSingleton<StandPage>();
            builder.Services.AddSingleton<PlotPage>();
            builder.Services.AddSingleton<LiveTreePage>();
            builder.Services.AddSingleton<DeadTreePage>();
            builder.Services.AddSingleton<CoarseWoodyMaterialPage>();
            builder.Services.AddSingleton<SummaryPage>();

            // ------------------------
            // Repositories
            // ------------------------
            builder.Services.AddSingleton<StandRepository>();
            builder.Services.AddSingleton<LookupRepository>();

            // ------------------------
            // ViewModels
            // ------------------------
            builder.Services.AddSingleton<MainViewModel>();

            // ------------------------
            // Services
            // ------------------------
            builder.Services.AddSingleton<DatabaseService>();
            builder.Services.AddSingleton<LookupRefreshService>();

            // ------------------------
            // Create template DB if needed
            // ------------------------
            var templateFilePath = Path.Combine(FileSystem.CacheDirectory, "template.db");
            if (!File.Exists(templateFilePath))
            {
                var templateOptions = new DbContextOptionsBuilder<AppDbContext>()
                    .UseSqlite($"Data Source={templateFilePath}")
                    .Options;

                using var templateDb = new AppDbContext(templateOptions);
                templateDb.Database.Migrate();
            }

            // ------------------------
            // Copy template to working DB
            // ------------------------
            var workingFilePath = Path.Combine(FileSystem.CacheDirectory, "working.db");
            File.Copy(templateFilePath, workingFilePath, overwrite: true);

            // ------------------------
            // Register AppDbContext properly
            // ------------------------
            builder.Services.AddDbContext<AppDbContext>((sp, options) =>
            {
                var dbService = sp.GetRequiredService<DatabaseService>();
                options.UseSqlite($"Data Source={workingFilePath}");
                dbService.SetDatabasePath(workingFilePath);
                dbService.DbIsNew = true;
            });

            // ------------------------
            // Build the app
            // ------------------------
            var app = builder.Build();

            // ------------------------
            // Optional: ensure DB created
            // ------------------------
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.EnsureCreated();
            }

            return app;
        }
    }
}
