using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SukiUI.Controls;

namespace MyDemo.Views
{
    public partial class TestView : SukiWindow
    {
        public TestView()
        {
            this.AttachDevTools();
            AvaloniaXamlLoader.Load(this);
        }
    }
}
