using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SukiUI.Controls;

namespace NetX.AppContainer.Views
{
    public partial class AppContainerWindow : SukiWindow
    {
        public AppContainerWindow()
        {
            this.AttachDevTools();
            AvaloniaXamlLoader.Load(this);
        }
    }
}
