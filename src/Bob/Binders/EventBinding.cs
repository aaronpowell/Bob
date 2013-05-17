using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;

namespace Bob.Binders
{
    public abstract class EventBinding : FrameworkElement
    {
        private Delegate _eventHandler;
        private UIElement _element;

        public string EventName { get; set; }

        public void BindToEvent(UIElement element)
        {
            if (_eventHandler != null)
                throw new InvalidOperationException("This EventBinding is already bound");

            if (EventName == null)
                return;
                //throw new InvalidOperationException("The EventName is not set for an event binding");

            var elementType = element.GetType();
            EventInfo eventInfo = GetEventInfo(elementType, EventName);
            if (eventInfo == null)
            {
                Debug.WriteLine(string.Format("Unable to bind event {0} on element of type {1} because that event cannot be found.", EventName, elementType.FullName));

                return;
            }

            _eventHandler = GetType().GetTypeInfo().GetDeclaredMethod("OnEventRaised").CreateDelegate(eventInfo.EventHandlerType, this);

            if (eventInfo.DeclaringType.GetTypeInfo().Attributes.HasFlag(TypeAttributes.WindowsRuntime))
            {
                //Do a bunch of reflection crap to imitate what the compiler does when it 
                //hooks up to WinRT object events
                var addMethodType = typeof(Func<,>).MakeGenericType(eventInfo.EventHandlerType, typeof(EventRegistrationToken));
                var addMethod = eventInfo.AddMethod.CreateDelegate(addMethodType, element);

                var removeMethod = (Action<EventRegistrationToken>)eventInfo.RemoveMethod.CreateDelegate(typeof(Action<EventRegistrationToken>), element);

                var genericAddEventHandlerMethod = typeof(EventBinding).GetRuntimeMethods().Single(m => m.Name == "AddEventHandler");
                var addEventHandlerMethod = genericAddEventHandlerMethod.MakeGenericMethod(eventInfo.EventHandlerType);

                addEventHandlerMethod.Invoke(null, new object[] { addMethod, removeMethod, _eventHandler });
            }
            else
            {
                eventInfo.AddEventHandler(element, _eventHandler);
            }

            _element = element;
        }

        /// <summary>
        /// This method acts as a proxy to the AddEventHandler method so that I can invoke this method
        /// using reflection, rather than invoking AddEventHandler directly with reflection (which is not allowed)
        /// </summary>
        internal static void AddEventHandler<T>(Func<T, EventRegistrationToken> addMethod, Action<EventRegistrationToken> removeMethod, T handler)
        {
            WindowsRuntimeMarshal.AddEventHandler(addMethod, removeMethod, handler);
        }

        public void UnbindFromEvent()
        {
            if (_eventHandler == null)
                throw new InvalidOperationException("This EventBinding is not bound");

            EventInfo eventInfo = GetEventInfo(_element.GetType(), EventName);

            if (eventInfo.DeclaringType.GetTypeInfo().Attributes.HasFlag(TypeAttributes.WindowsRuntime))
            {
                var removeMethod = (Action<EventRegistrationToken>)eventInfo.RemoveMethod.CreateDelegate(typeof(Action<EventRegistrationToken>), _element);
                WindowsRuntimeMarshal.RemoveEventHandler(removeMethod, _eventHandler);
            }
            else
            {
                eventInfo.RemoveEventHandler(_element, _eventHandler);
            }

            _eventHandler = null;
            _element = null;
        }

        private EventInfo GetEventInfo(Type type, string eventName)
        {
            EventInfo eventInfo = type.GetTypeInfo().GetDeclaredEvent(eventName);
            if (eventInfo == null)
            {
                Type baseType = type.GetTypeInfo().BaseType;
                if (baseType != null)
                    return GetEventInfo(type.GetTypeInfo().BaseType, eventName);
                
                return null;
            }
            return eventInfo;
        }

        protected abstract void OnEventRaised(object sender, object e);
    }
}
