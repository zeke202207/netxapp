using Avalonia;
using Avalonia.Automation.Peers;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media.Imaging;
using NetX.AppCore.Contract;
using NetX.RBAC.RPCService;
using ReactiveUI;
using Serilog;
using Splat;
using SukiUI.Controls;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;

namespace NetX.RBAC
{
    [SortIndex(LoginViewModel.Order, false)]
    [ViewModel(ServiceLifetime.Transient)]
    public class LoginViewModel : StartupWindowViewModel
    {
        public const int Order = 1;

        private bool _isLoggingIn;
        public bool IsLoggingIn
        {
            get => _isLoggingIn;
            set => this.RaiseAndSetIfChanged(ref _isLoggingIn, value);
        }

        private string _userName = "zeke";
        public string UserName
        {
            get => _userName;
            set => this.RaiseAndSetIfChanged(ref _userName, value);
        }

        private string _password = "123456";
        public string Password
        {
            get => _password;
            set => this.RaiseAndSetIfChanged(ref _password, value);
        }

        private string _captcha;
        public string Captcha
        {
            get => _captcha;
            set => this.RaiseAndSetIfChanged(ref _captcha, value);
        }

        private Bitmap captchaImage;
        public Bitmap CaptchaImage
        {
            get => captchaImage;
            set => this.RaiseAndSetIfChanged(ref captchaImage, value);
        }

        private string _captchaId;

        /// <summary>
        /// 
        /// </summary>
        public ICommand LoginCommand { get; }
        public ReactiveCommand<Unit, Task> RefreshCaptchaCommand { get; }

        private readonly IAccountRPC _accountRPC;

        public LoginViewModel(IControlCreator controlCreator, IAccountRPC accountRPC)
            : base(controlCreator, typeof(LoginWindow), LoginViewModel.Order)
        {
            _accountRPC = accountRPC;
            LoginCommand = ReactiveCommand.Create(() => Login(), CanExecute());
            RefreshCaptchaCommand = ReactiveCommand.Create(async () => await RefreshCaptcha());
        }

        /// <summary>
        /// 登陆系统
        /// </summary>
        private async void Login()
        {
            try
            {
                IsLoggingIn = true;
                var loginResult = await _accountRPC.LoginAsync(new LoginModel()
                {
                    UserName = UserName,
                    Password = Password,
                    Captcha = Captcha,
                    CaptchaId = _captchaId
                });
                if (loginResult.Success)
                    GotoNextWindow();
                else
                {
                    SukiHost.ShowDialog(new DialogMessageViewModel(base._controlCreator)
                    {
                        MessageType = DialogMessageType.Error,
                        Message = loginResult.Message,
                    }, allowBackgroundClose: false);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "登录失败");
            }
            finally
            {
                IsLoggingIn = false;
            }
        }

        /// <summary>
        /// 刷新验证码
        /// </summary>
        /// <returns></returns>
        private async Task RefreshCaptcha()
        {
            try
            {
                //CaptchaImage = new Bitmap($"{Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"test.png")}");
                var captcha = await _accountRPC.GetCaptchaAsync();
                _captchaId = captcha.CaptchaId;
                CaptchaImage = Base64StringToBitmap(captcha.CaptchaBase64);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "刷新验证码出错");
            }
        }

        private Bitmap Base64StringToBitmap(string base64String)
        {
            // 将Base64字符串解码为字节数组
            byte[] bytes = Convert.FromBase64String(base64String);
            // 使用字节数组创建一个内存流
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                // 从内存流创建Bitmap
                return new Bitmap(ms);
            }
        }

        private IObservable<bool>? CanExecute()
        {
            return Observable.Return(true);
        }

        public override Control CreateView(IControlCreator controlCreator, Type pageView) => controlCreator.CreateControl(pageView);
    }
}
