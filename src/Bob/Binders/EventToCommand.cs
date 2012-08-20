using System.Windows.Input;
using Windows.UI.Xaml;

namespace Bob.Binders
{
    public class EventToCommand : EventBinding
    {
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(EventBinding), new PropertyMetadata(default(ICommand)));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        protected override void OnEventRaised(object sender, object e)
        {
            //Sets the DataContext of this control in order to make Bindings against 
            //Command evaluate correctly
            //No, I can't use binding for this, because it doesn't work for some reason
            object dataContext = ((FrameworkElement)sender).DataContext;
            if (DataContext != dataContext)
                DataContext = dataContext;

            if (Command != null)
                Command.Execute(e);
        }
    }
}