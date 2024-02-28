using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Media.Imaging;
using Avalonia.Styling;
using Material.Icons;
using Microsoft.Extensions.Options;
using NetX.AppContainer.Contract;
using NetX.AppContainer.Models;
using NetX.AppContainer.Views;
using ReactiveUI;
using SukiUI;
using SukiUI.Controls;
using SukiUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TextMateSharp.Themes;

namespace NetX.AppContainer.ViewModels
{
    [SortIndex(MainViewModel.Order)]
    [ViewModel(ServiceLifetime.Singleton)]
    public class MainViewModel : StartupWindowViewModel
    {
        public const int Order = int.MaxValue;

        private readonly IEventBus _eventBus;

        private readonly CustomThemeDialogViewModel _customTheme;

        private SukiTheme _theme;
        public IAvaloniaReadOnlyList<IMenuPageViewModel> Menus { get; }
        public IAvaloniaReadOnlyList<SukiColorTheme> Themes { get; }

        private ThemeVariant _baseTheme = ThemeVariant.Default;
        public ThemeVariant BaseTheme
        {
            get => _baseTheme;
            set => this.RaiseAndSetIfChanged(ref _baseTheme, value);
        }

        private bool _animationsEnabled = true;
        public bool AnimationsEnabled
        {
            get => _animationsEnabled;
            set => this.RaiseAndSetIfChanged(ref _animationsEnabled, value);
        }

        private bool _windowLocked = false;
        public bool WindowLocked
        {
            get => _windowLocked;
            set => this.RaiseAndSetIfChanged(ref _windowLocked, value);
        }

        private bool _titleBarVisible = true;
        public bool TitleBarVisible
        {
            get => _titleBarVisible;
            set => this.RaiseAndSetIfChanged(ref _titleBarVisible, value);
        }

        private bool _fullScreenVisible = false;
        public bool FullScreenVisible
        {
            get=> _fullScreenVisible;
            set => this.RaiseAndSetIfChanged(ref _fullScreenVisible, value);
        }

        private Bitmap _avatar;
        public Bitmap Avatar
        {
            get => this._avatar;
            set => this.RaiseAndSetIfChanged(ref  this._avatar, value);
        }

        #region Command

        public ReactiveCommand<Unit, Unit> ToggleBaseThemeCommand { get; }
        public ReactiveCommand<Unit,Task> ToggleAnimationsCommand { get; }
        public ReactiveCommand<Unit, Unit> ToggleWindowLockCommand { get; }
        public ReactiveCommand<Unit, Unit> ToggleTitleBarCommand { get; }
        public ReactiveCommand<Unit, Unit> CreateCustomThemeCommand { get; }
        public ReactiveCommand<Unit, Unit> FullScreenCommand { get; }
        public ReactiveCommand<Unit, Task> UserDetailCommand { get; }

        #endregion

        public MainViewModel(
            IOptions<AppConfig> option, 
            IControlCreator controlCreator,
            IEnumerable<IMenuPageViewModel> pages,
            CustomThemeDialogViewModel customTheme,
            IEventBus eventBus)
            : base(controlCreator, typeof(MainWindow), MainViewModel.Order)
        {
            _customTheme = customTheme;
            Menus = new AvaloniaList<IMenuPageViewModel>(pages.OrderBy(p => p.Order));
            _theme = SukiTheme.GetInstance();
            Themes = _theme.ColorThemes;
            BaseTheme = _theme.ActiveBaseTheme;
            _theme.OnBaseThemeChanged += async variant =>
            {
                BaseTheme = variant;
                await SukiHost.ShowToast("Successfully Changed Theme", $"Changed Theme To {variant}");
            };
            _theme.OnColorThemeChanged += async theme => await SukiHost.ShowToast("Successfully Changed Color", $"Changed Color To {theme.DisplayName}.");
            _theme.OnBackgroundAnimationChanged += value => AnimationsEnabled = value;

            ToggleBaseThemeCommand = ReactiveCommand.Create(() => _theme.SwitchBaseTheme());
            ToggleAnimationsCommand = ReactiveCommand.Create<Task>(() =>
            {
                AnimationsEnabled = !AnimationsEnabled;
                var title = AnimationsEnabled ? "Animation Enabled" : "Animation Disabled";
                var content = AnimationsEnabled
                    ? "Background animations are now enabled."
                    : "Background animations are now disabled.";
                return SukiHost.ShowToast(title, content);
            });
            ToggleWindowLockCommand = ReactiveCommand.Create(() =>
            {
                WindowLocked = !WindowLocked;
                SukiHost.ShowToast(
                    $"Window {(WindowLocked ? "Locked" : "Unlocked")}",
                    $"Window has been {(WindowLocked ? "locked" : "unlocked")}.");
            });
            ToggleTitleBarCommand = ReactiveCommand.Create(() =>
            {
                TitleBarVisible = !TitleBarVisible;
                SukiHost.ShowToast(
                    $"Title Bar {(TitleBarVisible ? "Visible" : "Hidden")}",
                    $"Window title bar has been {(TitleBarVisible ? "shown" : "hidden")}.");
            });
            CreateCustomThemeCommand = ReactiveCommand.Create(() => CustomerTheme());
            FullScreenCommand = ReactiveCommand.Create(() =>
            {
                FullScreenVisible = !FullScreenVisible;
                if (null != base.Window)
                {
                    base.Window.WindowState = base.Window.WindowState == WindowState.FullScreen ? WindowState.Normal : WindowState.FullScreen;
                    TitleBarVisible = base.Window.WindowState != WindowState.FullScreen;
                    SukiHost.ShowToast($"Full Screen {(FullScreenVisible ? "Enabled" : "Disabled")}",
                                       $"Window has been {(FullScreenVisible ? "enabled" : "disabled")}.");
                }
            });
            UserDetailCommand = ReactiveCommand.Create(async () => {
                await _eventBus?.Publish(new UserInfoEvent("zeke"));
            });

            Avatar = LoadEmbeddedImage("NetX.AppContainer.Assets.default_avatar.png");
            _eventBus = eventBus;
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

        public void ChangeTheme(SukiColorTheme theme) => _theme.ChangeColorTheme(theme);

        public override Control CreateView(IControlCreator controlCreator, Type pageView) => controlCreator.CreateControl(pageView);

        protected override void ControlLoaded()
        {
            if (null != Window)
                Window.Closed += (s, e) => base.GotoWindow(-1);
            base.ControlLoaded();
        }
    }
}
