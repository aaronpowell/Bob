using System.ComponentModel;
using Bob.UI.Common;
using Okra.Core;
using Okra.Navigation;
using PropertyChanged;
using System.Composition;

namespace Bob.UI.ViewModels
{
    [ViewModelExport(PageNames.NumericTextBox)]
    public class NumericTextBoxViewModel : INotifyPropertyChanged
    {
        private readonly INavigationManager navigationManager;

        [ImportingConstructor]
        public NumericTextBoxViewModel(INavigationManager navigationManager)
        {
            this.navigationManager = navigationManager;
            GoBackCommand = new DelegateCommand(navigationManager.GoBack);
        }

        [DoNotNotify]
        public DelegateCommand GoBackCommand { get; set; }

        public bool CanGoBack { get { return navigationManager.CanGoBack; } }
        public int ValueAsInt { get; set; }
        public float ValueAsFloat { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
