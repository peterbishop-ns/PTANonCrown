
using DocumentFormat.OpenXml.Drawing;
using Microsoft.EntityFrameworkCore;
using PTANonCrown.Data.Models;
using PTANonCrown.ViewModel;
using System.Collections.ObjectModel;
using System.Runtime.Intrinsics.X86;

namespace PTANonCrown;

public partial class LiveTreePage : ContentPage
{

    private MainViewModel _mainViewModel;
    protected override void OnAppearing()
    {
        _mainViewModel.InitializeFirstTree(_mainViewModel.CurrentPlot);
    }

    public LiveTreePage(MainViewModel viewModel)//, DbContext dbContext)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _mainViewModel = viewModel;
        if (BindingContext is MainViewModel vm)
        {
            if (vm.CurrentPlot?.PlotTreeLive is ObservableCollection<TreeLive> treeCollection)
            {
                treeCollection.CollectionChanged += TreeCollection_CollectionChanged;
            }
        }
    }

    private void TreeCollection_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
        {
            // Scroll to the last item whenever new tree is added
            if (sender is ObservableCollection<TreeLive> collection && collection.Count > 0)
            {
                var lastItem = collection.Last();
                // MainThread lambda marked async
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Task.Delay(300); // give UI time to render
                    TreeCollectionView.ScrollTo(collection.Count-1, position: ScrollToPosition.End, animate: true);
                    // Find the Entry inside the last item's container
                    var entries = TreeCollectionView.FindDescendants<Entry>().ToList();
                    var secondEntry = entries.ElementAtOrDefault(entries.Count() - 3); // index 1 = second item
                    secondEntry?.Focus();
                });

            }
        }
    }




    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _mainViewModel.RefreshErrors();

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