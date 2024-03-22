using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Serilog;
using System.Reactive.Linq;

namespace NetX.RBAC
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            AvaloniaXamlLoader.Load(this);
        }

        protected override void OnDataContextEndUpdate()
        {
            base.OnDataContextEndUpdate();
            try
            {
                RefreshCaptch();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "刷新验证码出错");
            }
        }

        private void Captcha_Tapped(object? sender, Avalonia.Input.TappedEventArgs e)
        {
            try
            {
                RefreshCaptch();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "刷新验证码出错");
            }
        }

        private void RefreshCaptch()
        {
            if (DataContext is not LoginViewModel vm)
                return;
            var task = vm.RefreshCaptchaCommand?.Execute();
            task.GetAwaiter().GetResult();
        }
    }
}
