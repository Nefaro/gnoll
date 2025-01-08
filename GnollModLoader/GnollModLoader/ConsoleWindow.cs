using System;
using System.Runtime.InteropServices;

namespace GnollModLoader
{
    // http://stackoverflow.com/a/15079092/2524350
    class ConsoleWindow
    {
        public static void ShowConsoleWindow()
        {
            var handle = GetConsoleWindow();
            if (handle == IntPtr.Zero)
            {
                AllocConsole();
            }
            else
            {
                MoveWindow(handle, 50, 100, 1250, 500, true);
                ShowWindow(handle, SW_SHOW);
            }
        }

        public static void HideConsoleWindow()
        {
            var handle = GetConsoleWindow();

            ShowWindow(handle, SW_HIDE);
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool MoveWindow(IntPtr hwnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;
    }
}
