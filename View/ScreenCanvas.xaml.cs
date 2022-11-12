using ArtistHelper.ButtonControls;
using ArtistHelper.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
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
        public bool StartProcess { get; set; }
        private GlobalHotKeyManager _GHKManager = new();
        private static CancellationTokenSource? sourse = null;
        private InkCanvasEditingMode _inkCanvasEditingMode;

        public ScreenCanvas()
        {
            InitializeComponent();
            this.Left = SystemParameters.VirtualScreenLeft;
            this.Top = SystemParameters.VirtualScreenTop;
            HotKeyInit();

            WriteControls.ChangeColorEvent += async (obj, color) =>
            {
                Color newColor = ((SolidColorBrush)color).Color;
                CanvasLayer.DefaultDrawingAttributes.Color = newColor;

            };
            WriteControls.ChangeBrushSizeEvent += async (obj, size) =>
            {
                CanvasLayer.DefaultDrawingAttributes.Width = size;             
            };
            WriteControls.ChangeEditorModeEvent += async (obj, delLine) =>
            {
                if (delLine)
                {
                    _inkCanvasEditingMode = InkCanvasEditingMode.EraseByStroke;
                }
                else
                {
                    _inkCanvasEditingMode = InkCanvasEditingMode.EraseByPoint;
                }



            };
        }
        private void HotKeyInit()
        {
            _GHKManager.StatusProcessEvent += async (obj, status) =>
            {
                StartProcess = status;

                if (StartProcess)
                {
                    if (sourse == null)
                    {
                        sourse = new CancellationTokenSource();
                        await StartArtProccess();
                    }
                }
                else
                {
                    if (sourse != null)
                    {
                        sourse.Cancel();
                        sourse = null;
                        await StopArtProccess();
                    }
                }
            };
        }
        private async Task StartArtProccess()
        {
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
        private async Task StopArtProccess()
        {               
            this.Focusable = false;
            CanvasLayer.EditingMode = InkCanvasEditingMode.None;
            SetColor("#00000000");

        }

        private void SetColor(string HEXcolor)
        {
            var bc = new BrushConverter();
            CanvasLayer.Background = (Brush)bc.ConvertFrom(HEXcolor);
        }
    }
}
