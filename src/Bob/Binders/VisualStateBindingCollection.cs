using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Bob.Binders
{
    public class VisualStateBindingCollection : AttachableCollection<VisualStateBinding>
    {
        public static readonly DependencyProperty DataContextProperty =
            DependencyProperty.Register(
                "DataContext", 
                typeof(object), 
                typeof(VisualStateBindingCollection), 
                new PropertyMetadata(default(object), (d, a) => ((VisualStateBindingCollection)d).OnDataContextChanged(a)));

        public object DataContext
        {
            get { return GetValue(DataContextProperty); }
            set { SetValue(DataContextProperty, value); }
        }

        public VisualStateBindingCollection()
        {
            //Bind the DataContext property to the attached element's DataContext (this effectively
            //sets the property to {Binding})
            var binding = new Binding();
            BindingOperations.SetBinding(this, DataContextProperty, binding);
        }

        private void OnDataContextChanged(DependencyPropertyChangedEventArgs e)
        {
            //YAY for doing binding manually because the binding system is retarded
            //(Can't just set up a binding from this.DataContext to item.DataContext)
            if (e.NewValue != e.OldValue)
            {
                foreach (var item in this)
                {
                    item.DataContext = e.NewValue;
                }
            }
        }

        internal override void ItemAdded(VisualStateBinding item)
        {
            if (AssociatedObject != null)
            {
                item.Attach(AssociatedObject);
            }
        }

        internal override void ItemRemoved(VisualStateBinding item)
        {
            if (AssociatedObject != null)
            {
                item.Detach();
            }
        }

        protected override void OnAttached()
        {
            foreach (VisualStateBinding binding in this)
                binding.Attach(AssociatedObject);
        }

        protected override void OnDetaching()
        {
            foreach (VisualStateBinding binding in this)
                binding.Detach();
        }
    }
}