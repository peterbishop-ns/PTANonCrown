using PTANonCrown.Data.Models;
using System.Collections.ObjectModel;
using PTANonCrown.ViewModel;
namespace PTANonCrown.Behaviours
{
    public class EntryFormBehaviour : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.Completed += OnCompleted; // Handles Enter key
            bindable.HandlerChanged += OnHandlerChanged;


        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.Completed -= OnCompleted;
            bindable.HandlerChanged -= OnHandlerChanged;
        }

        private void OnHandlerChanged(object? sender, System.EventArgs e)
        {
#if WINDOWS
            if (sender is Entry entry && entry.Handler?.PlatformView is Microsoft.UI.Xaml.Controls.TextBox nativeEntry)
            {
                nativeEntry.KeyDown += (s, args) => OnKeyDown(entry, args);
            }
#endif
        }

#if WINDOWS
        private void OnKeyDown(Entry entry, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Tab)
            {
                e.Handled = true;
                MoveFocus(entry);
            }
        }
#endif

        private void OnCompleted(object? sender, System.EventArgs e)
        {
            if (sender is Entry entry)
            {
                MoveFocus(entry);
            }
        }

        private async void MoveFocus(Entry currentEntry)
        {
            // need to access the view model to get the LookupTrees
            var mainVM = Application.Current.MainPage.BindingContext as MainViewModel;


            var parentCollection = currentEntry.FindParent<CollectionView>();
            if (parentCollection == null) return;

            var entries = parentCollection.FindDescendants<Entry>().ToList();
            int currentIndex = entries.IndexOf(currentEntry);

            if (currentIndex >= 0 && currentIndex < entries.Count - 1)
            {
                entries[currentIndex + 1].Focus();
                entries[currentIndex + 1].SelectionLength = entries[currentIndex + 1].Text?.Length ?? 0;
            }
            else if (currentIndex == entries.Count - 1)
            {
                var collection = parentCollection.ItemsSource as ObservableCollection<TreeLive>;
                if (collection != null)
                {
                    int maxTreeNumber = collection.Max(t => t.TreeNumber);
                    collection.Add(new TreeLive() { TreeNumber = maxTreeNumber + 1, 
                    LookupTrees = mainVM.LookupTrees
                    });
                    mainVM.CurrentPlot.TreeCount = collection.Count();
                    parentCollection.ScrollTo(collection.Last(), position: ScrollToPosition.End);

                    // Wait for UI to update and then re-fetch the entries list
                    await MainThread.InvokeOnMainThreadAsync(async () =>
                    {
                        await Task.Delay(100); // allow for rendering

                        // Re-fetch entries after UI update
                        var updatedEntries = parentCollection.FindDescendants<Entry>().ToList();

                        // Focus the first entry in the new row
                        var newlyAddedEntry = updatedEntries.ElementAtOrDefault(currentIndex + 1);
                        if (newlyAddedEntry != null)
                        {
                            newlyAddedEntry.Focus();
                            newlyAddedEntry.SelectionLength = newlyAddedEntry.Text?.Length ?? 0;
                        }
                    });
                }
            }
        }

    }
}