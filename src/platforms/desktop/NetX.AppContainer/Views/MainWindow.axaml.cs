using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SukiUI.Controls;

namespace NetX.AppContainer.Views
{
    public partial class MainWindow : SukiWindow
    {
        public MainWindow()
        {
            this.AttachDevTools();
            AvaloniaXamlLoader.Load(this);
        }
    }
}
