using PTANonCrown.ViewModel;
using PTANonCrown.Services;

namespace PTANonCrown
{
    public partial class AppShell : Shell
    {
        public AppShell(MainViewModel mainViewModel)
        {
            AppLogger.Log($"AppShell", "App");

            InitializeComponent();
            BindingContext = mainViewModel;

            this.Navigated += async (s, e) => await mainViewModel.OnShellNavigatedAsync(s, e);

        }
    }
}