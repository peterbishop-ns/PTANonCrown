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
}