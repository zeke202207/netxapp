using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SukiUI.Controls;

namespace MyDemo.Views
{
    public partial class SplashScreenWindow : SukiWindow
    {
        public SplashScreenWindow()
        {
#if DEBUG
            this.AttachDevTools();
#endif
            AvaloniaXamlLoader.Load(this);
        }
    }
}
