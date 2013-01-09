using System;
using System.ComponentModel;
using System.Composition;
using Okra.Core;
using Okra.Navigation;
using PropertyChanged;

namespace Bob.UI.ViewModels
{
    [ViewModelExport(typeof(DayMonthYearSelectorDemoViewModel))]
    public class DayMonthYearSelectorDemoViewModel : INotifyPropertyChanged
    {
        private readonly INavigationManager _navigationManager;

        [ImportingConstructor]
        public DayMonthYearSelectorDemoViewModel(INavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
            MinDate = new DateTime(1988, 6, 12);
            MaxDate = new DateTime(2020, 5, 25);
            SortYearsAscending = true;
            GoBackCommand = new DelegateCommand(navigationManager.GoBack);
        }

        public DateTime? Date { get; set; }

        public DateTime MinDate { get; set; }

        public DateTime MaxDate { get; set; }

        public bool SortYearsAscending { get; set; }

        [DoNotNotify]
        public DelegateCommand GoBackCommand { get; set; }

        public bool CanGoBack { get { return _navigationManager.CanGoBack; } }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}