using System.Text.RegularExpressions;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Bob.Controls
{
    public class NumericTextBox : TextBox
    {
        private static readonly Regex NumericalRegex = new Regex(@"^\d*$");
        private static readonly Regex DecimalRegex = new Regex(@"(?n:(^(?!0,?\d)\d{1,3}(?=(?<1>,)|(?<1>))(\k<1>\d{3})*(\.\d\d)?)$)");

        public bool AllowDecimalPlaces { get; set; }
        
        public NumericTextBox()
        {
            KeyDown += KeyDownHandler;
            TextChanged +=TextChangedHandler;

            var scope = new InputScope();
            var name = new InputScopeName
                           {
                               NameValue = InputScopeNameValue.Number
                           };

            scope.Names.Add(name);

            InputScope = scope;
        }

        private void TextChangedHandler(object sender, TextChangedEventArgs e)
        {
            var regex = AllowDecimalPlaces ? DecimalRegex : NumericalRegex;

            if (!string.IsNullOrEmpty(Text) && !regex.IsMatch(Text))
                Text = Regex.Replace(Text, !AllowDecimalPlaces ? "[^0-9]" : @"[^0-9\.]", string.Empty);
        }

        void KeyDownHandler(object sender, KeyRoutedEventArgs e)
        {
            if (IsModifier(e.Key))
                return;
            e.Handled = !IsNumeric(e.Key);
        }

        private static bool IsModifier(VirtualKey key)
        {
            switch (key)
            {
                case VirtualKey.Shift:
                case VirtualKey.Control:
                case VirtualKey.Tab:
                case VirtualKey.Enter:
                    return true;
            }

            return false;
        }

        private bool IsNumeric(VirtualKey key, bool checkModifiers = true)
        {
            if (checkModifiers && Window.Current.CoreWindow.GetKeyState(VirtualKey.Shift).HasFlag(CoreVirtualKeyStates.Down))
                return false;

            switch (key)
            {
                case VirtualKey.NumberPad0:
                case VirtualKey.NumberPad1:
                case VirtualKey.NumberPad2:
                case VirtualKey.NumberPad3:
                case VirtualKey.NumberPad4:
                case VirtualKey.NumberPad5:
                case VirtualKey.NumberPad6:
                case VirtualKey.NumberPad7:
                case VirtualKey.NumberPad8:
                case VirtualKey.NumberPad9:
                case VirtualKey.Number0:
                case VirtualKey.Number1:
                case VirtualKey.Number2:
                case VirtualKey.Number3:
                case VirtualKey.Number4:
                case VirtualKey.Number5:
                case VirtualKey.Number6:
                case VirtualKey.Number7:
                case VirtualKey.Number8:
                case VirtualKey.Number9:
                    return true;
            }

            return AllowDecimalPlaces && ((int)key == 190 || key == VirtualKey.Decimal);
        }
    }
}
