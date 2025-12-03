using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace PTANonCrown.Behaviours
{

    public class FocusBehavior : Behavior<Entry>
    {
        // Bindable property you can set from XAML or ViewModel
        public static readonly BindableProperty IsFocusedProperty =
            BindableProperty.Create(
                nameof(IsFocused),
                typeof(bool),
                typeof(FocusBehavior),
                false,
                propertyChanged: OnIsFocusedChanged);

        public bool IsFocused
        {
            get => (bool)GetValue(IsFocusedProperty);
            set => SetValue(IsFocusedProperty, value);
        }

        private static void OnIsFocusedChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is FocusBehavior behavior && behavior.AssociatedObject != null && (bool)newValue)
            {
                behavior.AssociatedObject.Focus();
                behavior.IsFocused = false; // reset so it can be triggered again
            }
        }

        protected override void OnAttachedTo(Entry bindable)
        {
            base.OnAttachedTo(bindable);
            AssociatedObject = bindable;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            base.OnDetachingFrom(bindable);
            AssociatedObject = null;
        }

        public Entry? AssociatedObject { get; private set; }
    }

}
