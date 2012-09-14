using System;
using System.ComponentModel;
using Okra.Navigation;

namespace Bob.UI.ViewModels
{
    [ViewModelExport(typeof(DayMonthYearSelectorDemoViewModel))]
    public class DayMonthYearSelectorDemoViewModel : INotifyPropertyChanged
    {
        public DayMonthYearSelectorDemoViewModel()
        {
            MinDate = new DateTime(1988, 6, 12);
            MaxDate = new DateTime(2020, 5, 25);
            SortYearsAscending = true;
        }

        public DateTime? Date { get; set; }

        public DateTime MinDate { get; set; }

        public DateTime MaxDate { get; set; }

        public bool SortYearsAscending { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}