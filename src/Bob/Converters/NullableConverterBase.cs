using System;
using Windows.UI.Xaml.Data;

namespace Bob.Converters
{
    public abstract class NullableConverterBase<T> : IValueConverter
        where T : struct 
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return (value == null || string.IsNullOrEmpty(value.ToString())) ? null : System.Convert.ChangeType(value, typeof (T));
        }
    }
}
