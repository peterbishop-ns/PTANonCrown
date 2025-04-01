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
            bindable.Completed += OnCompleted;
        }

        private void OnCompleted(object? sender, EventArgs e)
        {
            if (sender is not Entry entry)
                return;

            // Find parent CollectionView
            var parentCollection = entry.FindParent<CollectionView>();
            if (parentCollection == null)
                return;

            // Find all entries inside CollectionView
            var entries = parentCollection
                .FindDescendants<Entry>()
                .ToList();

          //  if (entries.Count == 0)
          //
            // Find the index of the current Entry
            int currentIndex = entries.IndexOf(entry);

            if (currentIndex >= 0 && currentIndex < entries.Count - 1)
            {
                // Move focus to next Entry
                entries[currentIndex + 1].Focus();
            }
            else
            {
                // Optionally, handle wrap-around (focus back to first Entry)
                entries[0].Focus();
            }
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.Completed -= OnCompleted;
        }


    }
}