using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace Bob.Services
{
    public class MessageDialogNotificationService : INotificationService
    {
        public virtual Task Notify(string message)
        {
            return Notify(message, null);
        }

        public virtual Task Notify(string message, string title)
        {
            return Notify(message, title, null);
        }

        public virtual Task Notify(string message, string title, string primaryOptionText, Action primaryOptionAction = null)
        {
            return Notify(message, title, primaryOptionText, null, primaryOptionAction);
        }

        public async virtual Task Notify(string message, string title, string primaryOptionText, string secondaryOptionText, Action primaryOptionAction = null, Action secondaryOptionAction = null)
        {
            if(Application.Current.Resources.ContainsKey(message))
                message = (string) Application.Current.Resources[message];

            if(Application.Current.Resources.ContainsKey(title))
                title = (string) Application.Current.Resources[title];

            var dialog = new MessageDialog(message, title);

            if (!string.IsNullOrEmpty(primaryOptionText))
            {
                primaryOptionText = (string) Application.Current.Resources[primaryOptionText];
                var command = new UICommand(primaryOptionText);
                if (primaryOptionAction != null) command.Invoked = uiCommand => primaryOptionAction();
                dialog.Commands.Add(command);
            }

            if (!string.IsNullOrEmpty(secondaryOptionText))
            {
                secondaryOptionText = (string)Application.Current.Resources[secondaryOptionText];
                var command = new UICommand(secondaryOptionText);
                if (secondaryOptionAction != null) command.Invoked = uiCommand => secondaryOptionAction();
                dialog.Commands.Add(command);
            }

            await dialog.ShowAsync();
        }
    }
}