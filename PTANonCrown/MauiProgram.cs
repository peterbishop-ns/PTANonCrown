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
            builder.Services.AddTransient<StandPage>();
            builder.Services.AddTransient<PlotPage>();
            builder.Services.AddTransient<LiveTreePage>();
            builder.Services.AddTransient<DeadTreePage>();
            builder.Services.AddTransient<CoarseWoodyMaterialPage>();
            builder.Services.AddTransient<SummaryPage>();

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

            var templateFilePath = Path.Combine(FileSystem.AppDataDirectory, "template.pta");

            // Ensure the folder exists
            Directory.CreateDirectory(FileSystem.AppDataDirectory);

            File.WriteAllText(Path.Combine(FileSystem.AppDataDirectory, "test.txt"), "hello");

            if (!File.Exists(templateFilePath))
            {
                var templateOptions = new DbContextOptionsBuilder<AppDbContext>()
                    .UseSqlite($"Data Source={templateFilePath}")
                    .Options;

                using var templateDb = new AppDbContext(templateOptions);
                templateDb.Database.EnsureCreated();
            }

            // ------------------------
            // Populate the lookups
            // ------------------------
            var lookupService = new LookupRefreshService(new DatabaseService(templateFilePath));
            lookupService.RefreshLookupsAsync();

           


            // -------------------------
            // Special code to intercept attempt to close, so we can check if there are unsaved changes

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
