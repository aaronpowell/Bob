using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace Bob.Utilities
{
    public static class TaskExceptionExt
    {
        public static TaskScheduler Scheduler { get; set; }

        public static Task ThrowToDispatcherIfFails(this Task task)
        {
            if (Scheduler == null)
                throw new InvalidOperationException("You should specify the scheduler at least once in the application before using this class");

            task.ContinueWith(t =>
            {
                Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        throw t.Exception.Flatten();
                    });
            }, 
            CancellationToken.None, 
            TaskContinuationOptions.OnlyOnFaulted, 
            Scheduler);

            return task;
        }

        public static void InvokeOrThrows(this MethodInfo methodInfo, object context, params object[] args)
        {
            var result = methodInfo.Invoke(context, args) as Task;
            
            if (result == null) return;

            result.ThrowToDispatcherIfFails();
        }
    }
}