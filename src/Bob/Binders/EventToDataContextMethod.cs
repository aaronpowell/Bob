using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Bob.Utilities;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Bob.Binders
{
    public class EventToDataContextMethod : EventBinding
    {
        public string MethodName { get; set; }

        public string ExtraArgumentPropertyName { get; set; }

        public bool Bubble { get; set; }

        protected override void OnEventRaised(object sender, object e)
        {
            var frameworkElement = ((FrameworkElement)sender);
            var dataContext = frameworkElement.DataContext;

            object extraPropertyValue;
            TypeInfo extraPropertyType;
            GetExtraPropertyInfo(frameworkElement, out extraPropertyValue, out extraPropertyType);

            MethodInfo methodInfo;
            bool foundMethod;
            if (Bubble)
                foundMethod = TryFindDataContextMethodInfoByBubbling(frameworkElement, e, extraPropertyType, out dataContext, out methodInfo);
            else
                foundMethod = TryGetDataContextMethodInfo(frameworkElement, e, extraPropertyType, out methodInfo);

            if (foundMethod == false) 
                return;
            
            int paramCount = methodInfo.GetParameters().Count();
            if (paramCount == 2)
            {
                methodInfo.InvokeOrThrows(dataContext, new[] { e, extraPropertyValue });
            }
            else if (paramCount == 1)
            {
                methodInfo.InvokeOrThrows(dataContext, new[] { e });
            }
            else
            {
                methodInfo.InvokeOrThrows(dataContext, new object[0]);
            }
        }

        private void GetExtraPropertyInfo(FrameworkElement frameworkElement, out object extraPropertyValue, out TypeInfo extraPropertyType)
        {
            extraPropertyValue = null;
            extraPropertyType = null;

            if (ExtraArgumentPropertyName != null)
            {
                var extraPropertyInfo = frameworkElement.GetType().GetRuntimeProperty(ExtraArgumentPropertyName);
                if (extraPropertyInfo == null)
                    Debug.WriteLine(string.Format("Cannot find property {0} on UI element {1} when handling event {2}", ExtraArgumentPropertyName, frameworkElement.GetType().FullName, EventName));
                else
                {
                    extraPropertyValue = extraPropertyInfo.GetValue(frameworkElement);
                    if (extraPropertyValue != null)
                        extraPropertyType = extraPropertyValue.GetType().GetTypeInfo();
                }
            }
        }

        private bool TryFindDataContextMethodInfoByBubbling(FrameworkElement element, object eventArgs, TypeInfo extraPropertyType, out object dataContext, out MethodInfo methodInfo)
        {
            var success = TryGetDataContextMethodInfo(element, eventArgs, extraPropertyType, out methodInfo);
            if (success == false)
            {
                var parentElement = GetNextParentFrameworkElement(element);
                if (parentElement == null)
                {
                    dataContext = null;
                    methodInfo = null;
                    return false;
                }

                return TryFindDataContextMethodInfoByBubbling(parentElement, eventArgs, extraPropertyType, out dataContext, out methodInfo);
            }

            dataContext = element.DataContext;
            return true;
        }

        private FrameworkElement GetNextParentFrameworkElement(DependencyObject dependencyObject)
        {
            var parent = VisualTreeHelper.GetParent(dependencyObject);
            while (parent != null && parent is FrameworkElement == false)
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            return (FrameworkElement)parent;
        }

        private bool TryGetDataContextMethodInfo(FrameworkElement element, object eventArgs, TypeInfo extraPropertyType, out MethodInfo methodInfo)
        {
            var dataContext = element.DataContext;
            if (dataContext == null)
            {
                methodInfo = null;
                return false;
            }

            Type dataContextType = dataContext.GetType();
            IEnumerable<MethodInfo> methods = dataContextType.GetRuntimeMethods();
            
            methodInfo = methods.SingleOrDefault(m => m.Name == MethodName && HasTwoParametersNeeded(m.GetParameters(), eventArgs.GetType().GetTypeInfo(), extraPropertyType));
            if (methodInfo == null)
            {
                methodInfo = methods.SingleOrDefault(m => m.Name == MethodName && HasOneParameterNeeded(m.GetParameters(), eventArgs.GetType().GetTypeInfo()));
                if (methodInfo == null)
                {
                    methodInfo = dataContextType.GetRuntimeMethod(MethodName, new Type[0]);
                    if (methodInfo == null)
                    {
                        Debug.WriteLine(string.Format("Cannot locate the method {0} on the DataContext {1} when handling event {2}", MethodName, dataContextType.FullName, EventName));
                        return false;
                    }
                }
            }

            return true;
        }

        private bool HasOneParameterNeeded(ParameterInfo[] parameters, TypeInfo eventArgsType)
        {
            return parameters.Any() &&
                   parameters.ElementAt(0).ParameterType.GetTypeInfo().IsAssignableFrom(eventArgsType);
        }

        private bool HasTwoParametersNeeded(ParameterInfo[] parameters, TypeInfo eventArgsType, TypeInfo extraPropertyType)
        {
            return parameters.Count() == 2 &&
                   HasOneParameterNeeded(parameters, eventArgsType) &&
                   (
                      extraPropertyType != null &&
                      parameters.ElementAt(1).ParameterType.GetTypeInfo().IsAssignableFrom(extraPropertyType) ||
                      parameters.ElementAt(1).ParameterType.GetTypeInfo().IsValueType == false
                   );
        }
    }
}