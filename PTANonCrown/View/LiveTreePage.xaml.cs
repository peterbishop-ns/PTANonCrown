
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
        base.OnAppearing();
        _mainViewModel.InitializeFirstTree(_mainViewModel.CurrentPlot);
    }


    public LiveTreePage(MainViewModel viewModel)//, DbContext dbContext)
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

    private void Entry_Focused(object sender, FocusEventArgs e)
    {
        if (sender is Entry entry && !string.IsNullOrEmpty(entry.Text))
        {
            entry.CursorPosition = 0;
            entry.SelectionLength = entry.Text.Length;
        }
    }

   
}