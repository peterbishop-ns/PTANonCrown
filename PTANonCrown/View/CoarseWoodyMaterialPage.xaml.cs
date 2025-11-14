using PTANonCrown.ViewModel;

namespace PTANonCrown;

public partial class CoarseWoodyMaterialPage : ContentPage
{

    private MainViewModel mainViewModel;
    public CoarseWoodyMaterialPage(MainViewModel viewModel)
    {
        InitializeComponent();
        mainViewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
       // mainViewModel.InitializeCoarseWoody(mainViewModel.CurrentPlot);
    }

    private void OnlyIntegerAllowed(object sender, TextChangedEventArgs e)
    {
        var entry = (Entry)sender;
        string newText = new string(e.NewTextValue?.Where(char.IsDigit).ToArray() ?? Array.Empty<char>());

        if (string.IsNullOrEmpty(newText))
            newText = "0";

        if (entry.Text != newText)
            entry.Text = newText;
    }
}