using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SukiUI.Controls;

namespace NetX.AppContainer.Views
{
    public partial class AppContainerView : SukiWindow
    {
        public AppContainerView()
        {
            this.AttachDevTools();
            AvaloniaXamlLoader.Load(this);
        }
    }
}
