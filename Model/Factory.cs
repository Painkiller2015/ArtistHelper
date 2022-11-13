using ArtistHelper.ButtonControls;
using ArtistHelper.Service;
using ArtistHelper.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace ArtistHelper.Model
{
    public class Factory
    {
        public Factory()
        {
        }
        public void CreateControlPanelWindow()
        {
            if (!IsCreate<Window>("ControlPanel"))
            {
                CreateControlPanel();
            }
        }
        public void CreateImageCreatorWindow()
        {
            if (!IsCreate<Window>("ImageCreator"))
            {
                CreateImageCreator();
            }
        }
        public void CreateCanvasLayerWindow()
        {
            if (!IsCreate<Window>("ScreenCanvas"))
            {
                CreateCanvasLayer();
            }
        }
        private bool IsCreate<Window>(string objName)
        {
            return string.IsNullOrEmpty(objName)
           ? Application.Current.Windows.OfType<Window>().Any()
           : Application.Current.Windows.OfType<Window>().Any(w => w.GetType().Name == objName);
        }
        private void CreateControlPanel()
        {
            ControlPanel cp = new();
            cp.Show();
        }
        private void CreateImageCreator()
        {
            ImageCreator ic = new();
            ic.Show();
        }
        private void CreateCanvasLayer()
        {
            ScreenCanvas sv = new()
            {
                Height = SysConfig.HeightScreens,
                Width = SysConfig.WidthScreens
            };
            sv.Show();
            sv.StartArtProccess();
            
            
        }

    }
}
