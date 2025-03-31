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
    class EntryFormBehaviour: Behavior<Entry>
    {
        // BindableProperty for Items collection
        public static readonly BindableProperty ItemsProperty =
            BindableProperty.Create(nameof(Items), typeof(ObservableCollection<TreeLive>), typeof(EntryFormBehaviour));

        public ObservableCollection<TreeLive> Items
        {
            get => (ObservableCollection<TreeLive>)GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }

        public void AddNewItemOnEnterBehavior()
        {
            // You can bind the ObservableCollection to this property if needed
        }

        // Override OnBindingContextChanged to handle BindingContext changes
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            // Check if the BindingContext is available and set Items accordingly
            if (BindingContext is MainViewModel viewModel)
            {
                Items = viewModel.LiveTrees;
            }
        }
        protected override void OnAttachedTo(Entry bindable)
        {
            base.OnAttachedTo(bindable);

            // Attempt to access the parent BindingContext and set Items
            if (bindable.BindingContext is MainViewModel viewModel)
            {
                Items = viewModel.LiveTrees;
            }

            bindable.Completed += OnCompleted;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.Completed -= OnCompleted;
        }

        private void OnCompleted(object sender, EventArgs e)
        {
            var entry = sender as Entry;
            var currentItem = entry?.BindingContext as TreeLive;

            if (currentItem == null || Items == null) return;

            var currentIndex = Items.IndexOf(currentItem);

            if (currentIndex == Items.Count - 1)
            {
                // Create a new item if it's the last row
                Items.Add(new TreeLive { TreeID = 999 });
            }

            // Move focus to the next item
            MoveFocusToNextEntry(currentIndex + 1);
        }

        private void MoveFocusToNextEntry(int nextIndex)
        {
            if (nextIndex < Items.Count)
            {
                // Access the Entry by index
                var nextItem = Items[nextIndex];

                // You can manually trigger the focus movement in the next row
                // This requires accessing the Entry on the next row
                var nextEntry = nextItem; // Access the item that needs focus here (in a real-world app, you might access it by binding)

                // In MAUI, you would need a method for focusing an entry programmatically, such as:
                // nextEntry.Focus();
            }
        }
    }
}
