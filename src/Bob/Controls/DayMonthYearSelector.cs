using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Bob.Controls
{
    [TemplatePart(Name = "DayComboBox", Type = typeof(ComboBox))]
    [TemplatePart(Name = "MonthComboBox", Type = typeof(ComboBox))]
    [TemplatePart(Name = "YearComboBox", Type = typeof(ComboBox))]
    public class DayMonthYearSelector : Control
    {
        private const string NullYearLabel = "Year";
        private const string NullMonthLabel = "Month";
        private const string NullDayLabel = "Day";
        private ComboBox _dayComboBox;
        private ComboBox _monthComboBox;
        private ComboBox _yearComboBox;
        private bool _preventReentrancy;


        public static readonly DependencyProperty SelectedDateProperty =
            DependencyProperty.Register("SelectedDate", typeof(DateTime?), typeof(DayMonthYearSelector), new PropertyMetadata(default(DateTime?), (o, args) => ((DayMonthYearSelector)o).OnSelectedDateChanged(args)));

        public DateTime? SelectedDate
        {
            get { return (DateTime?)GetValue(SelectedDateProperty); }
            set { SetValue(SelectedDateProperty, value); }
        }

        public static readonly DependencyProperty ComboBoxStyleProperty =
            DependencyProperty.Register("ComboBoxStyle", typeof(Style), typeof(DayMonthYearSelector), new PropertyMetadata(default(Style), (o, args) => ((DayMonthYearSelector)o).OnComboBoxStyleChanged(args)));

        public Style ComboBoxStyle
        {
            get { return (Style)GetValue(ComboBoxStyleProperty); }
            set { SetValue(ComboBoxStyleProperty, value); }
        }

        public static readonly DependencyProperty MinDateProperty =
            DependencyProperty.Register("MinDate", typeof(DateTime), typeof(DayMonthYearSelector), new PropertyMetadata(DateTime.MinValue, (o, args) => ((DayMonthYearSelector)o).OnMinDateChanged(args)));

        public DateTime MinDate
        {
            get { return (DateTime)GetValue(MinDateProperty); }
            set { SetValue(MinDateProperty, value); }
        }

        public static readonly DependencyProperty MaxDateProperty =
            DependencyProperty.Register("MaxDate", typeof(DateTime), typeof(DayMonthYearSelector), new PropertyMetadata(DateTime.MaxValue, (o, args) => ((DayMonthYearSelector)o).OnMaxDateChanged(args)));

        public DateTime MaxDate
        {
            get { return (DateTime)GetValue(MaxDateProperty); }
            set { SetValue(MaxDateProperty, value); }
        }

        public static readonly DependencyProperty YearsSortAscendingProperty =
            DependencyProperty.Register("SortYearsAscending", typeof(bool), typeof(DayMonthYearSelector), new PropertyMetadata(true, (o, args) => ((DayMonthYearSelector)o).OnYearsSortOrderChanged(args)));

        public bool SortYearsAscending
        {
            get { return (bool)GetValue(YearsSortAscendingProperty); }
            set { SetValue(YearsSortAscendingProperty, value); }
        }

        public DayMonthYearSelector()
        {
            DefaultStyleKey = typeof(DayMonthYearSelector);
        }

        protected override async void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _dayComboBox = (ComboBox)GetTemplateChild("DayComboBox");
            _monthComboBox = (ComboBox)GetTemplateChild("MonthComboBox");
            _yearComboBox = (ComboBox)GetTemplateChild("YearComboBox");
            _dayComboBox.SelectionChanged += OnDaySelectionChanged;
            _monthComboBox.SelectionChanged += OnMonthSelectionChanged;
            _yearComboBox.SelectionChanged += OnYearSelectionChanged;
            ApplyComboBoxStyle(ComboBoxStyle);

            if (_preventReentrancy)
                return;

            _preventReentrancy = true;
            try
            {
                await PopulateDropdowns(SelectedDate, MinDate, MaxDate);
            }
            finally
            {
                _preventReentrancy = false;
            }
        }

        private void ApplyComboBoxStyle(Style comboBoxStyle)
        {
            if (comboBoxStyle != null)
            {
                if (_dayComboBox != null)
                    _dayComboBox.Style = comboBoxStyle;

                if (_monthComboBox != null)
                    _monthComboBox.Style = comboBoxStyle;

                if (_yearComboBox != null)
                    _yearComboBox.Style = comboBoxStyle;
            }
        }

        private async Task PopulateDropdowns(DateTime? dateTime, DateTime minDate, DateTime maxDate)
        {
            if (_yearComboBox == null || _monthComboBox == null || _dayComboBox == null)
                return;

            var years = GenerateYears(minDate, MaxDate);
            var months = dateTime == null ? GenerateAllMonths() : GenerateMonths(dateTime.Value.Year, minDate, maxDate);
            var days = dateTime == null ? GenerateAllDays() : GenerateDays(dateTime.Value.Month, dateTime.Value.Year, minDate, maxDate);

            var selectedYear = YearToDropdownString(dateTime != null ? dateTime.Value.Year : (int?)null);
            var selectedMonth = MonthToDropdownString(dateTime != null ? dateTime.Value.Month : (int?)null);
            var selectedDay = DayToDropdownString(dateTime != null ? dateTime.Value.Day : (int?)null);

            await UpdateComboBoxesItemsSourceAndSelectedItem(
                new ComboBoxUpdate(_yearComboBox, years, selectedYear),
                new ComboBoxUpdate(_monthComboBox, months, selectedMonth),
                new ComboBoxUpdate(_dayComboBox, days, selectedDay));
        }

        /// <summary>
        /// This method updates a number of <see cref="ComboBox"/>'s <see cref="ItemsControl.ItemsSource"/> and
        /// <see cref="Selector.SelectedItem"/> in a way that actually works! (Amazing!)
        /// </summary>
        /// <remarks>
        /// Attempting to set a ComboBox's SelectedItem after settings its ItemsSource does not work
        /// (the SelectedItem assignment is ignored). Yielding the SelectedItem assignment to later on
        /// the dispatcher seems to resolve the problem. My guess is that the problem is caused by
        /// the ComboBox generating its items asynchronously to the ItemsSource assignment.
        /// </remarks>
        private async Task UpdateComboBoxesItemsSourceAndSelectedItem(params ComboBoxUpdate[] comboBoxUpdates)
        {
            foreach (var update in comboBoxUpdates)
                update.ComboBox.ItemsSource = update.ItemsSource;

            await Task.Yield();

            foreach (var update in comboBoxUpdates)
                update.ComboBox.SelectedItem = update.SelectedItem;
        }

        private IList<string> GenerateYears(DateTime minDate, DateTime maxDate)
        {
            var years = Enumerable.Range(minDate.Year, maxDate.Year - minDate.Year + 1);
            if (SortYearsAscending == false)
                years = years.OrderByDescending(y => y);

            return new[] { NullYearLabel }.Concat(years.Select(y => YearToDropdownString(y))).ToList();
        }

        private static string YearToDropdownString(int? year)
        {
            return year == null ? NullYearLabel : year.Value.ToString("0000");
        }

        private IList<string> GenerateMonths(int year, DateTime minDate, DateTime maxDate)
        {
            int minMonth = year == minDate.Year ? minDate.Month : 1;
            int maxMonth = year == maxDate.Year ? maxDate.Month : 12;

            return new[] { NullMonthLabel }.Concat(Enumerable.Range(minMonth, maxMonth - minMonth + 1).Select(m => MonthToDropdownString(m))).ToList();
        }

        private IList<string> GenerateAllMonths()
        {
            return new[] { NullMonthLabel }.Concat(Enumerable.Range(1, 12).Select(m => MonthToDropdownString(m))).ToList();
        }

        private static string MonthToDropdownString(int? month)
        {
            return month == null ? NullMonthLabel : new DateTime(2012, month.Value, 1).ToString("MMMM");
        }

        private IList<string> GenerateAllDays()
        {
            return new[] { NullDayLabel }.Concat(Enumerable.Range(1, 31).Select(d => DayToDropdownString(d))).ToList();
        }

        private IList<string> GenerateDays(int month, int year, DateTime minDate, DateTime maxDate)
        {
            int minDay, maxDay;
            if (minDate.Month == month && minDate.Year == year)
                minDay = minDate.Day;
            else
                minDay = 1;

            if (maxDate.Month == month && maxDate.Year == year)
                maxDay = maxDate.Day;
            else
                maxDay = DateTime.DaysInMonth(year, month);

            return new[] { NullDayLabel }.Concat(Enumerable.Range(minDay, maxDay - minDay + 1).Select(d => DayToDropdownString(d))).ToList();
        }

        private static string DayToDropdownString(int? day)
        {
            return day == null ? NullDayLabel : day.Value.ToString("00");
        }

        private void OnDaySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_preventReentrancy)
                return;

            _preventReentrancy = true;
            try
            {
                if (_dayComboBox.SelectedItem == null)
                    _dayComboBox.SelectedItem = NullDayLabel;

                SetSelectedDateFromDropdowns();
            }
            finally
            {
                _preventReentrancy = false;
            }
        }

        private void SetSelectedDateFromDropdowns()
        {
            int? day = ParseDayFromDropdown();
            int? month = ParseMonthFromDropdown();
            int? year = ParseYearFromDropdown();

            if (day == null || month == null || year == null)
                SelectedDate = null;
            else
            {
                SelectedDate = new DateTime(year.Value, month.Value, day.Value, 0, 0, 0, DateTimeKind.Unspecified);
            }
        }

        private int? ParseDayFromDropdown()
        {
            var selectedItem = (string)_dayComboBox.SelectedItem;
            return selectedItem == NullDayLabel ? (int?)null : int.Parse(selectedItem);
        }

        private int? ParseMonthFromDropdown()
        {
            var selectedItem = (string)_monthComboBox.SelectedItem;
            return selectedItem == NullMonthLabel ? (int?)null : DateTime.ParseExact(selectedItem, "MMMM", CultureInfo.CurrentUICulture).Month;
        }

        private int? ParseYearFromDropdown()
        {
            var selectedItem = (string)_yearComboBox.SelectedItem;
            return selectedItem == NullYearLabel ? (int?)null : int.Parse((string)_yearComboBox.SelectedItem);
        }

        private async void OnMonthSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_preventReentrancy)
                return;

            _preventReentrancy = true;
            try
            {
                if (_monthComboBox.SelectedItem == null)
                    _monthComboBox.SelectedItem = NullMonthLabel;

                await SetDayDropdownWithRespectToTheYearAndMonth();

                SetSelectedDateFromDropdowns();
            }
            finally
            {
                _preventReentrancy = false;
            }
        }

        private async Task SetDayDropdownWithRespectToTheYearAndMonth()
        {
            int? day = ParseDayFromDropdown();
            int? month = ParseMonthFromDropdown();
            int? year = ParseYearFromDropdown();

            IList<string> daysList;
            if (month != null)
            {
                if (year == null)
                    year = 2012; //Use a substitute leap-year year as an estimate. When the year is actually set, the days list will be rebuilt anyway

                int daysInMonth = DateTime.DaysInMonth(year.Value, month.Value);

                if (year == MinDate.Year && month == MinDate.Month && day < MinDate.Day)
                    day = MinDate.Day;
                else if (year == MaxDate.Year && month == MaxDate.Month && day > MaxDate.Day)
                    day = MaxDate.Day;
                else if (day > daysInMonth)
                    day = daysInMonth;

                daysList = GenerateDays(month.Value, year.Value, MinDate, MaxDate);
            }
            else
                daysList = GenerateAllDays();

            await UpdateComboBoxesItemsSourceAndSelectedItem(
                new ComboBoxUpdate(_dayComboBox, daysList, DayToDropdownString(day)));
        }

        private async void OnYearSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_preventReentrancy)
                return;

            _preventReentrancy = true;
            try
            {
                if (_yearComboBox.SelectedItem == null)
                    _yearComboBox.SelectedItem = NullYearLabel;

                await SetMonthDropdownWithRespectToTheYear();
                await SetDayDropdownWithRespectToTheYearAndMonth();

                SetSelectedDateFromDropdowns();
            }
            finally
            {
                _preventReentrancy = false;
            }
        }

        private async Task SetMonthDropdownWithRespectToTheYear()
        {
            int? month = ParseMonthFromDropdown();
            int? year = ParseYearFromDropdown();

            if (year != null && month != null)
            {
                if (year == MinDate.Year && month < MinDate.Month)
                    month = MinDate.Month;
                else if (year == MaxDate.Year && month > MaxDate.Month)
                    month = MaxDate.Month;
            }

            var monthsList = year != null ? GenerateMonths(year.Value, MinDate, MaxDate) : GenerateAllMonths();
            await UpdateComboBoxesItemsSourceAndSelectedItem(
                new ComboBoxUpdate(_monthComboBox, monthsList, MonthToDropdownString(month)));
        }

        private async void OnSelectedDateChanged(DependencyPropertyChangedEventArgs args)
        {
            if (_preventReentrancy || args.OldValue == args.NewValue)
                return;

            var newSelectedDate = (DateTime?)args.NewValue;

            _preventReentrancy = true;
            try
            {
                var roundedSelectedDate = RoundDateToMinMax(newSelectedDate, MinDate, MaxDate);
                if (roundedSelectedDate == newSelectedDate)
                {
                    await PopulateDropdowns(SelectedDate, MinDate, MaxDate);
                }
                else
                {
                    //Change the property later on the dispatcher, because changing it now
                    //results in the new value not getting pushed back to bindings attached
                    //to this properly
#pragma warning disable 4014
                    Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        var capturedOriginalSelectedDate = newSelectedDate;

                        //Only update the date if it hasn't changed to something else since
                        //this task was submitted on the dispatcher
                        if (SelectedDate == capturedOriginalSelectedDate)
                            SelectedDate = roundedSelectedDate;
                    });
#pragma warning restore 4014
                }
            }
            finally
            {
                _preventReentrancy = false;
            }
        }

        private DateTime? RoundDateToMinMax(DateTime? dateTime, DateTime minDate, DateTime maxDate)
        {
            if (dateTime == null)
                return null;

            dateTime = new[] { dateTime.Value, maxDate }.Min();
            dateTime = new[] { dateTime.Value, minDate }.Max();

            return dateTime;
        }

        private async void OnMinDateChanged(DependencyPropertyChangedEventArgs args)
        {
            if (_preventReentrancy || args.OldValue == args.NewValue)
                return;

            var newMinDate = (DateTime)args.NewValue;

            _preventReentrancy = true;
            try
            {
                var roundedSelectedDate = RoundDateToMinMax(SelectedDate, newMinDate, MaxDate);
                SelectedDate = roundedSelectedDate;
                await PopulateDropdowns(roundedSelectedDate, newMinDate, MaxDate);
            }
            finally
            {
                _preventReentrancy = false;
            }
        }

        private async void OnMaxDateChanged(DependencyPropertyChangedEventArgs args)
        {
            if (_preventReentrancy || args.OldValue == args.NewValue)
                return;

            var newMaxDate = (DateTime)args.NewValue;

            _preventReentrancy = true;
            try
            {
                var roundedSelectedDate = RoundDateToMinMax(SelectedDate, MinDate, newMaxDate);
                SelectedDate = roundedSelectedDate;
                await PopulateDropdowns(roundedSelectedDate, MinDate, newMaxDate);
            }
            finally
            {
                _preventReentrancy = false;
            }
        }

        private void OnComboBoxStyleChanged(DependencyPropertyChangedEventArgs args)
        {
            if (args.NewValue == args.OldValue)
                return;

            ApplyComboBoxStyle((Style)args.NewValue);
        }

        private async void OnYearsSortOrderChanged(DependencyPropertyChangedEventArgs args)
        {
            if (_preventReentrancy || args.OldValue == args.NewValue || _yearComboBox == null)
                return;

            _preventReentrancy = true;
            try
            {
                var years = GenerateYears(MinDate, MaxDate);
                await UpdateComboBoxesItemsSourceAndSelectedItem(
                    new ComboBoxUpdate(_yearComboBox, years, _yearComboBox.SelectedItem));
            }
            finally
            {
                _preventReentrancy = false;
            }
        }

        private struct ComboBoxUpdate
        {
            public readonly ComboBox ComboBox;
            public readonly object ItemsSource;
            public readonly object SelectedItem;

            public ComboBoxUpdate(ComboBox comboBox, object itemsSource, object selectedItem)
            {
                ComboBox = comboBox;
                ItemsSource = itemsSource;
                SelectedItem = selectedItem;
            }
        }
    }
}