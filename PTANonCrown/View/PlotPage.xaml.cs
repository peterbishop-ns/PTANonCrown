using PTANonCrown.ViewModel;
using PTANonCrown.Services;

namespace PTANonCrown;

public partial class PlotPage : ContentPage
{
    private MainViewModel _mainViewModel {  get; set; }
    public PlotPage(MainViewModel viewModel)
    {

        InitializeComponent();
        BindingContext = viewModel;
        _mainViewModel = viewModel;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();


        //await _mainViewModel.GetOrCreateStandAsync();

        _mainViewModel.GetOrCreatePlot(_mainViewModel.CurrentStand);

        _mainViewModel.PopulateUiTreatments(_mainViewModel.CurrentPlot);

    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _mainViewModel.PopulatePlotFromUi(_mainViewModel.CurrentPlot);
        _mainViewModel.RefreshErrors();

    }
    private void OnlyIntegerAllowed(object sender, TextChangedEventArgs e)
    {
        var entry = (Entry)sender;
        string newText = new string(e.NewTextValue?.Where(char.IsDigit).ToArray() ?? Array.Empty<char>());

        if (entry.Text != newText)
            entry.Text = newText;
    }
}