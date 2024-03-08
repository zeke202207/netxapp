using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using NetX.AppCore.Contract;
using SukiUI.Controls;

namespace NetX.RBAC
{
    public partial class LoginWindow : SukiWindow
    {
        public LoginWindow()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
