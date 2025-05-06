
using DocumentFormat.OpenXml.Drawing;
using PTANonCrown.Data.Models;
using PTANonCrown.ViewModel;

namespace PTANonCrown;

public partial class LiveTreePage : ContentPage
{
    public LiveTreePage(MainViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;

    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        var vm = BindingContext as MainViewModel;

        if ((vm.CurrentPlot?.PlotTreeLive == null) | (vm.CurrentPlot?.PlotTreeLive.Count == 0)) {
            vm.AddNewTreeToPlot(vm.CurrentPlot, 1);
        }

    }



    private void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is Entry entry && entry.BindingContext is TreeLive treeRow)
        {
            // Get ViewModel
            var vm = BindingContext as MainViewModel;
            if (vm != null)
            {
                if (e.NewTextValue == string.Empty || e.NewTextValue is null)
                {
                    return;
                }
                // Get the filtered list based on user input
                var filteredResults = vm.LookupTrees
                        .Where(t => t.ShortCode.StartsWith(e.NewTextValue, StringComparison.OrdinalIgnoreCase) ||
                            t.Name.Contains(e.NewTextValue, StringComparison.OrdinalIgnoreCase))
                       .ToList();

                // Refresh the list in the UI based on the filtered list
                treeRow.TreeSpeciesFilteredList.Clear();
                foreach (var item in filteredResults)
                {
                    treeRow.TreeSpeciesFilteredList.Add(item);
                }

                // If there is a single match, choose it automatically
                if (filteredResults.Count == 1)
                {
                    treeRow.TreeSpecies = filteredResults.First();
                    treeRow.SearchSpecies = treeRow.TreeSpecies.ShortCode;

                    treeRow.TreeSpeciesFilteredList.Clear();
                }
                else if (filteredResults.Count == 0 && e.NewTextValue.Length == 2)
                {
                    {
                        treeRow.TreeSpecies = vm.LookupTrees.Where(t => t.Name.ToLower() == "unknown").FirstOrDefault();
                    }


                }
            }
        }
    }

    private void OnSuggestionSelected(object sender, TappedEventArgs e)
    {
        if (e.Parameter is TreeLive itemToUpdate && sender is Label label)
        {
            var selectedSpecies = label.BindingContext as TreeSpecies;
            if (selectedSpecies != null)
            {
                itemToUpdate.TreeSpecies = selectedSpecies;
                itemToUpdate.SearchSpecies = selectedSpecies.ShortCode;
                OnPropertyChanged(nameof(itemToUpdate));

            }
        }
    }
}