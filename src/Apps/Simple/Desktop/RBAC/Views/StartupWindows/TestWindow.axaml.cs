using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SukiUI.Controls;

namespace NetX.RBAC
{
    public partial class TestWindow : SukiWindow
    {
        public TestWindow()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
