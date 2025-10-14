using PTANonCrown.Data.Services;
using PTANonCrown.Services;
using PTANonCrown.ViewModel;
using System.Diagnostics;

namespace PTANonCrown
{
    public partial class App : Application
    {
        public App(MainViewModel mainViewModel, LookupRefreshService lookupRefreshService)
        {
            Services.AppLogger.Log($"Starting App", "App");
            InitializeComponent();

            // Subscribe to events that allow logging
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;

            // Fire-and-forget async refresh of lookups
            _ = Task.Run(async () => await lookupRefreshService.RefreshLookupsAsync());

            MainPage = new AppShell(mainViewModel);
        }
        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            LogException(e.ExceptionObject as Exception, "UnhandledException");

            // Optionally display a generic error page or message
        }

        private void OnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            LogException(e.Exception, "UnobservedTaskException");
            e.SetObserved();
        }

        private void LogException(Exception? ex, string context)
        {
            // Log to file, analytics service, or crash reporting tool
            //System.Diagnostics.Debug.WriteLine($"{context}: {ex?.Message}");
            //Trace.WriteLine();
            Services.AppLogger.Log($"{context}: {ex?.Message}", "App");
        }
    }
    
}