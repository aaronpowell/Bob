Bob
===

Bob wasn't a great product for Microsoft but maybe Bob can add what's missing for Windows 8. Bob aims to bring a bunch of stuff that Windows 8 XAML doesn't do natively until such time as it is actually part of the framework.

Features:

* Controls
	* `NumericTextBox` for when you want to allow only numbers (ints or floats). It'll also show the appropriate soft keyboard
	* `DayMonthYearSelector` for when you want the user to enter a date. It allows you specify a minimum and maximum date, and automatically handles months of differing length, leap years and null dates. The control has dropdowns for day, month and year, and exposes a single nullable DateTime property to bind to (SelectedDate).
    * `ConstrainedImage` for when you want to display an image at its actual size, unless it is too big for the area in which it has been placed, in which case you'd rather it be shrunk to fit the space available.
* Converters
	* `NullableDateTimeConverter`, `NullableFloatConverter`, `NullableIntConverter` because you can't natively bind types of [`Nullable<T>`](https://msdn.microsoft.com/library/b3h38hb0.aspx?WT.mc_id=javascript-0000-aapowell). There's also a `NullableConverterBase` so you can make your own
* Binders
	* Enum to Visual State via the `VisualStateBinder`. Very useful if you want to bind a series of Visual States based on a property of your ViewModel
		* This is extensible, you can implement your own `VisualStateBinding` so that you can do your own custom Visual State bindings
	* Event binding. You can either bind an event to an `ICommand` with the `EventToCommand` binding or an event to a methond on your DataContext via `EventToDataContextMethod`
		* This is extensible, you can implement your own `EventBinding` if you want to bind to something else

Download
===

Bob can be downloaded via NuGet. Enter the following command in your package manager console:

    Install-Package Bob

License
===

MIT