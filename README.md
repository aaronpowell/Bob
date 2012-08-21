Bob
===

Bob wasn't a great product for Microsoft but maybe Bob can add what's missing for Windows 8. Bob aims to bring a bunch of stuff that Windows 8 XAML doesn't do natively until such time as it is actually part of the framework.

Features:

* Controls
	* `NumericTextBox` for when you want to allow only numbers (ints or floats). It'll also show the appropriate soft keyboard
	* `DateSelector` (coming soon)
* Converters
	* `NullableDateTimeConverter`, `NullableFloatConverter`, `NullableIntConverter` because you can't natively bind types of [`Nullable<T>`](http://msdn.microsoft.com/en-us/library/b3h38hb0.aspx). There's also a `NullableConverterBase` so you can make your own
* Binders
	* Enum to Visual State via the `VisualStateBinder`. Very useful if you want to bind a series of Visual States based on a property of your ViewModel
		* This is extensible, you can implement your own `VisualStateBinding` so that you can do your own custom Visual State bindings
	* Event binding. You can either bind an event to an `ICommand` with the `EventToCommand` binding or an event to a methond on your DataContext via `EventToDataContextMethod`
		* This is extensible, you can implement your own `EventBinding` if you want to bind to something else

License
===

MIT