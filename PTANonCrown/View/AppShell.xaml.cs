using PTANonCrown.ViewModel;

namespace PTANonCrown
{
    public partial class AppShell : Shell
    {
        public AppShell(MainViewModel mainViewModel)
        {
            InitializeComponent();
            BindingContext = mainViewModel;
        }
    }
}
