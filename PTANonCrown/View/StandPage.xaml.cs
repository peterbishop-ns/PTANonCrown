using PTANonCrown.ViewModel;
using PTANonCrown.Services;

namespace PTANonCrown;

public partial class StandPage : ContentPage
{
    public StandPage(MainViewModel viewModel)
    {
        AppLogger.Log($"StandPage", "App");


        InitializeComponent();

        BindingContext = viewModel;
    }
}