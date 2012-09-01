using System;
using System.Threading.Tasks;

namespace Bob.Services
{
    public interface INotificationService
    {
        Task Notify(string message);
        Task Notify(string message, string title);
        Task Notify(string message, string title, string primaryOptionText, Action primaryOptionAction = null);
        Task Notify(string message, string title, string primaryOptionText, string secondaryOptionText, Action primaryOptionAction = null, Action secondaryOptionAction = null);
    }
}
