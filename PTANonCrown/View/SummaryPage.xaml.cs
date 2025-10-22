using CommunityToolkit.Maui.Storage;
using PTANonCrown.ViewModel;

namespace PTANonCrown;

public partial class SummaryPage : ContentPage
{
    public SummaryPage(MainViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }

    void ShowSoil(object sender, EventArgs e)
    {
        MainSummary.IsVisible = false;
        SoilSummary.IsVisible = true;
        
        VegetationSummary.IsVisible = false;
        TreatmentSummary.IsVisible = false;
        SpeciesSummary.IsVisible = false;
    }

    void ShowVegetation(object sender, EventArgs e)
    {
        MainSummary.IsVisible = false;
        SoilSummary.IsVisible = false;
        VegetationSummary.IsVisible = true;
        TreatmentSummary.IsVisible = false;
        SpeciesSummary.IsVisible = false;
    }

    void ShowTreatment(object sender, EventArgs e)
    {
        MainSummary.IsVisible = false;
        SoilSummary.IsVisible = false;
        VegetationSummary.IsVisible = false;
        TreatmentSummary.IsVisible = true;
        SpeciesSummary.IsVisible = false;
    }

    void ShowMain(object sender, EventArgs e)
    {
        MainSummary.IsVisible = true;
        SoilSummary.IsVisible = false;
        VegetationSummary.IsVisible = false;
        TreatmentSummary.IsVisible = false;
        SpeciesSummary.IsVisible = false;

    }    
    
    void ShowSpecies(object sender, EventArgs e)
    {
        MainSummary.IsVisible = false;
        SoilSummary.IsVisible = false;
        VegetationSummary.IsVisible = false;
        TreatmentSummary.IsVisible = false;
        SpeciesSummary.IsVisible = true;

    }

    private async void OnButtonClicked(object sender, EventArgs e)
    {
        try
        {

            // Pick a folder from the file system
            var folder = await FolderPicker.PickAsync(default);

            if (folder != null)
            {
                // Extract folder information
                string folderPath = $"Folder Name: {folder.Folder}";

                // Show folder info in a display alert
                await DisplayAlert("Folder Selected", folderPath, "OK");
            }
            else
            {
                // Handle case when no folder is picked
                await DisplayAlert("No Folder", "No folder was selected.", "OK");
            }
        }
        catch (Exception ex)
        {
            // Handle potential errors, like permission issues
            await DisplayAlert("Error", "An error occurred: " + ex.Message, "OK");
        }
    }
}