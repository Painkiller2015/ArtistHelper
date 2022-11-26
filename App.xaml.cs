using ArtistHelper.ButtonControls;
using System.Windows;


namespace ArtistHelper
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        GlobalHotKeyManager _GHKManager = new();
        public App()
        {
            _GHKManager.CreateControlPanelEvent += async (obj, needCreate) =>
            {

            };
        }
    }
}
