using Bob.UI.ViewModels;
using Okra.Navigation;

namespace Bob.UI.Views
{
    [PageExport(typeof(DayMonthYearSelectorDemoViewModel))]
    public sealed partial class DayMonthYearSelectorDemoView
    {
        public DayMonthYearSelectorDemoView()
        {
            InitializeComponent();
        }
    }
}
