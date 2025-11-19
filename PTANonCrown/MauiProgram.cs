using CommunityToolkit.Maui;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using PTANonCrown.Data.Context;
using PTANonCrown.Data.Repository;
using PTANonCrown.Data.Services;
using PTANonCrown.Services;
using PTANonCrown.ViewModel;
#if WINDOWS
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using WinRT.Interop;
#endif
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
            // ------------------------
            // Template file path
            // ------------------------
            var templateFilePath = Path.Combine(FileSystem.AppDataDirectory, "template.pta");

            Directory.CreateDirectory(FileSystem.AppDataDirectory);

            // Only create template if it doesn't exist
            if (!File.Exists(templateFilePath))
            {
                // Build options for template DB
                var templateOptions = new DbContextOptionsBuilder<AppDbContext>()
                    .UseSqlite($"Data Source={templateFilePath}")
                    .Options;

                

                // Create the template DB context
                using var templateDb = new AppDbContext(templateOptions);

                // Apply any pending migrations
                templateDb.Database.Migrate();

                // ------------------------
                // Populate the lookups
                // ------------------------
                var lookupService = new LookupRefreshService(new DatabaseService(templateFilePath));
                lookupService.RefreshLookupsAsync();

            }

            // todo - should not be running migrations in MauiProgram.cs. Apparently this is better done in App.xaml.cs
            // todo - lookupservice should not instantiate a new Database service; it shouold use the one from DI


            // -------------------------
            // 
            // --------------------------

            builder.ConfigureLifecycleEvents(events =>
            {
#if WINDOWS
                events.AddWindows(windows =>
                {
                    windows.OnWindowCreated(window =>
                    {
                        IntPtr hwnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
                        WindowId windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hwnd);
                        AppWindow appWindow = AppWindow.GetFromWindowId(windowId);

                        appWindow.Closing += async (sender, args) =>
                        {
                            args.Cancel = true; // stop immediate close

                            if (Microsoft.Maui.Controls.Application.Current?.MainPage?.BindingContext is MainViewModel vm)
                            {
                                bool canClose = await vm.HandleUnsavedChangesOnExitAsync();
                                if (canClose)
                                    appWindow.Destroy(); // manually close
                            }
                        };
                    });
                });
#endif
            });
  

            // ------------------------
            // Copy template to working DB
            // ------------------------
            var workingFilePath = Path.Combine(FileSystem.AppDataDirectory, $"working_{Guid.NewGuid()}.pta");
            File.Copy(templateFilePath, workingFilePath, overwrite: true);
            

            // Register AppDbContext properly
            // ------------------------
            builder.Services.AddDbContext<AppDbContext>((sp, options) =>
            {
                var dbService = sp.GetRequiredService<DatabaseService>();
                dbService.SetDatabasePath(workingFilePath);
                dbService.DbIsNew = true;
                options.UseSqlite($"Data Source={workingFilePath}");
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
