﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.VisualTree;
using FluentAvalonia.UI.Controls;
using NetX.AppCore.Contract;
using NetX.RBAC.RPCService;
using ReactiveUI;
using Serilog;
using Splat;
using System.Reactive;
using System.Reactive.Linq;

namespace NetX.RBAC
{
    [ViewModel(ServiceLifetime.Transient)]
    public class LoginViewModel : StartupWindowViewModel
    {
        /// <summary>
        /// 全局唯一标识
        /// </summary>
        public static Guid Id = new Guid("00000000-0000-0000-0000-000000000002");

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

        private string _barTitle;
        public string BarTitle
        {
            get => _barTitle;
            set => this.RaiseAndSetIfChanged(ref _barTitle, value);
        }

        private bool _barIsOpen = false;
        public bool BarIsOpen
        {
            get => _barIsOpen;
            set => this.RaiseAndSetIfChanged(ref _barIsOpen, value);
        }

        private string _barMessage;
        public string BarMessage
        {
            get => _barMessage;
            set => this.RaiseAndSetIfChanged(ref _barMessage, value);
        }

        private InfoBarSeverity _barSeverity = InfoBarSeverity.Informational;
        public InfoBarSeverity BarSeverity
        {
            get => _barSeverity;
            set => this.RaiseAndSetIfChanged(ref _barSeverity, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public ReactiveCommand<Unit, Unit> LoginCommand { get; }
        public ReactiveCommand<Unit, Unit> LogoutCommand { get; }
        public ReactiveCommand<Unit, Task> RefreshCaptchaCommand { get; }

        private readonly IAccountRPC _accountRPC;

        public LoginViewModel(IServiceProvider serviceProvider, IAccountRPC accountRPC)
            : base(LoginViewModel.Id, serviceProvider, typeof(LoginWindow))
        {
            _accountRPC = accountRPC;
            LoginCommand = ReactiveCommand.Create(() => Login(), CanExecute());
            LogoutCommand = ReactiveCommand.Create(() => Logout());
            RefreshCaptchaCommand = ReactiveCommand.Create(async () => await RefreshCaptcha());
        }

        /// <summary>
        /// 等出系统
        /// </summary>
        private void Logout()
        {
            base.CloseApplication();
        }

        /// <summary>
        /// 登陆系统
        /// </summary>
        private async void Login()
        {
            try
            {
                BarIsOpen = false;
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
                    BarTitle = "登录失败";
                    BarMessage = loginResult.Message;
                    BarSeverity = InfoBarSeverity.Error;
                    BarIsOpen = true;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "登录失败");
            }
            finally
            {
                IsLoggingIn = false;
                Captcha = string.Empty;
                _captchaId = string.Empty;
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
            return this.WhenAnyValue(
                 x => x.UserName,
                 x => x.Password,
                 x => x.Captcha,
                 (u, p, c) => !string.IsNullOrWhiteSpace(u) && !string.IsNullOrWhiteSpace(p) && !string.IsNullOrWhiteSpace(c) && c.Length == 4)
                 .DistinctUntilChanged();
        }

        public override Control CreateView(IControlCreator controlCreator, Type pageView) => controlCreator.CreateControl(pageView);
    }
}
