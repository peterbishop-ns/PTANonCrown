
using DocumentFormat.OpenXml.Drawing;
using Microsoft.EntityFrameworkCore;
using PTANonCrown.Data.Models;
using PTANonCrown.ViewModel;
using System.Collections.ObjectModel;
using System.Runtime.Intrinsics.X86;
using System.Collections.Specialized;

namespace PTANonCrown;

public partial class LiveTreePage : ContentPage
{

    private MainViewModel _mainViewModel;
    protected override void OnAppearing()
    {
        base.OnAppearing();
        _mainViewModel.InitializeFirstTree(_mainViewModel.CurrentPlot);
        // Unsubscribe before re-subscribing (in case OnAppearing gets called repeatedly)
        if (_mainViewModel.TreeRows is ObservableCollection<TreeLiveViewModel> treeCollection)
        {
            treeCollection.CollectionChanged -= TreeCollection_CollectionChanged;
            treeCollection.CollectionChanged += TreeCollection_CollectionChanged;
        }


        _mainViewModel.TreeRows.Clear();

        foreach (var tree in _mainViewModel.CurrentPlot.PlotTreeLive)
        {
            var newRow = new TreeLiveViewModel(tree, _mainViewModel.LookupTreeSpecies);
            newRow.FilteredSpecies.Clear();
            _mainViewModel.TreeRows.Add(newRow);
        }
            
    }
    private void TreeCollection_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems != null)
        {
            foreach (TreeLiveViewModel newTree in e.NewItems)
            {
                TreeCollectionView.ScrollTo(newTree, position: ScrollToPosition.End, animate: true);
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    newTree.FocusSpecies = true;
                });
            }
        }
    }

    

    private void SpeciesEntry_Completed(object sender, EventArgs e)
    {
        if (sender is Entry entry && entry.BindingContext is TreeLiveViewModel rowVm)
        {
            // If there is at least one filtered species, select the first one
            if (rowVm.FilteredSpecies.Any())
            {
                rowVm.SelectSpecies(rowVm.FilteredSpecies.First());
            }

            // Optionally dismiss the keyboard
            entry.Unfocus();
        }
    }

    void OnSpeciesTapped(object sender, TappedEventArgs e)
    {
        var species = (TreeSpecies)e.Parameter;

        // Find the TreeLiveViewModel for this row
        var rowVm = (sender as BindableObject).BindingContext as TreeLiveViewModel;
        rowVm?.SelectSpecies(species);
    }

    public LiveTreePage(MainViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _mainViewModel = viewModel;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        _mainViewModel.RefreshErrors();

    }
    private Entry? _activeEntry;

    private void SpeciesEntry_Focused(object sender, FocusEventArgs e)
    {
        _activeEntry = sender as Entry;


    }
    private void Entry_Focused(object sender, FocusEventArgs e)
    {
        if (sender is Entry entry && !string.IsNullOrEmpty(entry.Text))
        {
            entry.CursorPosition = 0;            // move cursor to start
            entry.SelectionLength = entry.Text.Length;  // select all text
        }
    }



}