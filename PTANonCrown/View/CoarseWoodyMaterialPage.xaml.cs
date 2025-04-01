using PTANonCrown.ViewModel;

namespace PTANonCrown;

public partial class CoarseWoodyMaterialPage : ContentPage
{
	public CoarseWoodyMaterialPage(MainViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }
}