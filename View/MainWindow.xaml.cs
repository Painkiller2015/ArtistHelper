using ArtistHelper.View;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
//using System.Windows.Forms;
using System.Drawing;
using Microsoft.VisualBasic;
using ArtistHelper.ButtonControls;
using ArtistHelper.Service;

namespace ArtistHelper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CreateControlPanel();
            CreateCanvasLayer();
        }

        private static void CreateCanvasLayer()
        {
            ScreenCanvas sv = new()
            {
                Height = SysConfig.GetHeightScreens(),
                Width = SysConfig.GetWidthScreens()                
            };
            
            sv.Show();
        }

        private static void CreateControlPanel()
        {
            WriteControls wc = new();
            wc.Show();
        }

        private void ImageMove(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
        private void Image_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                string inputSize = Interaction.InputBox("Введите размер изоображения (100 100)");
                string[] size = inputSize.Split(' ');
              
                int width = Convert.ToInt32(size[0]);
                int height = Convert.ToInt32(size[1]);

                try
                {
                    foreach (var imagePath in files)
                    {
                        BitmapImage image = new(new Uri(imagePath));
                        ImageWindows imgWin = new(image)
                        {
                            Width = width,
                            Height = height
                        };
                        imgWin.Show(); 
                        imgWin.Activate();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ЧТот не получилось: \n" + ex);
                    throw;
                }

            }
        }
    }

}