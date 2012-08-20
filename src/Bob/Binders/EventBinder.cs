using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Windows.UI.Xaml;

namespace Bob.Binders
{
    public class EventBinder
    {
        public static readonly DependencyProperty BindingsProperty =
            DependencyProperty.RegisterAttached("Bindings", typeof(ObservableCollection<EventBinding>), typeof(EventBinder), new PropertyMetadata(null));


        public static void SetBindings(UIElement element, ObservableCollection<EventBinding> value)
        {
            var existingCollection = (ObservableCollection<EventBinding>)element.GetValue(BindingsProperty);
            if (existingCollection != null)
                existingCollection.Clear(); //Cause all existing bound events to be unbound

            if (value != null)
                value.CollectionChanged += (s, e) => OnCollectionChanged(e, element);

            element.SetValue(BindingsProperty, value);
        }

        public static ObservableCollection<EventBinding> GetBindings(UIElement element)
        {
            var collection = (ObservableCollection<EventBinding>)element.GetValue(BindingsProperty);
            if (collection == null)
            {
                collection = new ObservableCollection<EventBinding>();
                SetBindings(element, collection);
            }

            return collection;
        }

        private static void OnCollectionChanged(NotifyCollectionChangedEventArgs args, UIElement element)
        {
            if (args.OldItems != null)
                foreach (EventBinding item in args.OldItems)
                    item.UnbindFromEvent();

            if (args.NewItems != null)
                foreach (EventBinding item in args.NewItems)
                    item.BindToEvent(element);
        }
    }
}