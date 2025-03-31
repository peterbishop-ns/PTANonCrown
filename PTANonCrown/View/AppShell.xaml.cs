using PTANonCrown.ViewModel;

namespace PTANonCrown
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            BindingContext = new AppShellViewModel();
        }
    }
}
