using Bob.UI.Common;
using Okra.Core;
using Okra.Navigation;
using System.Composition;

namespace Bob.UI.ViewModels
{
    [ViewModelExport(SpecialPageNames.Home)]
    public class HomeViewModel
    {
        [ImportingConstructor]
        public HomeViewModel(INavigationManager navigationManager)
        {
            NumericTextBoxCommand = new DelegateCommand(() => navigationManager.NavigateTo(PageNames.NumericTextBox));
        }

        public DelegateCommand NumericTextBoxCommand { get; set; }
    }
}
