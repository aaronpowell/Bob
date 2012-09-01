using System;
using Windows.UI.Xaml;

namespace Bob.Binders
{
    public static class VisualStateBinder
    {
        public static readonly DependencyProperty BindingsProperty =
            DependencyProperty.RegisterAttached(
                "Bindings", 
                typeof(VisualStateBindingCollection), 
                typeof(VisualStateBinder), 
                new PropertyMetadata(default(VisualStateBinding), OnBindingsChanged));

        public static VisualStateBindingCollection GetBindings(UIElement element)
        {
            var bindingCollection = element.GetValue(BindingsProperty) as VisualStateBindingCollection;

            if (bindingCollection == null)
            {
                bindingCollection = new VisualStateBindingCollection();
                element.SetValue(BindingsProperty, bindingCollection);
            }

            return bindingCollection;
        }

        public static void SetBindings(UIElement element, VisualStateBindingCollection value)
        {
            element.SetValue(BindingsProperty, value);
        }

        private static void OnBindingsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var oldValue = e.OldValue as VisualStateBindingCollection;
            var newValue = e.NewValue as VisualStateBindingCollection;

            if (oldValue == newValue) return;

            if (oldValue != null && oldValue.AssociatedObject != null)
            {
                oldValue.Detach();
            }

            if (newValue != null && d != null)
            {
                if (newValue.AssociatedObject != null)
                {
                    throw new InvalidOperationException("Too many");
                }

                newValue.Attach(d);
            }
        }
    }
}