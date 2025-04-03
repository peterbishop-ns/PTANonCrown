using PTANonCrown.ViewModel;
using PTANonCrown.Services;
using Microsoft.Maui.Controls;
//using AndroidX.Lifecycle;
using PTANonCrown.Models;
using Syncfusion.Maui.Core.Internals;

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
                // Filter lookup list based on user input
                var filteredResults = vm.LookupTrees
                        .Where(t => t.ShortCode.Contains(e.NewTextValue, StringComparison.OrdinalIgnoreCase) ||
                            t.Name.Contains(e.NewTextValue, StringComparison.OrdinalIgnoreCase))
                       .ToList();

                // Update the ViewModel (or another property) with the filtered list
                treeRow.TreeLookupFilteredList.Clear();
                foreach (var item in filteredResults)
                {
                    treeRow.TreeLookupFilteredList.Add(item);
                }

               if (filteredResults.Count == 1)
                {
                    treeRow.TreeLookup = filteredResults.First();
                    treeRow.SearchSpecies = $"{treeRow.TreeLookup.ShortCode} - {treeRow.TreeLookup.Name}";

                    treeRow.TreeLookupFilteredList.Clear();
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
                itemToUpdate.SearchSpecies = $"{selectedSpecies.ShortCode} - {selectedSpecies.Name}";
                OnPropertyChanged(nameof(itemToUpdate));
            }
        }
    }
}