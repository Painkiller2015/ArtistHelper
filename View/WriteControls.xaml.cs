using ArtistHelper.ButtonControls;
using Microsoft.VisualBasic.Devices;
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
using Xceed.Wpf.Toolkit;
using ArtistHelper.Model;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace ArtistHelper.View
{
    /// <summary>
    /// Логика взаимодействия для WriteControls.xaml
    /// </summary>
    public partial class WriteControls : Window
    {
        private static readonly Regex _regex = new Regex("[^0-9.-]+");
        public static event EventHandler<Brush> ChangeColorEvent;
        public static event EventHandler<int> ChangeBrushSizeEvent;
        public static event EventHandler<bool> ChangeEditorModeEvent;
        public WriteControls()
        {
            InitializeComponent();
        }

        private void ReSize(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(((TextBox)sender).Text))
            { 
                int brushSize = Convert.ToInt32(((TextBox)sender).Text);
                ContolPanelConfig.BrushesSize = brushSize;
                ChangeBrushSizeEvent.Invoke(this, brushSize);
            }
        }
        private void SwapColor(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            Brush brush = new SolidColorBrush((Color)e.NewValue);
            ContolPanelConfig.BrushesColor = brush;
            ChangeColorEvent.Invoke(this, brush);
        }

        private void ValidateText(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }

        private void EditorModeChange(object sender, RoutedEventArgs e)
        {
            ChangeEditorModeEvent.Invoke(this, (bool)((CheckBox)sender).IsChecked);
        }
        
    }
}
