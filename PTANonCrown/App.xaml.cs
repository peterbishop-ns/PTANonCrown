using PTANonCrown.Data.Services;
using PTANonCrown.Services;
using PTANonCrown.ViewModel;
using System.Diagnostics;

namespace PTANonCrown
{
    public partial class App : Application
    {
        private readonly MainViewModel _mainViewModel;
        private readonly LookupRefreshService _lookupRefreshService;

        public App(MainViewModel mainViewModel, LookupRefreshService lookupRefreshService)
        {
         //   _ = Task.Run(async () => await lookupRefreshService.RefreshLookupsAsync());


            Services.AppLogger.Log($"Starting App", "App");
            InitializeComponent();

            // Subscribe to events that allow logging
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
            _mainViewModel = mainViewModel;
            _lookupRefreshService = lookupRefreshService;

            _ = InitializeAsync();

            
            // Subscribe to event that allows navigation
            // Shell.Current.Navigated += OnShellNavigated;
        }
        private async Task InitializeAsync()
        {
            try
            {
                // Refresh lookups safely
                await _lookupRefreshService.RefreshLookupsAsync();

                // Make sure viewmodel has the data
                _mainViewModel.LoadLookupTables();

                // Once complete, show the main page
                MainPage = new AppShell(_mainViewModel);
            }
            catch (Exception ex)
            {
                // Log exception
                Services.AppLogger.Log($"Error during lookup refresh: {ex}", "App");
                throw;
            }
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