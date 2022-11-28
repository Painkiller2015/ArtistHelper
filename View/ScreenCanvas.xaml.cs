using ArtistHelper.ButtonControls;
using ArtistHelper.Model;
using ArtistHelper.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace ArtistHelper.View
{
    /// <summary>
    /// Логика взаимодействия для ScreenCanvas.xaml
    /// </summary>
    public partial class ScreenCanvas : Window
    {
        private InkCanvasEditingMode _inkCanvasEditingMode;
        private GlobalHotKeyManager _GHKManager = new();
        public ScreenCanvas()
        {
            InitializeComponent();

            this.Left = SystemParameters.VirtualScreenLeft;
            this.Top = SystemParameters.VirtualScreenTop;

            ControlPanel.ChangeColorEvent += async (obj, color) =>
            {
                Color newColor = ((SolidColorBrush)color).Color;
                CanvasLayer.DefaultDrawingAttributes.Color = newColor;
            };
            ControlPanel.ChangeBrushSizeEvent += async (obj, size) =>
            {
                CanvasLayer.DefaultDrawingAttributes.Width = size;
            };
            ControlPanel.ChangeEditorModeEvent += async (obj, delLine) =>
            {
                if (delLine)
                    _inkCanvasEditingMode = InkCanvasEditingMode.EraseByStroke;
                else
                    _inkCanvasEditingMode = InkCanvasEditingMode.EraseByPoint;
            };

            _GHKManager.CanvasActivEvent += async (obj, needCanvasActive) =>
            {
                if (needCanvasActive)
                    await StartArtProccess();
                if (!needCanvasActive)
                    await StopArtProccess();
            };
            _GHKManager.ClearCanvas += async (obj, clearCanvas) =>
            {
                CanvasLayer.Strokes.Clear();
            };
           
        }
        public async Task StartArtProccess()
        {
            StopFixImage();
            this.Focusable = true;
            this.Activate();
            SetColor("#0D808080");

            if (CanvasLayer.DefaultDrawingAttributes.Color == (Color)ColorConverter.ConvertFromString("#00FFFFFF"))
            {
                CanvasLayer.EditingMode = _inkCanvasEditingMode;
                return;
            }

            if (CanvasLayer.EditingMode != InkCanvasEditingMode.Ink)
                CanvasLayer.EditingMode = InkCanvasEditingMode.Ink;
        }
        public async Task StopArtProccess()
        {
            this.Focusable = false;
            CanvasLayer.EditingMode = InkCanvasEditingMode.None;
            SetColor("#00000000");
            StartFixImage();

            nint hwnd = new WindowInteropHelper(this).Handle;
            WinApi.HideFromAltTab(hwnd);
        }
        private void SetColor(string HEXcolor)
        {
            var bc = new BrushConverter();
            CanvasLayer.Background = (Brush)bc.ConvertFrom(HEXcolor);
        }
        //перенести в WinApi
        public static class WindowsServices
        {
            const int WS_EX_TRANSPARENT = 0x00000020;
            const int GWL_EXSTYLE = (-20);

            [DllImport("user32.dll")]
            static extern int GetWindowLong(IntPtr hwnd, int index);

            [DllImport("user32.dll")]
            static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

            public static void SetWindowExTransparent(IntPtr hwnd)
            {
                int extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
                SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_TRANSPARENT);
            }
            public static void ClearWindowExTransparent(IntPtr hwnd)
            {
                int extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
                SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle & ~WS_EX_TRANSPARENT);
            }
        }
        private async Task StartFixImage()
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            WindowsServices.SetWindowExTransparent(hwnd);
        }
        private async Task StopFixImage()
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            WindowsServices.ClearWindowExTransparent(hwnd);
        }

    }
}
