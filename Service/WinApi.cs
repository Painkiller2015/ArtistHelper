using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ArtistHelper.Service
{
    internal class WinApi
    {
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        [DllImport("user32.dll")]
        static extern int GetWindowLong(IntPtr hwnd, int GWL_EXSTYLE);
        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

        static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        const int GWL_EXSTYLE = (-20);
        const UInt32 SWP_NOSIZE = 0x0001;
        const UInt32 SWP_NOMOVE = 0x0002;
        const UInt32 SWP_SHOWWINDOW = 0x0040;
        const long WS_EX_TOPMOST = 0x00000008L;
        const long WS_EX_NOREDIRECTIONBITMAP = 0x00200000L;      
        const int WS_EX_TOOLWINDOW = 0x00000080;

        public static void HideInAltTab(IntPtr hwnd)
        {
            int extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
            SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_TOOLWINDOW); //set
             //   SetWindowLong(hwnd, GWL_EXSTYLE, GetWindowLong(hwnd, GWL_EXSTYLE) & ~WS_EX_TOOLWINDOW);//off
        }
        public static void ShowInAltTab(IntPtr hwnd)
        {
            int extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
            SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle & ~WS_EX_TOOLWINDOW);//off
        }

        public static void SetTopWindow()
        {
            IntPtr hwnd = GetForegroundWindow();
           
            if (IsWindowTopMost(hwnd))
                SetWindowPos(hwnd, HWND_NOTOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW);
            else
                SetWindowPos(hwnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW);            
        }

        public static bool IsWindowTopMost(IntPtr Handle)
        {
            return (GetWindowLong(Handle, GWL_EXSTYLE) & WS_EX_TOPMOST) != 0;
        }
    }
}
