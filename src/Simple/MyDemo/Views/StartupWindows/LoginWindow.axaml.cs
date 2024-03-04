using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using NetX.AppContainer.Contract;
using SukiUI.Controls;

namespace MyDemo.Views
{
    public partial class LoginWindow : SukiWindow
    {
        public LoginWindow()
        {
            this.AttachDevTools();
            AvaloniaXamlLoader.Load(this);
        }
    }
}
