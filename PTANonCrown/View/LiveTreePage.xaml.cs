
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

    private void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is Entry entry && entry.BindingContext is TreeLive treeRow)
        {
            // Get ViewModel
            var vm = BindingContext as MainViewModel;
            if (vm != null)
            {
                // Get the filtered list based on user input
                var filteredResults = vm.LookupTrees
                        .Where(t => t.ShortCode.StartsWith(e.NewTextValue, StringComparison.OrdinalIgnoreCase) ||
                            t.Name.Contains(e.NewTextValue, StringComparison.OrdinalIgnoreCase))
                       .ToList();

                // Refresh the list in the UI based on the filtered list
                treeRow.TreeLookupFilteredList.Clear();
                foreach (var item in filteredResults)
                {
                    treeRow.TreeLookupFilteredList.Add(item);
                }

                // If there is a single match, choose it automatically
                if (filteredResults.Count == 1)
                {
                    treeRow.TreeLookup = filteredResults.First();
                    treeRow.SearchSpecies = treeRow.TreeLookup.ShortCode;

                    treeRow.TreeLookupFilteredList.Clear();
                }
                else
                {
                    treeRow.TreeLookup = null;
                }


            }
        }
    }

    private void OnSuggestionSelected(object sender, TappedEventArgs e)
    {
        if (e.Parameter is TreeLive itemToUpdate && sender is Label label)
        {
            var selectedSpecies = label.BindingContext as TreeLookup;
            if (selectedSpecies != null)
            {
                itemToUpdate.TreeLookup = selectedSpecies;
                itemToUpdate.SearchSpecies = selectedSpecies.ShortCode;
                OnPropertyChanged(nameof(itemToUpdate));

            }
        }
    }
}