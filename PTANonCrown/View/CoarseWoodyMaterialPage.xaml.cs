using PTANonCrown.ViewModel;

namespace PTANonCrown;

public partial class CoarseWoodyMaterialPage : ContentPage
{
    public CoarseWoodyMaterialPage(MainViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
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