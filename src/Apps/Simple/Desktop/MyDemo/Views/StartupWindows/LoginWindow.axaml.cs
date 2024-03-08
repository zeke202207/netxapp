using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using NetX.AppCore.Contract;
using SukiUI.Controls;

namespace MyDemo.Views
{
    public partial class LoginWindow : SukiWindow
    {
        public LoginWindow()
        {
#if DEBUG
            this.AttachDevTools();
#endif
            AvaloniaXamlLoader.Load(this);
        }
    }
}
