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

    private void OnSuggestionSelected(object sender, TappedEventArgs e)
    {
        if (e.Parameter is TreeLive itemToUpdate && sender is Label label)
        {
            var selectedSpecies = label.BindingContext as TreeLookup;
            if (selectedSpecies != null)
            {
                itemToUpdate.TreeLookup = selectedSpecies;
                OnPropertyChanged(nameof(itemToUpdate));
            }
        }
    }
}