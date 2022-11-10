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
using System.Windows.Shapes;

namespace ArtistHelper.View
{
    /// <summary>
    /// Логика взаимодействия для ImageWindows.xaml
    /// </summary>
    public partial class ImageWindows : Window
    {
        public ImageWindows(BitmapImage image)
        {
            InitializeComponent();
            Image.Source = image;
        }

        private void ImageMove(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void MouseWheelHandler(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
                if (Image.Opacity < 1)
                    Image.Opacity += 0.1;

            if (e.Delta < 0)
                if (Image.Opacity > 0.2)
                    Image.Opacity -= 0.1;
        }
    }
}
