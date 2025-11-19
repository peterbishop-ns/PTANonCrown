using PTANonCrown.ViewModel;
using PTANonCrown.Services;

namespace PTANonCrown;

public partial class StandPage : ContentPage
{
    private MainViewModel _mainViewModel { get; set; }

    public StandPage(MainViewModel viewModel)
    {
        AppLogger.Log($"StandPage", "App");


        InitializeComponent();

        BindingContext = viewModel;
        _mainViewModel = viewModel;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await _mainViewModel.GetOrCreateStandAsync();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        // Refresh UI bindings for error properties
        _mainViewModel.RefreshErrors();
    }

}