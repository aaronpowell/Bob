using Okra;
using Windows.UI.Xaml;

namespace Bob.UI
{
    sealed partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var bootstrapper = new AppBootstrapper();
            bootstrapper.Initialize();
        }
    }

    class AppBootstrapper : OkraBootstrapper
    {
        
    }
}
