using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
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

        private readonly IEventBus _eventBus;
        private readonly CustomThemeDialogViewModel _customTheme;
        private readonly AppUserConfig _option;

        private SukiTheme _theme;
        public IAvaloniaReadOnlyList<IMenuPageViewModel> Menus { get; }
        public ObservableCollection<SukiColorTheme> Themes { get; } = new();

        private string _baseTheme = ThemeVariant.Default.ToString();
        public string BaseTheme
        {
            get => _baseTheme;
            set
            {
                var theme = GetThemeVariant(_baseTheme);
                this.RaiseAndSetIfChanged(ref _baseTheme, value);
                if (_option.Themes.Theme != theme)
                {
                    _option.Themes.Theme = theme;
                    _option.Save();
                }
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

        #region Command

        public ReactiveCommand<Unit, Unit> ToggleBaseThemeCommand { get; }
        public ReactiveCommand<Unit,Task> ToggleAnimationsCommand { get; }
        public ReactiveCommand<Unit, Unit> ToggleWindowLockCommand { get; }
        public ReactiveCommand<Unit, Unit> ToggleTitleBarCommand { get; }
        public ReactiveCommand<Unit, Unit> CreateCustomThemeCommand { get; }
        public ReactiveCommand<Unit, Unit> FullScreenCommand { get; }
        public ReactiveCommand<Unit, Unit> ExitFullScreenCommand { get; }
        public ReactiveCommand<Unit, Task> UserDetailCommand { get; }

        #endregion

        public MainViewModel(
            IOptions<AppUserConfig> option, 
            IControlCreator controlCreator,
            IEnumerable<IMenuPageViewModel> pages,
            CustomThemeDialogViewModel customTheme,
            IEventBus eventBus)
            : base(controlCreator, typeof(MainWindow), MainViewModel.Order)
        {
            _customTheme = customTheme;
            _customTheme.OnColorThemeChanged += colortheme => ColorThemeChanged(colortheme);
            _option = option.Value;
            Menus = new AvaloniaList<IMenuPageViewModel>(pages.OrderBy(p => p.Order));
            _theme = SukiTheme.GetInstance();
            //Themes = _theme.ColorThemes;
            Themes.AddRange(_theme.ColorThemes);
            BaseTheme = GetThemeVariant(_theme.ActiveBaseTheme);
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

            Avatar = LoadEmbeddedImage("NetX.AppCore.Assets.default_avatar.png");
            _eventBus = eventBus;

            InitConfig(_option);
        }

        private async Task UserDetail()
        {
            try
            {
                await _eventBus?.Publish(new UserInfoEvent("zeke"));
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
                BaseTheme = GetThemeVariant(variant);
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

        private string GetThemeVariant(ThemeVariant theme)
        {
            if(theme == ThemeVariant.Dark)
                return ThemeVariant.Light.ToString();
            else 
                return ThemeVariant.Dark.ToString();
        }

        private ThemeVariant GetThemeVariant(string theme)
        {
            return theme switch
            {
                "Light" => ThemeVariant.Light,
                "Dark" => ThemeVariant.Dark,
                _ => ThemeVariant.Default
            };
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
                Window.Closed += (s, e) => base.GotoWindow(-1);
            base.ControlLoaded();
        }
    }
}
