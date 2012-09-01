using System;
using System.Linq;
using System.Text.RegularExpressions;
using Bob.Utilities;
using Windows.System;
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

            var scope = new InputScope();
            var name = new InputScopeName
            {
                NameValue = InputScopeNameValue.Number
            };

            scope.Names.Add(name);

            InputScope = scope;

            LostFocus += LostFocusHandler;
        }

        void LostFocusHandler(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Text))
                return;

            var regex = AllowDecimalPlaces ? DecimalRegex : NumericalRegex;

            if (!regex.IsMatch(Text))
            {
                var sanatizedInput = Regex.Replace(Text, !AllowDecimalPlaces ? "[^0-9]" : @"[^0-9\.]", string.Empty);

                sanatizedInput = string.Join(".", sanatizedInput.Split('.').Take(2));

                var amount = Math.Truncate(decimal.Parse(sanatizedInput) * 100) / 100;

                Text = amount.ToString().PadLeft(sanatizedInput.Length, '0');
            }
            else
            {
                var amount = Math.Truncate(decimal.Parse(Text) * 100) / 100;

                Text = amount.ToString().PadLeft(Text.Length, '0');
            }
        }

        void KeyDownHandler(object sender, KeyRoutedEventArgs e)
        {
            if (KeyValidation.IsModifier(e.Key))
                return;
            e.Handled = !KeyValidation.IsNumeric(e.Key) && !(AllowDecimalPlaces && ((int)e.Key == 190 || e.Key == VirtualKey.Decimal));
        }
    }
}
