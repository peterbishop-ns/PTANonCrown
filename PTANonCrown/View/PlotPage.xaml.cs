using PTANonCrown.ViewModel;
using PTANonCrown.Services;

namespace PTANonCrown;

public partial class PlotPage : ContentPage
{
    public PlotPage(MainViewModel viewModel)
    {

        InitializeComponent();
        BindingContext = viewModel;
    }

  
    private void OnlyIntegerAllowed(object sender, TextChangedEventArgs e)
    {
        var entry = (Entry)sender;
        string newText = new string(e.NewTextValue?.Where(char.IsDigit).ToArray() ?? Array.Empty<char>());

        if (entry.Text != newText)
            entry.Text = newText;
    }
}