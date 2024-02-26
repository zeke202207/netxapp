using Avalonia.Controls;
using SukiUI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetX.AppContainer.Extentions
{
    internal static class WindowsExtentions
    {
        internal static void NoTitle(this SukiWindow window)
        {
            window.IsTitleBarVisible = false;
            window.CanResize = false;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        internal static void NoTitle(this SukiWindow window, int width, int height)
        {
            window.Width = width;
            window.Height = height;
            window.NoTitle();
        }
    }
}
