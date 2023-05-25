using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace Student_Achievements.Classes
{
    public static class WindowHelper
    {
        private const int GwlStyle = -16;
        private const int WsSysmenu = 0x80000;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        public static void RemoveSysMenu(IntPtr hWnd)
        {
            int style = GetWindowLong(hWnd, GwlStyle);
            SetWindowLong(hWnd, GwlStyle, (style & ~WsSysmenu));
        }

        public static void InitializeSource(Window window)
        {
            window.SourceInitialized += (s, e) =>
            {
                window.MinWidth = window.ActualWidth;
                window.MinHeight = window.ActualHeight;
            };

            var handle = new WindowInteropHelper(window).Handle;
            RemoveSysMenu(handle);
        }
    }
}
