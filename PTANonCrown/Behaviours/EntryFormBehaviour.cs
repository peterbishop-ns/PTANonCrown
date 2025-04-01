using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PTANonCrown.Models;
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

        private void MoveFocus(Entry currentEntry)
        {
            var parentCollection = currentEntry.FindParent<CollectionView>();
            if (parentCollection == null) return;

            var entries = parentCollection.FindDescendants<Entry>().ToList();
            int currentIndex = entries.IndexOf(currentEntry);

            if (currentIndex >= 0 && currentIndex < entries.Count - 1)
            {
                entries[currentIndex + 1].Focus();
            }
        }
    }
}