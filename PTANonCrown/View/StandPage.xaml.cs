using PTANonCrown.ViewModel;
namespace PTANonCrown;

public partial class StandPage : ContentPage
{
	public StandPage(MainViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }
}