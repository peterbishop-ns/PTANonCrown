using PTANonCrown.ViewModel;

namespace PTANonCrown;

public partial class PlotPage : ContentPage
{
    public PlotPage(MainViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }


}