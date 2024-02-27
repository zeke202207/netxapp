using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SukiUI.Controls;

namespace MyDemo.Views
{
    public partial class SplashScreenView : SukiWindow
    {
        public SplashScreenView()
        {
            this.AttachDevTools();
            AvaloniaXamlLoader.Load(this);
        }
    }
}
