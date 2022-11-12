using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;


namespace ArtistHelper.Service
{
    public static class SysConfig
    {        
        //private static Workspase _workspase = new();
        public static int GetWidthScreens() => (int)SystemParameters.VirtualScreenWidth;
        public static int GetHeightScreens() => (int)SystemParameters.VirtualScreenHeight;
        //private class Workspase
        //{
        //    public int Height { get; private set; } = default;
        //    public int Width { get; private set; } = default;

        //    public Workspase()
        //    {
        //        if (Screen.AllScreens.Length > 1)
        //        {
        //            foreach (var screen in Screen.AllScreens)
        //            {
        //                if (Height < screen.Bounds.Height)
        //                    Height = screen.Bounds.Height;
        //                Width += screen.Bounds.Width;
        //            }
        //        }
        //        else
        //        {
        //            Height = Screen.PrimaryScreen.Bounds.Height;
        //            Width = Screen.PrimaryScreen.Bounds.Width;
        //        }
        //    }
        //}
    }
   
}
