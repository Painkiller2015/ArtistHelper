﻿using ArtistHelper.ButtonControls;
using ArtistHelper.Service;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
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
using System.IO;
using System.Windows.Forms;


namespace ArtistHelper.View
{
    /// <summary>
    /// Логика взаимодействия для ImageWindows.xaml
    /// </summary>
    public partial class ImageWindows : Window
    {
        private GlobalHotKeyManager _GHKManager = new();
        private bool _CtrlButtonIsPressed;
        private bool _AltButtonIsPressed;
        private double _monitorAspectRatio;
        private double _scrollStepWidth;
        private double _scrollStepHeight;
        private double _angle;
        private bool _isMirrored = false;
        private RotateTransform _rotateTransform = new();
        private ScaleTransform _scaleTransform = new();
        public ImageWindows(BitmapImage image)
        {
            InitializeComponent();
            Image.Height = image.Height;
            Image.Width = image.Width;
            Image.Source = image;

            _GHKManager.ImageFixEvent += async (obj, needFixImage) =>
            {
                if (needFixImage)
                    await StartFixImage();
                if (!needFixImage)
                    await StopFixImage();
            };
            _GHKManager.CtrlButtonPressEvent += async (obj, isPressed) =>
            {
                _CtrlButtonIsPressed = isPressed;
            };
            _GHKManager.AltButtonPressEvent += async (obj, isPressed) =>
            {
                _AltButtonIsPressed = isPressed;
            };
            _GHKManager.MirrorImage += async (obj, mirror) =>
            {
                if (_isMirrored)
                    _scaleTransform.ScaleX = 1;
                else
                    _scaleTransform.ScaleX = -1;

                SetRender(_rotateTransform, _scaleTransform);
                _isMirrored = !_isMirrored;
            };
        }

        private void ImageMove(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
        private void MouseWheelHandler(object sender, MouseWheelEventArgs e)
        {
            if (_CtrlButtonIsPressed)
            {
                Screen currentScreen = this.CurrentScreen();
                _monitorAspectRatio = Image.Source.Width / Image.Source.Height;
                _scrollStepWidth = currentScreen.Bounds.Width / 100.00;
                _scrollStepHeight = _scrollStepWidth / _monitorAspectRatio;

                if (e.Delta > 0)
                {
                    Image.Width += _scrollStepWidth;
                    Image.Height += _scrollStepHeight;
                }
                if (e.Delta < 0)
                {
                    Image.Width -= _scrollStepWidth;
                    Image.Height -= _scrollStepHeight;
                }
            }
            if (_AltButtonIsPressed)
            {
                if (e.Delta > 0)
                    _angle += 1;
                if (e.Delta < 0)
                    _angle -= 1;

                _rotateTransform.Angle = _angle;
                SetRender(_rotateTransform, _scaleTransform);
            }
            if (!_CtrlButtonIsPressed && !_AltButtonIsPressed)
            {
                if (e.Delta > 0)
                    if (Image.Opacity < 1)
                        Image.Opacity += 0.1;
                if (e.Delta < 0)
                    if (Image.Opacity > 0.2)
                        Image.Opacity -= 0.1;
            }
        }
        private void SetRender(params Transform[] transforms)
        {
            TransformGroup multiTranform = new();

            foreach (var transform in transforms)
                multiTranform.Children.Add(transform);

            Image.RenderTransform = multiTranform;
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
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            var hwnd = new WindowInteropHelper(this).Owner;
            WindowsServices.SetWindowExTransparent(hwnd);
        }
    }
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
}
