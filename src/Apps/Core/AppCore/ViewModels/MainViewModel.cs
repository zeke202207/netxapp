using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Styling;
using DynamicData;
using Material.Icons;
using Microsoft.Extensions.Options;
using NetX.AppCore.Contract;
using NetX.AppCore.Models;
using NetX.AppCore.Views;
using ReactiveUI;
using Serilog;
using SukiUI;
using SukiUI.Controls;
using SukiUI.Enums;
using SukiUI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TextMateSharp.Themes;

namespace NetX.AppCore.ViewModels
{
    [SortIndex(MainViewModel.Order)]
    [ViewModel(ServiceLifetime.Singleton)]
    public class MainViewModel : StartupWindowViewModel
    {
        public const int Order = int.MaxValue;
        private Guid id;

        private readonly IEventBus _eventBus;
        private readonly CustomThemeDialogViewModel _customTheme;
        private readonly AppUserConfig _option;

        private SukiTheme _theme;
        public IAvaloniaReadOnlyList<IMenuPageViewModel> Menus { get; }
        public ObservableCollection<SukiColorTheme> Themes { get; } = new();

        private ThemeVariant _baseTheme = ThemeVariant.Default;
        public ThemeVariant BaseTheme
        {
            get => _baseTheme;
            set
            {
                if(_option.Themes.Theme != value)
                {
                    _option.Themes.Theme = value;
                    _option.Save();
                }
                this.RaiseAndSetIfChanged(ref _baseTheme, value);
            }
        }

        private bool _animationsEnabled = true;
        public bool AnimationsEnabled
        {
            get => _animationsEnabled;
            set
            {
                this.RaiseAndSetIfChanged(ref _animationsEnabled, value);
                if (_option.Layouts.AnimationsEnabled != value)
                {
                    _option.Layouts.AnimationsEnabled = value;
                    _option.Save();
                }
            }
        }

        private bool _windowLocked = false;
        public bool WindowLocked
        {
            get => _windowLocked;
            set
            {
                this.RaiseAndSetIfChanged(ref _windowLocked, value);
                if(_option.Layouts.WindowLocked != value)
                {
                    _option.Layouts.WindowLocked = value;
                    _option.Save();
                }
            }
        }

        private bool _titleBarVisible = true;
        public bool TitleBarVisible
        {
            get => _titleBarVisible;
            set
            {
                this.RaiseAndSetIfChanged(ref _titleBarVisible, value);
                if(_option.Layouts.TitlebarVisible != value)
                {
                    _option.Layouts.TitlebarVisible = value;
                    _option.Save();
                }
            }
        }

        private bool _fullScreenVisible = false;
        public bool FullScreenVisible
        {
            get => _fullScreenVisible;
            set => this.RaiseAndSetIfChanged(ref _fullScreenVisible, value);
        }

        private Bitmap _avatar;
        public Bitmap Avatar
        {
            get => this._avatar;
            set => this.RaiseAndSetIfChanged(ref this._avatar, value);
        }

        private int _avatarSize = 80;
        public int AvatarSize
        {
            get => _avatarSize;
            set
            {
                this.RaiseAndSetIfChanged(ref _avatarSize, value);
            }
        }

        private string _userName = "zeke";
        public string UserName
        {
            get => _userName;
            set => this.RaiseAndSetIfChanged(ref _userName, value);
        }

        private Orientation _footerOrientation = Orientation.Horizontal;
        public Orientation FooterOrientation
        {
            get => _footerOrientation;
            set
            {
                this.RaiseAndSetIfChanged(ref _footerOrientation, value);
            }
        }

        #region Command

        public ReactiveCommand<Unit, Unit> ToggleBaseThemeCommand { get; }
        public ReactiveCommand<Unit,Task> ToggleAnimationsCommand { get; }
        public ReactiveCommand<Unit, Unit> ToggleWindowLockCommand { get; }
        public ReactiveCommand<Unit, Unit> ToggleTitleBarCommand { get; }
        public ReactiveCommand<Unit, Unit> CreateCustomThemeCommand { get; }
        public ReactiveCommand<Unit, Unit> FullScreenCommand { get; }
        public ReactiveCommand<Unit, Unit> ExitFullScreenCommand { get; }
        public ReactiveCommand<Unit, Task> UserDetailCommand { get; }
        public ReactiveCommand<Unit, Task> ChangePasswordCommand { get; }
        public ReactiveCommand<Unit, Task> ReLoginCommand { get; }
        public ReactiveCommand<Unit, Task> ExitAppCommand { get; }

        #endregion

        public MainViewModel(
            IOptions<AppUserConfig> option, 
            IControlCreator controlCreator,
            IEnumerable<IMenuPageViewModel> pages,
            CustomThemeDialogViewModel customTheme,
            IEventBus eventBus)
            : base(controlCreator, typeof(MainWindow), MainViewModel.Order)
        {
            id= Guid.NewGuid();
            _customTheme = customTheme;
            _customTheme.OnColorThemeChanged += colortheme => ColorThemeChanged(colortheme);
            _option = option.Value;
            Menus = new AvaloniaList<IMenuPageViewModel>(pages.OrderBy(p => p.Order));
            _theme = SukiTheme.GetInstance();
            //Themes = _theme.ColorThemes;
            Themes.AddRange(_theme.ColorThemes);
            BaseTheme = _theme.ActiveBaseTheme;
            _theme.OnBaseThemeChanged += async variant => await BaseThemeChanged(variant);
            _theme.OnColorThemeChanged += async theme => await SukiHost.ShowToast("Successfully Changed Color", $"Changed Color To {theme.DisplayName}.");
            _theme.OnBackgroundAnimationChanged += value => AnimationsEnabled = value;

            ToggleBaseThemeCommand = ReactiveCommand.Create(() => ToggleBaseTheme());
            ToggleAnimationsCommand = ReactiveCommand.Create<Task>(async () => await ToggleAnimations());
            ToggleWindowLockCommand = ReactiveCommand.Create(() => ToggleWindowLock());
            ToggleTitleBarCommand = ReactiveCommand.Create(() => ToggleTitleBar());
            CreateCustomThemeCommand = ReactiveCommand.Create(() => CustomerTheme());
            FullScreenCommand = ReactiveCommand.Create(() => ToggleFullScreen(!FullScreenVisible));
            ExitFullScreenCommand = ReactiveCommand.Create(() => ToggleFullScreen(false));
            UserDetailCommand = ReactiveCommand.Create(async () => await UserDetail());
            ChangePasswordCommand = ReactiveCommand.Create(() => ChangePassword());
            ReLoginCommand = ReactiveCommand.Create(() => ReLogin());
            ExitAppCommand = ReactiveCommand.Create(() => ExitApp());

            Avatar = LoadEmbeddedImage("NetX.AppCore.Assets.default_avatar.png");
            _eventBus = eventBus;

            InitConfig(_option);
        }

        #region Method

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        private async Task ChangePassword()
        {
            try
            {
                await _eventBus?.Publish(new UserInfoEvent(InfoType.ChangePassword,this));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "修改密码失败");
            }
        }

        /// <summary>
        /// 重新登录
        /// </summary>
        /// <returns></returns>
        private async Task ReLogin()
        {
            try
            {
                await _eventBus?.Publish(new UserInfoEvent(InfoType.Relogin, this));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "重新登录失败");
            }
        }

        /// <summary>
        /// 退出程序
        /// </summary>
        /// <returns></returns>
        private async Task ExitApp()
        {
            try
            {
                base.CloseApplication();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "退出应用失败");
            }
        }

        private async Task UserDetail()
        {
            try
            {
                await _eventBus?.Publish(new UserInfoEvent(InfoType.UserInfo, this));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "点击用户信息失败");
            }
        }

        private void ToggleTitleBar()
        {
            try
            {
                TitleBarVisible = !TitleBarVisible;
                SukiHost.ShowToast(
                    $"Title Bar {(TitleBarVisible ? "Visible" : "Hidden")}",
                    $"Window title bar has been {(TitleBarVisible ? "shown" : "hidden")}.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "设置菜单栏可见性失败");
            }
        }

        private void ToggleWindowLock()
        {
            try
            {
                WindowLocked = !WindowLocked;
                SukiHost.ShowToast(
                    $"Window {(WindowLocked ? "Locked" : "Unlocked")}",
                    $"Window has been {(WindowLocked ? "locked" : "unlocked")}.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "设置菜单栏锁定失败");
            }
        }

        private async Task ToggleAnimations()
        {
            try
            {
                AnimationsEnabled = !AnimationsEnabled;
                var title = AnimationsEnabled ? "Animation Enabled" : "Animation Disabled";
                var content = AnimationsEnabled
                    ? "Background animations are now enabled."
                    : "Background animations are now disabled.";
                await SukiHost.ShowToast(title, content);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "设置动画失败");
            }
        }

        private void ToggleBaseTheme()
        {
            try
            {
                _theme.SwitchBaseTheme();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "设置主题失败");
            }
        }

        private async Task BaseThemeChanged(ThemeVariant variant)
        {
            try
            {
                BaseTheme = variant;
                await SukiHost.ShowToast("Successfully Changed Theme", $"Changed Theme To {variant}");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "设置主题失败");
            }
        }

        private void ColorThemeChanged(SukiColorTheme colortheme)
        {
            try
            {
                List<SukiColorTheme> remove = new List<SukiColorTheme>();
                var defaultTheme = Enum.GetNames(typeof(SukiColor));
                for (int i = 0; i < Themes.Count; i++)
                {
                    var item = Themes[i];
                    if (defaultTheme.Contains(item.DisplayName))
                        continue;
                    remove.Add(item);
                }
                Themes.RemoveMany(remove);
                Themes.Add(colortheme);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "设置主题色失败");
            }
        }

        private void ToggleFullScreen(bool isFullScreen)
        {
            try
            {
                FullScreenVisible = isFullScreen;
                TitleBarVisible = !FullScreenVisible;
                if (null != base.Window)
                {
                    if (FullScreenVisible && base.Window.WindowState != WindowState.FullScreen)
                        base.Window.WindowState = WindowState.FullScreen;
                    else
                        base.Window.WindowState = WindowState.Normal;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "设置全屏切换失败");
            }
        }

        /// <summary>
        /// 初始化layout配置
        /// </summary>
        /// <param name="config"></param>
        private void InitConfig(AppUserConfig config)
        {
            this.AnimationsEnabled = config.Layouts.AnimationsEnabled;
            this.WindowLocked = config.Layouts.WindowLocked;
            this.TitleBarVisible = config.Layouts.TitlebarVisible;
        }

        private void CustomerTheme()
        {
            SukiHost.ShowDialog(_customTheme, allowBackgroundClose: true);
        }

        private Bitmap LoadEmbeddedImage(string imagePath)
        {
            var assembly = typeof(MainViewModel).Assembly;
            using (var stream = assembly.GetManifestResourceStream(imagePath))
            {
                if (stream != null)
                {
                    return new Bitmap(stream);
                }
            }
            return null;
        }

        #endregion

        public void GotoStartupWindow(int step)
        {
            base.GotoWindow(step);
            base.CloseApplication();
        }

        public void ChangeTheme(SukiColorTheme theme)
        {
            _theme.ChangeColorTheme(theme);
            if(_option.Themes.ThemeColor.DisplayName != theme.DisplayName)
            {
                _option.Themes.ThemeColor.DisplayName = theme.DisplayName;
                _option.Themes.ThemeColor.Primary = theme.Primary.ToString();
                _option.Themes.ThemeColor.Accent = theme.Accent.ToString();
                _option.Save();
            }
        }

        public override Control CreateView(IControlCreator controlCreator, Type pageView) => controlCreator.CreateControl(pageView);

        protected override void ControlLoaded()
        {
            if (null != Window)
                Window.Closed += (s, e) => base.GotoWindow(base.GotoStep);
            base.ControlLoaded();
        }
    }
}
