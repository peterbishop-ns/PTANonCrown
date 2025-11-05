
using DocumentFormat.OpenXml.Drawing;
using Microsoft.EntityFrameworkCore;
using PTANonCrown.Data.Models;
using PTANonCrown.ViewModel;
using System.Runtime.Intrinsics.X86;

namespace PTANonCrown;

public partial class LiveTreePage : ContentPage
{
    //private readonly DbContext _dbContext;


    public LiveTreePage(MainViewModel viewModel)//, DbContext dbContext)
    {
        InitializeComponent();
        BindingContext = viewModel;
     //   _dbContext = dbContext;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        var vm = BindingContext as MainViewModel;

        if ((vm.CurrentPlot?.PlotTreeLive == null) | (vm.CurrentPlot?.PlotTreeLive.Count == 0)) {
            vm.AddNewTreeToPlot(vm.CurrentPlot, 1);
        }
    }


    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        /*var vm = BindingContext as MainViewModel;

        var entry = _dbContext.Entry(vm.CurrentStand);
        if (entry.State == EntityState.Modified || entry.State == EntityState.Added)
        {
            // The entity exists in the database but some property values have changed
        }*/


    }

    private void Entry_Focused(object sender, FocusEventArgs e)
    {
        if (sender is Entry entry && !string.IsNullOrEmpty(entry.Text))
        {
            entry.CursorPosition = 0;
            entry.SelectionLength = entry.Text.Length;
        }
    }

    private void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is Entry entry && entry.BindingContext is TreeLive treeRow)
        {
            // Get ViewModel
           // var vm = BindingContext as MainViewModel;
           // if (vm != null)
           // {
                if (e.NewTextValue == string.Empty || e.NewTextValue is null)
                {
                    return;
                }

                var match = treeRow.LookupTrees.Where(t => t.ShortCode.ToLower() == e.NewTextValue.ToLower()).FirstOrDefault();

                if (match != null)
                {
                    treeRow.TreeSpecies = match;
                    treeRow.SearchSpecies = match.ShortCode;
                }

                if (match is null & e.NewTextValue.Length >= 2)
                {
                    treeRow.TreeSpecies = treeRow.LookupTrees.Where(t => t.Name.ToLower() == "unknown").FirstOrDefault();

                }

            //}
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