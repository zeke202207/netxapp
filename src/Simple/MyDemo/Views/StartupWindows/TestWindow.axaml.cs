using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SukiUI.Controls;

namespace MyDemo.Views
{
    public partial class TestWindow : SukiWindow
    {
        public TestWindow()
        {
            this.AttachDevTools();
            AvaloniaXamlLoader.Load(this);
        }
    }
}
