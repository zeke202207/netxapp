using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SukiUI.Controls;

namespace MyDemo.Views
{
    public partial class LoginView : SukiWindow
    {
        public LoginView()
        {
            this.AttachDevTools();
            AvaloniaXamlLoader.Load(this);
        }
    }
}
