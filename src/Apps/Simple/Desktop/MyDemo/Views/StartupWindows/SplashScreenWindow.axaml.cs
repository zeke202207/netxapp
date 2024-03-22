using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using FluentAvalonia.UI.Windowing;

namespace MyDemo.Views
{
    public partial class SplashScreenWindow : AppWindow
    {
        public SplashScreenWindow()
        {
            AvaloniaXamlLoader.Load(this);
#if DEBUG
            this.AttachDevTools();
#endif

        }
    }
}
