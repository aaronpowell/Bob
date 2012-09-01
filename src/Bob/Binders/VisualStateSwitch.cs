using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Bob.Binders
{
    public abstract class VisualStateSwitch : DependencyObject, IAttachedObject
    {
        public string State { get; set; }
        public DependencyObject AssociatedObject { get; private set; }

        public void Attach(DependencyObject dependencyObject)
        {
            AssociatedObject = dependencyObject;
        }

        public void Detach()
        {
            AssociatedObject = null;
        }

        public virtual void EvaluateValue(object value)
        {
            if (!IsValueMatch(value)) return;

            var parentControl = FindParentControl(AssociatedObject);
            if (parentControl != null)
                VisualStateManager.GoToState(parentControl, State, true);
        }

        protected abstract bool IsValueMatch(object value);

        protected static Control FindParentControl(DependencyObject dependencyObject)
        {
            var findParentControl = dependencyObject as Control;
            if (findParentControl != null)
                return findParentControl;

            var parent = VisualTreeHelper.GetParent(dependencyObject);
            return parent == null ? null : FindParentControl(parent);
        }
    }
}