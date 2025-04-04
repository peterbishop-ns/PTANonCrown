using PTANonCrown.Models;
using System.Collections.ObjectModel;

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

        private void MoveFocus(Entry currentEntry)
        {
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
                // Assuming your collection is a list, for example
                var collection = parentCollection.ItemsSource as ObservableCollection<TreeLive>;
                if (collection != null)
                {

                    int maxTreeNumber = collection.Max(t => t.TreeNumber);
                    // Add a new item to the collection
                    collection.Add(new TreeLive() { TreeNumber = maxTreeNumber + 1 });

                    // Optionally, you could focus the newly added entry here as well
                    var newEntry = entries.Last(); // or get the new entry dynamically

                    newEntry.Focus();
                    newEntry.SelectionLength = newEntry.Text?.Length ?? 0;
                    parentCollection.ScrollTo(collection.Last(), position: ScrollToPosition.End);

                }
            }

        }
    }
}