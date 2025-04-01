using PTANonCrown.ViewModel;
namespace PTANonCrown
{
    public partial class App : Application
    {
        public App(MainViewModel mainViewModel)
        {
            InitializeComponent();

            MainPage = new AppShell(mainViewModel);
        }
    }
}
