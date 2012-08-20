using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Bob.Binders
{
    public class VisualStateSwitchCase : DependencyObject, IAttachedObject
    {
        public string State { get; set; }
        public string Value { get; set; }

        public void Attach(DependencyObject dependencyObject)
        {
            AssociatedObject = dependencyObject;
        }

        public void Detach()
        {
            AssociatedObject = null;
        }

        public DependencyObject AssociatedObject { get; private set; }

        public void EvaluateValue(object value)
        {
            if (value.ToString() == Value)
            {
                var parentControl = FindParentControl(AssociatedObject);
                if (parentControl != null)
                    VisualStateManager.GoToState(parentControl, State, true);
            }
                
        }

        private Control FindParentControl(DependencyObject dependencyObject)
        {
            var findParentControl = dependencyObject as Control;
            if (findParentControl != null)
                return findParentControl;

            var parent = VisualTreeHelper.GetParent(dependencyObject);
            if (parent == null)
                return null;

            return FindParentControl(parent);
        }
    }
}