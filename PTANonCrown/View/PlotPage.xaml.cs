using PTANonCrown.ViewModel;

namespace PTANonCrown;

public partial class PlotPage : ContentPage
{
    public PlotPage(MainViewModel viewModel)
    {

        try
        {
            InitializeComponent();
            // Other constructor logic
        }
        catch (Exception ex)
        {
            LogError("Constructor", ex);
        }
        
        //InitializeComponent();
        BindingContext = viewModel;
    }
    void LogError(string where, Exception ex)
    {
        var path = "c:\\temp\\crashlog.txt";
        File.AppendAllText(path, $"[{DateTime.Now}] {where}: {ex}\n");
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