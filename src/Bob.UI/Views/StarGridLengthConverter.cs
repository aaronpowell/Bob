using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Bob.UI.Views
{
    public class StarGridLengthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return null;

            var gridLength = (GridLength)value;
            return gridLength.Value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return null;

            var integer = (double)value;
            return new GridLength(integer, GridUnitType.Star);
        }
    }
}