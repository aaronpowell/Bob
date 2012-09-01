using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Bob.Binders
{
    public class VisualStateBinding : AttachableCollection<VisualStateSwitch>
    {
        private BindingObserver valueSourceBindingObserver;

        public static readonly DependencyProperty DataContextProperty =
            DependencyProperty.Register(
                "DataContext", 
                typeof(object), 
                typeof(VisualStateBinding), 
                new PropertyMetadata(default(object), (d, a) => ((VisualStateBinding)d).OnDataContextChanged(a)));

        public object DataContext
        {
            get { return GetValue(DataContextProperty); }
            set { SetValue(DataContextProperty, value); }
        }

        private void OnDataContextChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue)
            {
                CreateValueSourceBindingObserver();
            }
        }

        private void CreateValueSourceBindingObserver()
        {
            if (valueSourceBindingObserver != null)
                valueSourceBindingObserver.ValueChanged -= OnValueSourceValueChanged;

            valueSourceBindingObserver = new BindingObserver
                                             {
                                                 DataContext = DataContext
                                             };
            valueSourceBindingObserver.ValueChanged += OnValueSourceValueChanged;
            valueSourceBindingObserver.SetBinding(BindingObserver.ValueProperty, ValueSource);
        }

        private void OnValueSourceValueChanged(object sender, EventArgs e)
        {
            if (VisualTreeHelper.GetParent(AssociatedObject) == null)
            {
                ((FrameworkElement)AssociatedObject).Loaded += OnElementLoaded;
                return;
            }

            var value = valueSourceBindingObserver.Value;
            foreach (var @case in this)
            {
                @case.EvaluateValue(value);
            }
        }

        private void OnElementLoaded(object sender, RoutedEventArgs e)
        {
            OnValueSourceValueChanged(valueSourceBindingObserver, EventArgs.Empty);
            ((FrameworkElement)sender).Loaded -= OnElementLoaded;
        }

        public static readonly DependencyProperty ValueSourceProperty =
            DependencyProperty.Register(
                "ValueSource", 
                typeof(Binding), 
                typeof(object),
                new PropertyMetadata(DependencyProperty.UnsetValue, (d, a) => ((VisualStateBinding)d).OnValueSourceChanged(a)));

        public Binding ValueSource
        {
            get { return (Binding)GetValue(ValueSourceProperty); }
            set { SetValue(ValueSourceProperty, value); }
        }

        private void OnValueSourceChanged(DependencyPropertyChangedEventArgs e)
        {
            if (valueSourceBindingObserver == null)
                return; //Let the OnDataContextChanged method create it initially

            if (e.NewValue != e.OldValue)
                CreateValueSourceBindingObserver();
        }

        internal override void ItemAdded(VisualStateSwitch item)
        {
            if (AssociatedObject != null)
            {
                item.Attach(AssociatedObject);
            }
        }

        internal override void ItemRemoved(VisualStateSwitch item)
        {
            if (AssociatedObject != null)
            {
                item.Detach();
            }
        }

        protected override void OnAttached()
        {
            foreach (VisualStateSwitch binding in this)
                binding.Attach(AssociatedObject);
        }

        protected override void OnDetaching()
        {
            foreach (VisualStateSwitch binding in this)
                binding.Detach();

            valueSourceBindingObserver = null;
        }

        private sealed class BindingObserver : FrameworkElement
        {
            public static readonly DependencyProperty ValueProperty =
                DependencyProperty.Register(
                    "Value", 
                    typeof(object), 
                    typeof(BindingObserver), 
                    new PropertyMetadata(DependencyProperty.UnsetValue, (d, a) => ((BindingObserver)d).OnValueChanged(a)));

            public object Value
            {
                get { return GetValue(ValueProperty); }
                private set { SetValue(ValueProperty, value); }
            }

            private void OnValueChanged(DependencyPropertyChangedEventArgs args)
            {
                if (ValueChanged != null)
                    ValueChanged(this, EventArgs.Empty);
            }

            public event EventHandler ValueChanged;
        }
    }
}