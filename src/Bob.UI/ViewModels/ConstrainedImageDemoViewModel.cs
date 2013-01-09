using System.ComponentModel;
using System.Composition;
using Okra.Core;
using Okra.Navigation;
using PropertyChanged;

namespace Bob.UI.ViewModels
{
    [ViewModelExport(typeof(ConstrainedImageDemoViewModel))]
    public class ConstrainedImageDemoViewModel : INotifyPropertyChanged
    {
        private readonly INavigationManager _navigationManager;

        [ImportingConstructor]
        public ConstrainedImageDemoViewModel(INavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
            GoBackCommand = new DelegateCommand(navigationManager.GoBack);
        }

        [DoNotNotify]
        public DelegateCommand GoBackCommand { get; set; }

        public bool CanGoBack { get { return _navigationManager.CanGoBack; } }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}