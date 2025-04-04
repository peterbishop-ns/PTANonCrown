using PTANonCrown.ViewModel;

namespace PTANonCrown;

public partial class DeadTreePage : ContentPage
{
    public DeadTreePage(MainViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }
}