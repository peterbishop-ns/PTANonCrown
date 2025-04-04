using PTANonCrown.ViewModel;

namespace PTANonCrown;

public partial class SummaryPage : ContentPage
{
    public SummaryPage(MainViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }
}