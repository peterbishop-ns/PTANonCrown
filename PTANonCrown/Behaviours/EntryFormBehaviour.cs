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
            var mainVM = Application.Current.MainPage.BindingContext as MainViewModel;
            if (mainVM == null) return;

            var parentCollection = currentEntry.FindParent<CollectionView>();
            if (parentCollection == null) return;

            var entries = GetSortedEntries(parentCollection);
            int currentIndex = entries.IndexOf(currentEntry);

            //FIRST CASE: Tabbing over within a row; focus the next text box
            if (currentIndex >= 0 && currentIndex < entries.Count - 1)
            {
                // Make sure it is scrolled to the current item
                parentCollection.ScrollTo(currentIndex + 1);
                
                // Focus on the next Entry
                var nextEntry = entries[currentIndex + 1];
                nextEntry.Focus();
                nextEntry.SelectionLength = nextEntry.Text?.Length ?? 0;
            }

            // SECOND CASE: tabbing over at the end of a row
            // Create a new tree
            else if (currentIndex == entries.Count - 1 &&
                     parentCollection.ItemsSource is ObservableCollection<TreeLive> collection)
            {
                AddNewTree(collection, mainVM);
            }
        }


        private List<Entry> GetSortedEntries(CollectionView collectionView)
        {
            return collectionView
                    .FindDescendants<Entry>()
                    .OrderBy(e => ((TreeLive)e.BindingContext).TreeNumber)
                    .ToList();
        }

        private void AddNewTree(ObservableCollection<TreeLive> collection, MainViewModel mainVM)
        {
            int maxTreeNumber = collection.Max(t => t.TreeNumber);
            collection.Add(new TreeLive
            {
                TreeNumber = maxTreeNumber + 1,
                Plot = mainVM.CurrentPlot
            });
            mainVM.CurrentPlot.TreeCount = collection.Count;

            // Note: was having difficulty making it scroll down to the new tree.
            // Workaround was to put logic in the LiveTreePage.xaml.cs code-behind that detects changes to the collection of trees, regardless of how a tree is added
        }
    }
}